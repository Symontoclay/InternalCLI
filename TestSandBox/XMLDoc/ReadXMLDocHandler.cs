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

    public class Tyu
    {
        public string Id { get; set; }
        protected int Re { get; set; }
        private int D { get; set; }
    }

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

            var tmpType = typeof(Tyu);

            foreach (var prop in tmpType.GetProperties())
            {
                _logger.Info($"prop = {prop}");
                //_logger.Info($" = {}");
                //_logger.Info($" = {}");
                //_logger.Info($" = {}");
                //_logger.Info($" = {}");
                //_logger.Info($" = {}");
            }

            var idProp = tmpType.GetProperty("Id");

            _logger.Info($"idProp = {idProp}");

            var reProp = tmpType.GetProperty("Re", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            _logger.Info($"reProp = {reProp}");

            var dProp = tmpType.GetProperty("D", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            _logger.Info($"dProp = {dProp}");

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

            var classesOrInterfacesList = new List<ClassCard>();

            foreach(var typeCard in cardsOfTypesList)
            {
                _logger.Info($"typeCard = {typeCard}");

                typeCard.IsProcessed = true;

                var fullName = typeCard.Name.FullName;

                var type = typesDict[fullName];

                //_logger.Info($"type.IsPublic = {type.IsPublic}");
                //_logger.Info($"type.IsNotPublic = {type.IsNotPublic}");

                var kindOfType = GetKindOfType(type);

                _logger.Info($"kindOfType = {kindOfType}");

                //continue;

                List<XMLMemberCard> membersList = null;

                if (cardsOfMembersDict.ContainsKey(fullName))
                {
                    membersList = cardsOfMembersDict[fullName];
                }
                else
                {
                    membersList = new List<XMLMemberCard>();
                }

                switch(kindOfType)
                {
                    case KindOfType.Class:
                        classesOrInterfacesList.Add(ProcessClassOrInterface(typeCard, type, membersList, kindOfType));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kindOfType), kindOfType, null);
                }

                //_logger.Info($" = {}");
            }

            foreach(var memberCard in cardsOfMembersList)
            {
                _logger.Info($"memberCard = {memberCard}");
            }

            _logger.Info("End");
        }

        private ClassCard ProcessClassOrInterface(XMLMemberCard typeCard, Type type, List<XMLMemberCard> membersList, KindOfType kindOfType)
        {
            var result = new ClassCard();
            result.KindOfType = kindOfType;           
            result.XMLMemberCard = typeCard;
            result.Type = type;
            result.IsPublic = type.IsPublic;

            FillUpNamedElementCard(typeCard, result);

            _logger.Info($"membersList.Count = {membersList.Count}");

            foreach(var memberCard in membersList)
            {
                _logger.Info($"memberCard = {memberCard}");

                switch(memberCard.Name.Kind)
                {
                    case KindOfMember.Property:
                        {
                            var property = ProcessProperty(memberCard, type);
                            property.Parent = result;
                            result.PropertiesList.Add(property);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(memberCard.Name.Kind), memberCard.Name.Kind, null);
                }
            }

            _logger.Info($"result = {result}");

            throw new NotImplementedException();

            return result;
        }

        private PropertyCard ProcessProperty(XMLMemberCard memberCard, Type parentType)
        {
            var result = new PropertyCard();

            FillUpNamedElementCard(memberCard, result);

            var property = parentType.GetProperty(memberCard.Name.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            if (property == null)
            {
                property = parentType.GetProperty(memberCard.Name.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (property == null)
                {
                    throw new NotImplementedException();
                }

                result.KindOfMemberAccess = KindOfMemberAccess.Protected;
            }
            else
            {
                result.KindOfMemberAccess = KindOfMemberAccess.Public;
            }            

            result.PropertyInfo = property;

            _logger.Info($"result = {result}");

            throw new NotImplementedException();

            return result;
        }

        private void FillUpNamedElementCard(XMLMemberCard source, NamedElementCard dest)
        {
            dest.Name = source.Name;
            dest.Summary = source.Summary;
            dest.Remarks = source.Remarks;
            dest.ExamplesList = source.ExamplesList;
        }

        //private KindOfMemberAccess GetKindOfMemberAccess()
        //{

        //}

        private Type _delegateType = typeof(Delegate);

        private KindOfType GetKindOfType(Type type)
        {
            if(type.IsClass)
            {
                if(_delegateType.IsAssignableFrom(type))
                {
                    return KindOfType.Delegate;
                }
                return KindOfType.Class;
            }

            if(type.IsInterface)
            {
                return KindOfType.Interface;
            }

            if(type.IsEnum)
            {
                return KindOfType.Enum;
            }

            throw new NotImplementedException();
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
