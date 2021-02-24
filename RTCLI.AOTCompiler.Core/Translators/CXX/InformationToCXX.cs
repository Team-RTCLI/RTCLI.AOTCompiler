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
                    return $"{genericElementType.CXXTypeName}<{string.Join(',', genericArgumentTypes.Select(a => a.CXXTypeName))}>";
                if (IsGenericParameter)
                    return definitionGP.FullName;
                else return "RTCLI::" + string.Join("::", FullName.Split('.', '/')).Replace("<>", "__").Replace('`', '_').Replace("<", "_").Replace(">", "_");
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
                    return $"{genericElementType.CXXTypeName}<{string.Join(',', genericArgumentTypes.Select(a => a.CXXTypeName))}>";
                if (IsGenericParameter)
                    return definitionGP.FullName;
                if (IsPointer)
                    return definitionPointer.FullName;
                else return definition.Name.Replace("<>", "__").Replace('`', '_').Replace("<", "_").Replace(">", "_");
            }
        }



        public string CallStaticConstructor(Translators.MethodTranslateContext methodContext)
        {
            if (methodContext.MethodInfo == StaticConstructor)
                return "";
            if (!methodContext.StaticReference.Contains(this))
            {
                methodContext.StaticReference.Add(this);
                if (StaticConstructor != null)
                    return $"{StaticConstructor.CXXMethodCallName(this)}();";
            }
            return "";
        }
    }

    public partial class MethodInformation : IMemberInformation
    {
        private string CXXParamsSequence(bool WithConstant)
        {
            string sequence = "";
            //Since LdArg.0 -> this, start argument index from 1
            uint i = 1;
            foreach (var param in Parameters)
            {
                sequence += param.CXXParamDecorated + " " + param.Name;
                if(param.Definition.HasConstant && WithConstant)
                {
                    if (param.Definition.Constant != null)
                        sequence += " = " + param.Definition.Constant;
                    else
                        sequence += " = " + (param.IsValueType ? $"{param.CXXTypeName}()" : $"RTCLI::null");
                }
                if (i++ != Parameters.Count)
                    sequence = sequence + ", " + ((i % 3 == 1) ? "\n\t" : "");
            }
            return sequence;
        }

        public string CXXMethodDeclareName
        {
            get
            {
                var type = MetadataContext.GetTypeInformation(definition.DeclaringType);
                return type.CXXTypeName + (type.HasGenericParameters ? $"<{type.CXXTemplateArg}>" : "") + "::" + CXXMethodNameShort;
            }
        }
        public string CXXMethodCallName(TypeInformation type)
        {
            return $"{type.CXXTypeName}::{CXXMethodNameShort}";
        }
        public string CXXMethodNameShort
            => (definition.IsConstructor ? (definition.IsStatic ? "StaticConstructor" : "Constructor") : definition?.Name.Replace('<', '_').Replace('>', '_'));

        public string CXXMethodSignature(bool WithConstant) => (IsStatic ? "static " : "") + CXXRetType + " " + CXXMethodNameShort + CXXParamSequence(WithConstant);
        public string CXXParamSequence(bool WithConstant) => $"({CXXParamsSequence(WithConstant)})"; //Param Sequence
        public string CXXArgSequence => $"({string.Join(',', Parameters.Select(a => a.Name))})";

        public string CXXTemplateParam =>
            genericParameterTypes != null ? string.Join(',', genericParameterTypes.Select(a => $"class {a.CXXTypeName}")) : "";
        public string CXXTemplateArg =>
            genericParameterTypes != null ? string.Join(',', genericParameterTypes.Select(a => a.CXXTypeName)) : "";
        public string CXXRetType
        {
            get
            {
                var type = MetadataContext.GetTypeInformation(definition.ReturnType);
                //if (type == null)
                //    return "RTCLI::System::Void";
                
                if (type.IsValueType && !definition.ReturnType.IsByReference)
                    return type.CXXTypeName;
                else
                    return type.CXXTypeName + "&";
            }
        } 
        public string CXXStackName => $"{string.Join("_", CXXMethodDeclareName.Split(new string[] { "::", "<", ">" }, StringSplitOptions.None))}__Stack";
    }

    public partial class FieldInformation : IMemberInformation
    {
        public string CXXTypeName => MetadataContext.GetTypeInformation(definition.FieldType).CXXTypeName;
        public string CXXTypeNameShort => MetadataContext.GetTypeInformation(definition.FieldType).CXXTypeNameShort;
        public string CXXFieldDeclaration => (IsStatic ? "static " : "") +
            (this.definition.FieldType.IsValueType ? $"{CXXTypeName} " : $"RTCLI::Managed<{CXXTypeName}> ") +
            $"{Utilities.GetCXXValidTokenString(Name)};";
    }

    public partial class VariableInformation
    {
        public string CXXTypeName => MetadataContext.GetTypeInformation(Definition.VariableType).CXXTypeName;

        public string CXXVarDeclaration => Type.IsValueType ? CXXTypeName : $"RTCLI::TRef<{CXXTypeName}>";
        public string CXXVarInitVal => this.Definition.VariableType.IsValueType ? $"{CXXVarDeclaration}()" : $"RTCLI::null";
    }
}