using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using System.Linq;

namespace RTCLI.AOTCompiler3.Meta
{ 
    public static class FieldInformationCXX
    {
        public static string CXXTypeName(this FieldDefinition field)
        {
            return field.FieldType.CXXTypeName();
        }
        public static string CXXFieldDeclaration(this FieldDefinition field)
        {
            string res =
                (field.IsStatic ? "static " : "") +
                (field.FieldType.IsGenericParameter ? $"RTCLI::TField<{field.CXXTypeName()}> " :
                field.FieldType.IsValueType ? $"{field.CXXTypeName()} " : $"RTCLI::Managed<{field.CXXTypeName()}> ") +
            $"{Utilities.GetCXXValidTokenString(field.Name)};";
            return res;
        }
    }
}
