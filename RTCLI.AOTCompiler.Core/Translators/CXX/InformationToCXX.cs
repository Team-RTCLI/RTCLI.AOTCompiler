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
                    return $"{genericDeclaringType.CXXTypeName}<{string.Join(',', genericArgumentTypes.Select(a => a.CXXTypeName))}>";
                if (IsGenericParameter)
                    return definitionGP.FullName;
                else return "RTCLI::" + string.Join("::", FullName.Split('.', '/')).Replace("<>", "__").Replace('`', '_'); ;
            }
        }
        public string CXXTemplateParam =>
            genericParameterTypes != null ? string.Join(',', genericParameterTypes.Select(a => $"class {a.CXXTypeName}")) : "";
        public string CXXTemplateArg =>
            genericParameterTypes != null ? string.Join(',', genericParameterTypes.Select(a => a.CXXTypeName)) : "";
        public string CXXTypeNameShort
        {
            get
            {
                if (IsArray)
                    return $"RTCLI::System::ElementArray<{elementType.CXXTypeName}>";
                if (IsGenericInstance)
                    return $"{genericDeclaringType.CXXTypeName}<{string.Join(',', genericArgumentTypes.Select(a => a.CXXTypeName))}>";
                if (IsGenericParameter)
                    return definitionGP.FullName;
                else return definition.Name.Replace("<>", "__").Replace('`', '_');
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
        {
            get
            {
                var type = MetadataContext.GetTypeInformation(definition.DeclaringType);
                return type.CXXTypeName + (type.HasGenericParameters ? $"<{type.CXXTemplateArg}>" : "") + "::" + CXXMethodNameShort;
            }
        }
        public string CXXMethodNameShort
            => (definition.IsConstructor ? (definition.IsStatic ? "StaticConstructor" : "Constructor") : definition?.Name.Replace('<', '_').Replace('>', '_'));

        public string CXXMethodSignature => (IsStatic ? "static " : "")+  CXXRetType + " " + CXXMethodNameShort + CXXParamSequence;
        public string CXXParamSequence => "(" + CXXParamsSequence() + ")"; //Param Sequence

        public string CXXTemplateParam =>
            genericParameterTypes != null ? string.Join(',', genericParameterTypes.Select(a => $"class {a.CXXTypeName}")) : "";
        public string CXXTemplateArg =>
            genericParameterTypes != null ? string.Join(',', genericParameterTypes.Select(a => a.CXXTypeName)) : "";
        public string CXXRetType => MetadataContext.GetTypeInformation(definition.ReturnType)?.CXXTypeName;
        public string CXXStackName => $"{string.Join("_", CXXMethodName.Split(new string[] { "::", "<", ">" }, StringSplitOptions.None))}__Stack";
    }

    public partial class FieldInformation : IMemberInformation
    {
        public string CXXTypeName => MetadataContext.GetTypeInformation(definition.FieldType).CXXTypeName;
        public string CXXTypeNameShort => MetadataContext.GetTypeInformation(definition.FieldType).CXXTypeNameShort;
        public string CXXFieldDeclaration => (IsStatic ? "static " : "") +
            (this.definition.FieldType.IsValueType ? $"{CXXTypeName} " : $"RTCLI::ref<{CXXTypeName}> ") +
            $"{Utilities.GetCXXValidTokenString(Name)};";
    }

    public partial class VariableInformation
    {
        public string CXXTypeName => MetadataContext.GetTypeInformation(Definition.VariableType).CXXTypeName;

        public string CXXVarDeclaration => this.Definition.VariableType.IsValueType ? $"{CXXTypeName}" : $"{CXXTypeName}&";
        public string CXXVarInitVal => this.Definition.VariableType.IsValueType ? $"{CXXVarDeclaration}()" : $"({CXXVarDeclaration})RTCLI::null";
    }
}