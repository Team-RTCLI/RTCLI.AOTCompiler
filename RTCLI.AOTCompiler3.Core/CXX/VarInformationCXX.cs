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
        public static string CXXVarDeclaration(this VariableDefinition Var)
        {
            var Type = Var.VariableType;
            return Type.IsGenericParameter ? $"RTCLI::TVar<{Type.CXXTypeName()}>" :
                Type.IsValueType ? Type.CXXTypeName() :
                $"RTCLI::TRef<{Type.CXXTypeName()}>";
        }
        
        public static string CXXVarInitVal(this VariableDefinition Var)
        {
            var Type = Var.VariableType;
            return Type.IsValueType ? $"{Var.CXXVarDeclaration()}()" : $"RTCLI::null";
        }
    }
}
