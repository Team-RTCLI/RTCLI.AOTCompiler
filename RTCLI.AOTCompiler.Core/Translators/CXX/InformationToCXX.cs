using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Mono.Cecil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public partial class TypeInformation : IMemberInformation
    {
        public string CXXNamespace => "RTCLI::" + string.Join("::", Namespace.Split('.'));
        public string CXXTypeName
        {
            get
            {
                if (IsArray)
                    return $"RTCLI::System::ElementArray<{elementType.CXXTypeName}>";
                if (IsGenericInstance)
                    return $"{genericDeclaringType.CXXTypeName}<{string.Join(',', genericArgumentTypes.Select(a=>a.CXXTypeName))}>";
                else return "RTCLI::" + string.Join("::", FullName.Split('.', '/')).Replace("<>", "__").Replace('`', '_'); ;
            }
        }
        public string CXXTypeNameShort
        {
            get
            {
                if (IsArray)
                    return $"RTCLI::System::ElementArray<{elementType.CXXTypeName}>";
                if (IsGenericInstance)
                    return $"{genericDeclaringType.CXXTypeName}<{string.Join(',', genericArgumentTypes.Select(a => a.CXXTypeName))}>";
                else return definition.Name.Replace("<>", "__"); ;
            }
        }
    }

    public partial class MethodInformation : IMemberInformation
    {
        private string CXXParamsSequence()
        {
            string sequence = "";
            //Since LdArg.0 -> this, start argument index from 1
            uint i = 1;
            foreach (var param in Parameters)
            {
                sequence += param.CXXParamDecorated + " " + param.Name;
                if (i++ != Parameters.Count)
                    sequence = sequence + ", " + ((i % 3 == 1) ? "\n\t" : "");
            }
            return sequence;
        }

        public string CXXMethodName
            =>  // Return Type
             MetadataContext.GetTypeInformation(definition.DeclaringType)?.CXXTypeName //Type Name
             + "::" + CXXMethodNameShort;//MethodName
        public string CXXMethodNameShort
            => (definition.IsConstructor ? "Constructor" : definition?.Name.Replace('<', '_').Replace('>', '_'));

        public string CXXMethodSignature => CXXRetType + " " + CXXMethodNameShort + CXXParamSequence;
        public string CXXParamSequence => "(" + CXXParamsSequence() + ")"; //Param Sequence

        public string CXXRetType => MetadataContext.GetTypeInformation(definition.ReturnType)?.CXXTypeName;
        public string CXXStackName => $"{string.Join("_", CXXMethodName.Split("::"))}__Stack";
    }

    public partial class FieldInformation : IMemberInformation
    {
        public string CXXTypeName => MetadataContext.GetTypeInformation(definition.FieldType).CXXTypeName;
        public string CXXTypeNameShort => MetadataContext.GetTypeInformation(definition.FieldType).CXXTypeNameShort;
        public string CXXFieldDeclaration => $"{CXXTypeName} {Utilities.GetCXXValidTokenString(Name)};";
    }


}