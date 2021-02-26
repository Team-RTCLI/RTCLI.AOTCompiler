using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using System.Linq;

namespace RTCLI.AOTCompiler3.Meta
{
    public static class MethodInformationCXX
    {
        public static string CXXTemplateParam(this MethodDefinition method)
        {
            var gTs = method.GenericParameters;
            return gTs != null ? string.Join(',', gTs.Select(a => $"class {a.CXXTypeName()}")) : "";
        }

        public static string CondStr(bool Cond, string Str)
        {
            return Cond ? Str : "";
        }
        [H2001()]
        public static string CXXMethodSignature(this MethodDefinition method)
        {
            return (method.IsStatic ? "static " : "") +
                method.CXXRetType() + " " +
                method.CXXShortMethodName() + method.CXXParamSequence(true);
        }

        [H2001()]
        public static string CXXMethodImplSignature(this MethodDefinition method, bool ValueType, TypeDefinition Type = null)
        {
            return method.CXXRetType() + " " +
                method.CXXMethodDeclareName(ValueType, Type) + method.CXXParamSequence(false);
        }

        public static string CXXMethodDeclarePrefix(this TypeDefinition Type, bool ValueType)
        {
            return $"{Type.CXXTypeName()}{CondStr(!ValueType && Type.IsValueType, "_V")}{CondStr(Type.HasGenericParameters, $"<{Type.CXXTemplateArg()}>")}";
        }

        public static string CXXMethodDeclareName(this MethodDefinition Method, bool ValueType, TypeDefinition Type = null)
        {
            if(Type is null)
                Type = Method.DeclaringType;
            return $"{Type.CXXMethodDeclarePrefix(ValueType)}::{Method.CXXShortMethodName()}";
        }

        public static string CXXArgSequence(this MethodDefinition method)
        {
            return $"({string.Join(',', method.Parameters.Select(a => a.Name))})";
        }
        
        public static string CXXParamSequence(this MethodDefinition method, bool WithConstant)
        {
            string sequence = "";
            //Since LdArg.0 -> this, start argument index from 1
            uint i = 1;
            foreach (var param in method.Parameters)
            {
                sequence += param.CXXParamDecorated() + " " + param.Name;
                if (param.HasConstant && WithConstant)
                {
                    if (param.Constant != null)
                        sequence += " = " + param.Constant;
                    else
                        sequence += " = " +
                            (param.ParameterType.IsValueType ? 
                            $"{param.ParameterType.CXXTypeName()}()" :
                            $"RTCLI::null");
                }
                if (i++ != method.Parameters.Count)
                    sequence = sequence + ", " + ((i % 3 == 1) ? "\n\t" : "");
            }
            return $"({sequence})";
        }
        
        public static string CXXRetType(this MethodDefinition method)
        {
            var type = method.ReturnType;
            //if (type.DeclaringType == null)
            //    return "RTCLI::System::Void";

            bool IsByRef = type.IsByReference;
            if (IsByRef)
            {
                if (type.IsGenericParameter)
                    return $"RTCLI::TRet<{type.CXXTypeName()}&>";
                else if (type.GetElementType().IsValueType)
                    return type.CXXTypeName() + "&";
                else
                    return $"RTCLI::TRef<{type.CXXTypeName()}>&";
            }
            else
            {
                if (type.IsGenericParameter)
                    return $"RTCLI::TRet<{type.CXXTypeName()}>";
                else if (type.IsValueType)
                    return type.CXXTypeName();
                else
                    return $"{type.CXXTypeName()}&";
            }
        }

        public static string CXXMethodCallName(this MethodDefinition method, TypeReference type)
        {
            return $"{type.CXXTypeName()}::{method.CXXShortMethodName()}";
        }
        public static string CXXRowName(this MethodDefinition method)
            => method?.Name.Replace('<', '_').Replace('>', '_');
        public static string CXXShortMethodName(this MethodDefinition method)
        {
            if(method.IsConstructor)
            {
                if(method.IsStatic)
                {
                    return Constants.CXXStaticCtorName;
                }
                return Constants.CXXCtorName;
            }
#if ENABLE_EXPLICT_OVERRIDE
            if(method.IsVirtual)
                return method?.DeclaringType.CXXTypeName().Replace("::", "_") + "_" + method?.Name.Replace('<', '_').Replace('>', '_'); ;
#endif
            return method.CXXRowName();
        }
    }
}
