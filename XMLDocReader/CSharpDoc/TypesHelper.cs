using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLDocReader.CSharpDoc
{
    public static class TypesHelper
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static bool IsSystemOrThirdPartyType(string fullName)
        {
            if (fullName.StartsWith("System."))
            {
                return true;
            }

            return false;
        }

        private static List<Type> GetBaseTypesList(Type type, bool onlyNoSystemOrThirdPartyType)
        {
            var curentBaseType = type.BaseType;

            var typesList = new List<Type>();

            while (curentBaseType != null)
            {
                if (onlyNoSystemOrThirdPartyType && IsSystemOrThirdPartyType(curentBaseType.FullName))
                {
                    return typesList;
                }

                typesList.Add(curentBaseType);

                curentBaseType = curentBaseType.BaseType;
            }

            return typesList;
        }

        public static List<Type> GetBaseTypesAndInterfacesList(Type type, bool onlyNoSystemOrThirdPartyType)
        {
            var typesList = GetBaseTypesList(type, onlyNoSystemOrThirdPartyType);

            var interfacesList = type.GetInterfaces().Distinct().ToList();

            if (onlyNoSystemOrThirdPartyType)
            {
                interfacesList = interfacesList.Where(p => !IsSystemOrThirdPartyType(p.FullName)).ToList();
            }

            typesList.AddRange(interfacesList);

            return typesList;
        }

        public static List<Type> GetDirectlyImplementedInterfacesList(List<Type> interfacesList, bool onlyNoSystemOrThirdPartyType)
        {
            var childInterfacesList = new List<Type>();

            foreach (var tmpInterface in interfacesList)
            {
                //_logger.Info($"tmpInterface.FullName = {tmpInterface.FullName}");

                if (onlyNoSystemOrThirdPartyType)
                {
                    childInterfacesList.AddRange(tmpInterface.GetInterfaces().Where(p => !IsSystemOrThirdPartyType(p.FullName)));
                }
                else
                {
                    childInterfacesList.AddRange(tmpInterface.GetInterfaces());
                }
            }

            //_logger.Info($"childInterfacesList.Count = {childInterfacesList.Count}");

            //_logger.Info($"childInterfacesList.Select(p => p.FullName) = {JsonConvert.SerializeObject(childInterfacesList.Select(p => p.FullName).ToList(), Formatting.Indented)}");

            var result = interfacesList.Except(childInterfacesList).ToList();

            if (onlyNoSystemOrThirdPartyType)
            {
                result = result.Where(p => !IsSystemOrThirdPartyType(p.FullName)).ToList();
            }

            return result;
        }
    }
}
