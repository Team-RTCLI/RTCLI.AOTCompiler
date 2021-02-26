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
            return pram.ParameterType.CXXVarDeclaration();
        }
        public static string CXXTypeName(this ParameterDefinition pram)
        {
            return pram.ParameterType.CXXTypeName();
        }
    }
}
