using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using XMLDocReader;

namespace TestSandBox.XMLDoc
{
    public delegate void OnD();

    public class ReadXMLDocHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            //var path = Environment.GetEnvironmentVariable("PATH");

            //_logger.Info($"path = {path}");

            //Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + Directory.GetCurrentDirectory());

            var targetXMLDocFileName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.UnityAsset.Core.xml");

            _logger.Info($"targetXMLDocFileName = {targetXMLDocFileName}");

            var tmpType = typeof(OnD);

            _logger.Info($"type.IsClass = {tmpType.IsClass}");
            _logger.Info($"type.IsInterface = {tmpType.IsInterface}");
            _logger.Info($"type.IsEnum = {tmpType.IsEnum}");

            _logger.Info($"typeof(Delegate).IsAssignableFrom(tmpType) = {typeof(Delegate).IsAssignableFrom(tmpType)}");
            
            _logger.Info($"GetType().FullName = {GetType().FullName}");

            var typesDict = new Dictionary<string, Type>();

            var coreHelperAssemblyFileName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClayCoreHelper.dll");

            _logger.Info($"coreHelperAssemblyFileName = {coreHelperAssemblyFileName}");

            var coreHelperAssembly = Assembly.LoadFrom(coreHelperAssemblyFileName);

            foreach (var type in coreHelperAssembly.GetTypes())
            {
                //_logger.Info($"type.FullName = {type.FullName}");

                typesDict[type.FullName] = type;
            }

            var coreAssemblyFileName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.Core.dll");

            _logger.Info($"coreAssemblyFileName = {coreAssemblyFileName}");

            var coreAssembly = Assembly.LoadFrom(coreAssemblyFileName);

            var targetAssemblyFileName = Path.Combine(Directory.GetCurrentDirectory(), "SymOntoClay.UnityAsset.Core.dll");

            var targetAssembly = Assembly.LoadFrom(targetAssemblyFileName);

            //_logger.Info($"AppDomain.CurrentDomain.GetAssemblies() = {JsonConvert.SerializeObject(AppDomain.CurrentDomain.GetAssemblies().Select(p => p.GetName().Name), Formatting.Indented)}");

            foreach (var type in targetAssembly.GetTypes())
            {
                //_logger.Info($"type.FullName = {type.FullName}");

                typesDict[type.FullName] = type;
            }

            var memberCardsList = XMLMemberCardsReader.Read(targetXMLDocFileName);

            _logger.Info($"memberCardsList.Count = {memberCardsList.Count}");

            var cardsOfTypesList = new List<XMLMemberCard>();
            var cardsOfMembersList = new List<XMLMemberCard>();

            foreach (var memberCard in memberCardsList)
            {
                if(memberCard.Name.Kind == KindOfMember.Type)
                {
                    cardsOfTypesList.Add(memberCard);
                }
                else
                {
                    cardsOfMembersList.Add(memberCard);
                }
            }

            _logger.Info($"cardsOfTypesList.Count = {cardsOfTypesList.Count}");
            _logger.Info($"cardsOfMembersList.Count = {cardsOfMembersList.Count}");

            var cardsOfMembersDict = cardsOfMembersList.GroupBy(p => p.Name.Path).ToDictionary(p => p.Key, p => p.ToList());

            _logger.Info($"cardsOfMembersDict.Count = {cardsOfMembersDict.Count}");

            foreach(var typeCard in cardsOfTypesList)
            {
                _logger.Info($"typeCard = {typeCard}");

                typeCard.IsProcessed = true;

                var fullName = typeCard.Name.FullName;

                var type = typesDict[fullName];

                _logger.Info($"type.IsClass = {type.IsClass}");
                _logger.Info($"type.IsInterface = {type.IsInterface}");
                _logger.Info($"type.IsEnum = {type.IsEnum}");
                //_logger.Info($" = {}");

                //if(cardsOfMembersDict.ContainsKey(fullName))
                //{
                //    var membersList = cardsOfMembersDict[fullName];

                //    _logger.Info($"membersList.Count = {membersList.Count}");

                //    foreach(var memberCard in membersList)
                //    {
                //        _logger.Info($"memberCard = {memberCard}");
                //    }
                //}
            }

            foreach(var memberCard in cardsOfMembersList)
            {
                _logger.Info($"memberCard = {memberCard}");
            }

            _logger.Info("End");
        }

        public void ParseGenericType()
        {
            _logger.Info("Begin");

            var fullTypeName = "System.Collections.Generic.IList{SymOntoClay.UnityAsset.Core.DeviceOfBiped}";

            var figureBracketPos = fullTypeName.IndexOf("{");

            _logger.Info($"figureBracketPos = {figureBracketPos}");

            var name = fullTypeName.Substring(0, figureBracketPos);

            _logger.Info($"name = {name}");

            var strWithParameters = fullTypeName.Substring(figureBracketPos + 1, fullTypeName.Length - figureBracketPos - 2);

            _logger.Info($"strWithParameters = {strWithParameters}");

            var parametersList = MemberNameParser.GetParametersList(strWithParameters);

            _logger.Info($"parametersList = {JsonConvert.SerializeObject(parametersList, Formatting.Indented)}");

            _logger.Info("End");
        }
    }
}
