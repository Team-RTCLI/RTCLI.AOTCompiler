using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using System.Linq;

namespace RTCLI.AOTCompiler3.Meta
{
    public static class ParamInformationCXX
    {
        public static string CXXParamDecorated(this ParameterDefinition pram)
        {
            if (pram.ParameterType.IsGenericParameter)
                return $"RTCLI::TVar<{pram.CXXTypeName()}>";
            if (!pram.ParameterType.IsValueType || (pram.IsOut || pram.IsIn))
                return pram.CXXTypeName() + "&";

            return pram.CXXTypeName();
        }
        public static string CXXTypeName(this ParameterDefinition pram)
        {
            return pram.ParameterType.CXXTypeName();
        }
    }
}
