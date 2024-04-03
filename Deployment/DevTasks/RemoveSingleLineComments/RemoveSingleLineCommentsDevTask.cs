using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.RemoveSingleLineComments
{
    public class RemoveSingleLineCommentsDevTask : BaseDeploymentTask
    {
        public RemoveSingleLineCommentsDevTask(RemoveSingleLineCommentsDevTaskOptions options)
            : this(options, null)
        { 
        }

        public RemoveSingleLineCommentsDevTask(RemoveSingleLineCommentsDevTaskOptions options, IDeploymentTask parentTask)
            : base("ECEC71CC-5AA3-4EC1-88CE-6455586E369E", false, options, parentTask)
        {
            _options = options;
        }

        private readonly RemoveSingleLineCommentsDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateList(nameof(_options.TargetDirsList), _options.TargetDirsList);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            foreach(var targetDir in _options.TargetDirsList)
            {
                ProcessDir(targetDir);
            }
        }

        private void ProcessDir(string dirName)
        {
#if DEBUG
            //_logger.Info($"dirName = '{dirName}'");
#endif

            var lowerDirName = dirName.ToLower();

#if DEBUG
            //_logger.Info($"lowerDirName = '{lowerDirName}'");
#endif

            if(lowerDirName.EndsWith(".git"))
            {
                return;
            }

            if (lowerDirName.EndsWith(".github"))
            {
                return;
            }

            if (lowerDirName.EndsWith(".vs"))
            {
                return;
            }

            if (lowerDirName.EndsWith("bin"))
            {
                return;
            }

            if (lowerDirName.EndsWith("obj"))
            {
                return;
            }

#if DEBUG
            //_logger.Info($"dirName = '{dirName}'");
            //_logger.Info($"lowerDirName = '{lowerDirName}'");
#endif

            var subDirs = Directory.GetDirectories(dirName);

            foreach(var subDir in subDirs)
            {
#if DEBUG
                //_logger.Info($"subDir = '{subDir}'");
#endif

                ProcessDir(subDir);
            }

            var csFiles = Directory.GetFiles(dirName).Where(p => p.EndsWith(".cs")).ToList();

            foreach(var csFile in csFiles)
            {
#if DEBUG
                //_logger.Info($"csFile = '{csFile}'");
#endif

                ProcessFile(csFile);
            }
        }

        private void ProcessFile(string fileName)
        {
#if DEBUG
            //_logger.Info($"fileName = '{fileName}'");
#endif

            var rowsList = File.ReadAllLines(fileName);

            var rowsWithoutSingleLineCommentsList = new List<string>();

            foreach (var row in rowsList)
            {
#if DEBUG
                //_logger.Info($"row = '{row}'");
#endif
                var trimmedRow = row.Trim();

#if DEBUG
                //_logger.Info($"trimmedRow = '{trimmedRow}'");
#endif
                if (trimmedRow.StartsWith("//") && !trimmedRow.StartsWith("///"))
                {
                    continue;
                }

                rowsWithoutSingleLineCommentsList.Add(row);
            }

#if DEBUG
            //_logger.Info("------------------------------------------------------------------------------");
#endif

            List<string> rowsWithoutPreprocessorConditonalsList = null;


            while (true)
            {
                var processItemsResult = RemoveEmptyPreprocessorsStatements(rowsWithoutSingleLineCommentsList);

#if DEBUG
                //_logger.Info($"processItemsResult.Item1 = {processItemsResult.Item1}");
#endif

                if(processItemsResult.Item1)
                {
                    rowsWithoutSingleLineCommentsList = processItemsResult.Item2;
                }
                else
                {
                    rowsWithoutPreprocessorConditonalsList = processItemsResult.Item2;
                    break;
                }                
            }

#if DEBUG
            //_logger.Info("======================================");
#endif
//            foreach (var row in rowsWithoutPreprocessorConditonalsList)
//            {
//#if DEBUG
//                _logger.Info($"row = '{row}'");
//#endif
//            }

            File.WriteAllLines(fileName, rowsWithoutPreprocessorConditonalsList);
        }

        private (bool, List<string>) RemoveEmptyPreprocessorsStatements(List<string> rows)
        {
            var rowsWithoutPreprocessorConditonalsList = new List<string>();

            var wasSkippedSharpIf = false;
            var wasSkippedEmptyRowAfterSkippedSharpIf = false;

            var n = -1;

            var wasChangedSmth = false;

            foreach (var row in rows)
            {
                n++;

#if DEBUG
                //_logger.Info($"wasSkippedSharpIf = {wasSkippedSharpIf}");
                //_logger.Info($"wasSkippedEmptyRowAfterSkippedSharpIf = {wasSkippedEmptyRowAfterSkippedSharpIf}");

                //_logger.Info($"n = {n}");
                //_logger.Info($"row = '{row}'");
#endif
                var trimmedRow = row.Trim().ToLower();

#if DEBUG
                //_logger.Info($"trimmedRow = '{trimmedRow}'");
#endif
                if (trimmedRow.StartsWith("#if"))
                {
                    var nextTrimmedRow = rows[n + 1].Trim().ToLower();

#if DEBUG
                    //_logger.Info($"nextTrimmedRow = '{nextTrimmedRow}'");
#endif
                    if (nextTrimmedRow.StartsWith("#endif"))
                    {
                        wasSkippedSharpIf = true;
                        wasChangedSmth = true;

                        continue;
                    }
                    else
                    {
#if DEBUG
                        //_logger.Info($"row (added) = '{row}'");
#endif
                        rowsWithoutPreprocessorConditonalsList.Add(row);
                    }
                }
                else
                {
                    if (trimmedRow.StartsWith("#endif") && wasSkippedSharpIf)
                    {
                        wasSkippedSharpIf = false;
#if DEBUG
                        //_logger.Info($"{n + 1} < {rows.Count} = {n + 1 < rows.Count}");
#endif
                        if (n + 1 < rows.Count)
                        {
                            var nextTrimmedRow = rows[n + 1].Trim().ToLower();
#if DEBUG
                            //_logger.Info($"nextTrimmedRow = '{nextTrimmedRow}'");
#endif
                            if (string.IsNullOrWhiteSpace(nextTrimmedRow))
                            {
                                wasSkippedEmptyRowAfterSkippedSharpIf = true;
                                wasChangedSmth = true;
                            }
                        }

                        continue;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(trimmedRow) && wasSkippedEmptyRowAfterSkippedSharpIf)
                        {
                            wasSkippedEmptyRowAfterSkippedSharpIf = false;

                            continue;
                        }
                        else
                        {
#if DEBUG
                            //_logger.Info($"row (added) = '{row}'");
#endif
                            rowsWithoutPreprocessorConditonalsList.Add(row);
                        }
                    }
                }
            }

            return (wasChangedSmth, rowsWithoutPreprocessorConditonalsList);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpaces = DisplayHelper.Spaces(nextN);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Removes single line comments in:");
            foreach (var item in _options.TargetDirsList)
            {
                sb.AppendLine($"{nextNSpaces}{item}");
            }

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
