using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public static class PackageCardCleaner
    {
        //private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Clean(List<PackageCard> packageCardsList, string targetRootTypeName, RepackingTypeCardOptions repackingTypeCardOptions)
        {
            Clean(packageCardsList, new List<string>() { targetRootTypeName }, repackingTypeCardOptions);
        }

        public static void Clean(List<PackageCard> packageCardsList, List<string> targetRootTypeNamesList, RepackingTypeCardOptions repackingTypeCardOptions)
        {
            //_logger.Info($"targetRootTypeNamesList = {JsonConvert.SerializeObject(targetRootTypeNamesList, Formatting.Indented)}");

            targetRootTypeNamesList = NormalizeTargetRootTypeNamesList(targetRootTypeNamesList);

            //_logger.Info($"targetRootTypeNamesList (after) = {JsonConvert.SerializeObject(targetRootTypeNamesList, Formatting.Indented)}");

            var classesList = new List<ClassCard>();
            var interfacesList = new List<ClassCard>();
            var enumsList = new List<EnumCard>();

            foreach (var packageCard in packageCardsList)
            {
                classesList.AddRange(packageCard.ClassesList);
                interfacesList.AddRange(packageCard.InterfacesList);
                enumsList.AddRange(packageCard.EnumsList);
            }

            //_logger.Info($"classesList.Count = {classesList.Count}");
            //_logger.Info($"interfacesList.Count = {interfacesList.Count}");
            //_logger.Info($"enumsList.Count = {enumsList.Count}");

            var classesInitialNamesDict = classesList.ToDictionary(p => p.Name.InitialName, p => p);
            var interfacesInitialNamesDict = interfacesList.ToDictionary(p => p.Name.InitialName, p => p);
            var enumsInitialNamesDict = enumsList.ToDictionary(p => p.Name.InitialName, p => p);

            //_logger.Info($"classesInitialNamesDict.Count = {classesInitialNamesDict.Count}");
            //_logger.Info($"interfacesInitialNamesDict.Count = {interfacesInitialNamesDict.Count}");
            //_logger.Info($"enumsInitialNamesDict.Count = {enumsInitialNamesDict.Count}");

            var targetInitialTypeNames = new List<string>();

            foreach(var targetRootTypeName in targetRootTypeNamesList)
            {
                SearchTargetNamesForRepackingTypeCard(targetRootTypeName, repackingTypeCardOptions, targetInitialTypeNames, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
            }

            //_logger.Info($"targetInitialTypeNames = {JsonConvert.SerializeObject(targetInitialTypeNames, Formatting.Indented)}");

            RemoveItemsFromPackageCardsListForRepackingTypeCard(packageCardsList, targetInitialTypeNames, repackingTypeCardOptions);

            //_logger.Info("End");
        }

        private static List<string> NormalizeTargetRootTypeNamesList(List<string> targetRootTypeNamesList)
        {
            var result = new List<string>();

            foreach(var targetRootTypeName in targetRootTypeNamesList)
            {
                if (targetRootTypeName.StartsWith("T:"))
                {
                    result.Add(targetRootTypeName);
                }
                else
                {
                    result.Add($"T:{targetRootTypeName}");
                }
            }

            return result;
        }

        private static void RemoveItemsFromPackageCardsListForRepackingTypeCard(List<PackageCard> packageCardsList, List<string> targetInitialTypeNames, RepackingTypeCardOptions repackingTypeCardOptions)
        {
            //_logger.Info($"packageCardsList.Count = {packageCardsList.Count}");

            foreach (var packageCard in packageCardsList.ToList())
            {
                //_logger.Info($"packageCard.NamespacesList.Count = {packageCard.NamespacesList.Count}");
                //_logger.Info($"packageCard.ClassesList.Count = {packageCard.ClassesList.Count}");
                //_logger.Info($"packageCard.InterfacesList.Count = {packageCard.InterfacesList.Count}");
                //_logger.Info($"packageCard.EnumsList.Count = {packageCard.EnumsList.Count}");

                RemoveItemsFromPackageCardForRepackingTypeCard(packageCard, targetInitialTypeNames, repackingTypeCardOptions);

                //_logger.Info($"packageCard.NamespacesList.Count (after) = {packageCard.NamespacesList.Count}");
                //_logger.Info($"packageCard.ClassesList.Count (after) = {packageCard.ClassesList.Count}");
                //_logger.Info($"packageCard.InterfacesList.Count (after) = {packageCard.InterfacesList.Count}");
                //_logger.Info($"packageCard.EnumsList.Count (after) = {packageCard.EnumsList.Count}");

                if (!packageCard.NamespacesList.Any() && !packageCard.ClassesList.Any() && !packageCard.InterfacesList.Any() && !packageCard.EnumsList.Any())
                {
                    packageCardsList.Remove(packageCard);
                }
            }

            //_logger.Info($"packageCardsList.Count (after) = {packageCardsList.Count}");
        }

        private static void RemoveItemsFromPackageCardForRepackingTypeCard(PackageCard packageCard, List<string> targetInitialTypeNames, RepackingTypeCardOptions repackingTypeCardOptions)
        {
            //_logger.Info($"packageCard.AssemblyName = {packageCard.AssemblyName}");

            //_logger.Info($"packageCard.NamespacesList.Count = {packageCard.NamespacesList.Count}");

            foreach (var namespaceCard in packageCard.NamespacesList.Where(p => p.Parent == null).ToList())
            {
                //_logger.Info($"namespaceCard.NamespacesList.Count = {namespaceCard.NamespacesList.Count}");
                //_logger.Info($"namespaceCard.ClassesList.Count = {namespaceCard.ClassesList.Count}");
                //_logger.Info($"namespaceCard.InterfacesList.Count = {namespaceCard.InterfacesList.Count}");
                //_logger.Info($"namespaceCard.EnumsList.Count = {namespaceCard.EnumsList.Count}");

                RemoveItemsFromNamespaceForRepackingTypeCard(packageCard, namespaceCard, targetInitialTypeNames, repackingTypeCardOptions);

                //_logger.Info($"namespaceCard.NamespacesList.Count (after) = {namespaceCard.NamespacesList.Count}");
                //_logger.Info($"namespaceCard.ClassesList.Count (after) = {namespaceCard.ClassesList.Count}");
                //_logger.Info($"namespaceCard.InterfacesList.Count (after) = {namespaceCard.InterfacesList.Count}");
                //_logger.Info($"namespaceCard.EnumsList.Count (after) = {namespaceCard.EnumsList.Count}");

                if (!namespaceCard.NamespacesList.Any() && !namespaceCard.ClassesList.Any() && !namespaceCard.InterfacesList.Any() && !namespaceCard.EnumsList.Any())
                {
                    //_logger.Info($"namespaceCard.Name (for removing) = {namespaceCard.Name}");

                    packageCard.NamespacesList.Remove(namespaceCard);
                }
            }

            //_logger.Info($"packageCard.NamespacesList.Count (after) = {packageCard.NamespacesList.Count}");
        }

        private static void RemoveItemsFromNamespaceForRepackingTypeCard(PackageCard packageCard, NamespaceCard namespaceCard, List<string> targetInitialTypeNames, RepackingTypeCardOptions repackingTypeCardOptions)
        {
            //_logger.Info($"namespaceCard.Name = {namespaceCard.Name}");

            //_logger.Info($"namespaceCard.NamespacesList.Count = {namespaceCard.NamespacesList.Count}");

            if (namespaceCard.NamespacesList.Any())
            {
                foreach (var childNamespaceCard in namespaceCard.NamespacesList.ToList())
                {
                    //_logger.Info($"childNamespaceCard.NamespacesList.Count = {childNamespaceCard.NamespacesList.Count}");
                    //_logger.Info($"childNamespaceCard.ClassesList.Count = {childNamespaceCard.ClassesList.Count}");
                    //_logger.Info($"childNamespaceCard.InterfacesList.Count = {childNamespaceCard.InterfacesList.Count}");
                    //_logger.Info($"childNamespaceCard.EnumsList.Count = {childNamespaceCard.EnumsList.Count}");

                    RemoveItemsFromNamespaceForRepackingTypeCard(packageCard, childNamespaceCard, targetInitialTypeNames, repackingTypeCardOptions);

                    //_logger.Info($"childNamespaceCard.NamespacesList.Count (after) = {childNamespaceCard.NamespacesList.Count}");
                    //_logger.Info($"childNamespaceCard.ClassesList.Count (after) = {childNamespaceCard.ClassesList.Count}");
                    //_logger.Info($"childNamespaceCard.InterfacesList.Count (after) = {childNamespaceCard.InterfacesList.Count}");
                    //_logger.Info($"childNamespaceCard.EnumsList.Count (after) = {childNamespaceCard.EnumsList.Count}");

                    if (!childNamespaceCard.NamespacesList.Any() && !childNamespaceCard.ClassesList.Any() && !childNamespaceCard.InterfacesList.Any() && !childNamespaceCard.EnumsList.Any())
                    {
                        //_logger.Info($"childNamespaceCard.Name (for removing) = {childNamespaceCard.Name}");

                        namespaceCard.NamespacesList.Remove(childNamespaceCard);
                        packageCard.NamespacesList.Remove(childNamespaceCard);
                    }
                }
            }

            RemoveItemsFromParentElementCard(packageCard, namespaceCard, targetInitialTypeNames, repackingTypeCardOptions);
        }

        private static void RemoveItemsFromClassOrInterfaceForRepackingTypeCard(PackageCard packageCard, ClassCard classCard, List<string> targetInitialTypeNames, RepackingTypeCardOptions repackingTypeCardOptions)
        {
            //_logger.Info($"classCard = {classCard}");

            if (classCard.PropertiesList.Any())
            {
                foreach (var property in classCard.PropertiesList.ToList())
                {
                    if (repackingTypeCardOptions.PublicMembersOnly)
                    {
                        if (property.KindOfMemberAccess == KindOfMemberAccess.Public)
                        {
                            continue;
                        }

                        classCard.PropertiesList.Remove(property);
                    }
                }
            }

            if(classCard.ConstructorsList.Any())
            {
                foreach(var constructor in classCard.ConstructorsList.ToList())
                {
                    if (repackingTypeCardOptions.PublicMembersOnly)
                    {
                        if (constructor.KindOfMemberAccess == KindOfMemberAccess.Public)
                        {
                            continue;
                        }

                        classCard.ConstructorsList.Remove(constructor);
                    }
                }
            }

            if (classCard.MethodsList.Any())
            {
                foreach (var method in classCard.MethodsList.ToList())
                {
                    if (repackingTypeCardOptions.PublicMembersOnly)
                    {
                        if (method.KindOfMemberAccess == KindOfMemberAccess.Public)
                        {
                            continue;
                        }

                        classCard.MethodsList.Remove(method);
                    }
                }
            }

            RemoveItemsFromParentElementCard(packageCard, classCard, targetInitialTypeNames, repackingTypeCardOptions);
        }

        private static void RemoveItemsFromParentElementCard(PackageCard packageCard, ParentElementCard parentElementCard, List<string> targetInitialTypeNames, RepackingTypeCardOptions repackingTypeCardOptions)
        {
            //_logger.Info($"parentElementCard.ClassesList.Count = {parentElementCard.ClassesList.Count}");

            if (parentElementCard.ClassesList.Any())
            {
                foreach (var classCard in parentElementCard.ClassesList.ToList())
                {
                    if(targetInitialTypeNames.Any())
                    {
                        if (targetInitialTypeNames.Contains(classCard.Name.InitialName))
                        {
                            //_logger.Info($"classCard = {classCard}");

                            if (repackingTypeCardOptions.PublicMembersOnly)
                            {
                                RemoveItemsFromClassOrInterfaceForRepackingTypeCard(packageCard, classCard, targetInitialTypeNames, repackingTypeCardOptions);
                            }

                            continue;
                        }

                        parentElementCard.ClassesList.Remove(classCard);
                        packageCard.ClassesList.Remove(classCard);
                    }
                    else
                    {
                        if (repackingTypeCardOptions.PublicMembersOnly)
                        {
                            RemoveItemsFromClassOrInterfaceForRepackingTypeCard(packageCard, classCard, targetInitialTypeNames, repackingTypeCardOptions);
                        }
                    }
                }
            }

            //_logger.Info($"parentElementCard.InterfacesList.Count = {parentElementCard.InterfacesList.Count}");

            if (parentElementCard.InterfacesList.Any())
            {
                foreach (var interfaceCard in parentElementCard.InterfacesList.ToList())
                {
                    if(targetInitialTypeNames.Any())
                    {
                        if (targetInitialTypeNames.Contains(interfaceCard.Name.InitialName))
                        {
                            //_logger.Info($"interfaceCard = {interfaceCard}");

                            if (repackingTypeCardOptions.PublicMembersOnly)
                            {
                                RemoveItemsFromClassOrInterfaceForRepackingTypeCard(packageCard, interfaceCard, targetInitialTypeNames, repackingTypeCardOptions);
                            }

                            continue;
                        }

                        parentElementCard.InterfacesList.Remove(interfaceCard);
                        packageCard.InterfacesList.Remove(interfaceCard);
                    }
                    else
                    {
                        if (repackingTypeCardOptions.PublicMembersOnly)
                        {
                            RemoveItemsFromClassOrInterfaceForRepackingTypeCard(packageCard, interfaceCard, targetInitialTypeNames, repackingTypeCardOptions);
                        }
                    }
                }
            }

            //_logger.Info($"parentElementCard.EnumsList.Count = {parentElementCard.EnumsList.Count}");

            if (parentElementCard.EnumsList.Any())
            {
                if (targetInitialTypeNames.Any())
                {
                    foreach (var enumCard in parentElementCard.EnumsList.ToList())
                    {
                        if (targetInitialTypeNames.Contains(enumCard.Name.InitialName))
                        {
                            //_logger.Info($"enumCard = {enumCard}");
                            continue;
                        }

                        parentElementCard.EnumsList.Remove(enumCard);
                        packageCard.EnumsList.Remove(enumCard);
                    }
                }
            }
        }

        private static void SearchTargetNamesForRepackingTypeCard(string initialTypeName, RepackingTypeCardOptions repackingTypeCardOptions, List<string> result, Dictionary<string, ClassCard> classesInitialNamesDict, Dictionary<string, ClassCard> interfacesInitialNamesDict, Dictionary<string, EnumCard> enumsInitialNamesDict)
        {
            //_logger.Info($"initialTypeName = '{initialTypeName}'");

            if (result.Contains(initialTypeName))
            {
                return;
            }

            if (classesInitialNamesDict.ContainsKey(initialTypeName))
            {
                SearchTargetClassNameForRepackingTypeCard(initialTypeName, repackingTypeCardOptions, result, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
            }
            else
            {
                if (interfacesInitialNamesDict.ContainsKey(initialTypeName))
                {
                    SearchTargetInterfaceNameForRepackingTypeCard(initialTypeName, repackingTypeCardOptions, result, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
                }
                else
                {
                    if (enumsInitialNamesDict.ContainsKey(initialTypeName))
                    {
                        result.Add(initialTypeName);
                    }
                    else
                    {
                        throw new Exception($"'{initialTypeName}' must be documented.");
                    }
                }
            }
        }

        private static void SearchTargetClassNameForRepackingTypeCard(string initialTypeName, RepackingTypeCardOptions repackingTypeCardOptions, List<string> result, Dictionary<string, ClassCard> classesInitialNamesDict, Dictionary<string, ClassCard> interfacesInitialNamesDict, Dictionary<string, EnumCard> enumsInitialNamesDict)
        {
            //_logger.Info($"initialTypeName = '{initialTypeName}'");

            result.Add(initialTypeName);

            var targetCard = classesInitialNamesDict[initialTypeName];

            //_logger.Info($"targetCard = {targetCard}");

            SearchTargetClassOrInterfaceNameForRepackingTypeCard(targetCard, repackingTypeCardOptions, result, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
        }

        private static void SearchTargetInterfaceNameForRepackingTypeCard(string initialTypeName, RepackingTypeCardOptions repackingTypeCardOptions, List<string> result, Dictionary<string, ClassCard> classesInitialNamesDict, Dictionary<string, ClassCard> interfacesInitialNamesDict, Dictionary<string, EnumCard> enumsInitialNamesDict)
        {
            //_logger.Info($"initialTypeName = '{initialTypeName}'");

            result.Add(initialTypeName);

            var targetCard = interfacesInitialNamesDict[initialTypeName];

            //_logger.Info($"targetCard = {targetCard}");

            SearchTargetClassOrInterfaceNameForRepackingTypeCard(targetCard, repackingTypeCardOptions, result, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
        }

        private static void SearchTargetClassOrInterfaceNameForRepackingTypeCard(ClassCard targetCard, RepackingTypeCardOptions repackingTypeCardOptions, List<string> result, Dictionary<string, ClassCard> classesInitialNamesDict, Dictionary<string, ClassCard> interfacesInitialNamesDict, Dictionary<string, EnumCard> enumsInitialNamesDict)
        {
            var baseType = targetCard.BaseType;

            if (baseType != null)
            {
                throw new NotImplementedException();
            }

            var baseInterfacesList = targetCard.BaseInterfacesList;

            if (baseInterfacesList.Any())
            {
                foreach (var baseInterface in baseInterfacesList)
                {
                    var targetInitialName = baseInterface.Name.InitialName;

                    //_logger.Info($"targetInitialName = '{targetInitialName}'");

                    SearchTargetNamesForRepackingTypeCard(targetInitialName, repackingTypeCardOptions, result, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
                }
            }

            if (targetCard.PropertiesList.Any())
            {
                foreach (var property in targetCard.PropertiesList)
                {
                    //_logger.Info($"property = {property}");

                    if (property.KindOfMemberAccess != KindOfMemberAccess.Public && repackingTypeCardOptions.PublicMembersOnly)
                    {
                        continue;
                    }

                    if (property.UsedTypesList.Any())
                    {
                        foreach (var usedType in property.UsedTypesList)
                        {
                            var targetInitialName = usedType.Name.InitialName;

                            //_logger.Info($"targetInitialName = '{targetInitialName}'");

                            SearchTargetNamesForRepackingTypeCard(targetInitialName, repackingTypeCardOptions, result, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
                        }
                    }
                }
            }

            if(targetCard.ConstructorsList.Any())
            {
                foreach(var constructor in targetCard.ConstructorsList)
                {
                    //_logger.Info($"constructor = {constructor}");

                    if (constructor.KindOfMemberAccess != KindOfMemberAccess.Public && repackingTypeCardOptions.PublicMembersOnly)
                    {
                        continue;
                    }

                    if (constructor.UsedTypesList.Any())
                    {
                        foreach (var usedType in constructor.UsedTypesList)
                        {
                            var targetInitialName = usedType.Name.InitialName;

                            //_logger.Info($"targetInitialName = '{targetInitialName}'");

                            SearchTargetNamesForRepackingTypeCard(targetInitialName, repackingTypeCardOptions, result, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
                        }
                    }

                    if (constructor.ParamsList.Any())
                    {
                        foreach (var param in constructor.ParamsList)
                        {
                            //_logger.Info($"param = {param}");

                            if (param.UsedTypesList.Any())
                            {
                                foreach (var usedType in param.UsedTypesList)
                                {
                                    var targetInitialName = usedType.Name.InitialName;

                                    //_logger.Info($"targetInitialName = '{targetInitialName}'");

                                    SearchTargetNamesForRepackingTypeCard(targetInitialName, repackingTypeCardOptions, result, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
                                }
                            }
                        }
                    }
                }
            }

            if (targetCard.MethodsList.Any())
            {
                foreach (var method in targetCard.MethodsList)
                {
                    //_logger.Info($"method = {method}");

                    if (method.KindOfMemberAccess != KindOfMemberAccess.Public && repackingTypeCardOptions.PublicMembersOnly)
                    {
                        continue;
                    }

                    if (method.UsedTypesList.Any())
                    {
                        foreach (var usedType in method.UsedTypesList)
                        {
                            var targetInitialName = usedType.Name.InitialName;

                            //_logger.Info($"targetInitialName = '{targetInitialName}'");

                            SearchTargetNamesForRepackingTypeCard(targetInitialName, repackingTypeCardOptions, result, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
                        }
                    }

                    if (method.ParamsList.Any())
                    {
                        foreach (var param in method.ParamsList)
                        {
                            //_logger.Info($"param = {param}");

                            if (param.UsedTypesList.Any())
                            {
                                foreach (var usedType in param.UsedTypesList)
                                {
                                    var targetInitialName = usedType.Name.InitialName;

                                    //_logger.Info($"targetInitialName = '{targetInitialName}'");

                                    SearchTargetNamesForRepackingTypeCard(targetInitialName, repackingTypeCardOptions, result, classesInitialNamesDict, interfacesInitialNamesDict, enumsInitialNamesDict);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
