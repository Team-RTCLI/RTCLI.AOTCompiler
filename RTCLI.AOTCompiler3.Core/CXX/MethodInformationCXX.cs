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

        [H2001()]
        public static string CXXMethodSignature(this MethodDefinition method, bool WithConstant)
        {
            return (method.IsStatic ? "static " : "") +
                method.CXXRetType() + " " +
                method.CXXShortMethodName() + method.CXXParamSequence(WithConstant);
            var Type = method.DeclaringType;

            string H2001 = (method.IsStatic ? "static " : "") +
                method.CXXRetType() + " " +
                method.CXXShortMethodName() + method.CXXParamSequence(WithConstant);
            string H2001_0 = $"{(method.IsNewSlot ? "virtual " : "")}{H2001 + (method.IsAbstract ? " = 0" : "")};";
            string H2001_1 = Type.IsValueType ? H2001_0.Replace("virtual ", "") : H2001_0; // [H2001-1] struct de-virtual
            string H2001_2 = (method.IsFinal && !Type.IsValueType) ? H2001_1.Replace(";", " final;") : H2001_1; // [H2001-2] final-specifier

            return H2001_2;
        }

        public static string CXXMethodDeclareName(this MethodDefinition Method)
        {
            var Type = Method.DeclaringType;
            return Type.CXXTypeName() + (Type.HasGenericParameters ? $"<{Type.CXXTemplateArg()}>" : "") 
                + "::" + Method.CXXShortMethodName();
        }
        public static string CXXMethodSignatureFull(this MethodDefinition method, bool WithConstant)
        {
            return (method.IsStatic ? "static " : "") +
                method.CXXRetType() + " " + method.DeclaringType.CXXTypeName().Replace("::", "_") + "_" +
                method.CXXShortMethodName() + method.CXXParamSequence(WithConstant);
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
            if (method.ReturnType.IsByReference)
                return type.CXXTypeName() + "&";
            if (method.ReturnType.IsGenericParameter)
                return $"RTCLI::TRet<{type.CXXTypeName()}>";
            if (type.IsValueType)
                return type.CXXTypeName();
            else
                return type.CXXTypeName() + "&";
        }

        public static string CXXMethodCallName(this MethodDefinition method, TypeReference type)
        {
            return $"{type.CXXTypeName()}::{method.CXXShortMethodName()}";
        }

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
            return method?.Name.Replace('<', '_').Replace('>', '_');
        }
    }
}
