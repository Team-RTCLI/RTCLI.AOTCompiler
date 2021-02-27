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
        public static string CXXConstant(this FieldDefinition field)
        {
            if (field.Constant != null)
                return field.Constant.ToString();
            else
                return (field.FieldType.IsValueType ? $"{field.FieldType.CXXTypeName()}()" : $"RTCLI::null");
        }
        public static string CXXFieldDeclaration(this FieldDefinition field)
        {
            if (field.HasConstant && !field.IsStatic)
            {
            }
            string res =
                (field.IsStatic ? "static " : "") +
                (field.FieldType.IsGenericParameter ? $"RTCLI::TField<{field.CXXTypeName()}> " :
                field.FieldType.IsValueType ? $"{field.CXXTypeName()} " : $"RTCLI::TRef<{field.CXXTypeName()}> ") +
            $"{Utilities.GetCXXValidTokenString(field.Name)};";
            return res;
        }
    }
}
