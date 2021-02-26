using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

namespace RTCLI.AOTCompiler3.Meta
{
    public static class VarInformationCXX
    {
        public static string CXXVarDeclaration(this TypeReference type)
        {
            if (type.IsVoid())
                return "RTCLI::System::Void";
            bool IsByRef = type.IsByReference;
            if (IsByRef)
            {
                if (type.IsGenericParameter)
                    return $"RTCLI::TLocal<{type.CXXTypeName()}&>";
                else if (type.GetElementType().IsValueType)
                    return $"RTCLI::TRef<{type.CXXTypeName()}>";
                else
                    return $"RTCLI::TRef<RTCLI::TRef<{type.CXXTypeName()}>>";
            }
            else
            {
                if (type.IsGenericParameter)
                    return $"RTCLI::TLocal<{type.CXXTypeName()}>";
                else if (type.IsValueType)
                    return type.CXXTypeName();
                else
                    return $"RTCLI::TRef<{type.CXXTypeName()}>";
            }
        }

        public static string CXXVarDeclaration(this VariableDefinition Var)
        {
            return Var.VariableType.CXXVarDeclaration();
        }
        
        public static string CXXVarInitVal(this VariableDefinition Var)
        {
            var Type = Var.VariableType;
            return Type.IsValueType ? $"{Var.CXXVarDeclaration()}()" : $"RTCLI::null";
        }
    }
}
