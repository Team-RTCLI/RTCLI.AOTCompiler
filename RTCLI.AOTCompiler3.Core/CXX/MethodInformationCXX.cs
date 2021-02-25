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

        public static string CXXMethodSignature(this MethodDefinition method, bool WithConstant)
        {
            return (method.IsStatic ? "static " : "") +
                method.CXXRetType() + " " +
                method.CXXShortMethodName() + method.CXXParamSequence(WithConstant);
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
            return sequence;
        }
        
        public static string CXXRetType(this MethodDefinition method)
        {
            var type = method.ReturnType;
            //if (type == null)
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
