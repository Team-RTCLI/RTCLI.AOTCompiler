using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

namespace RTCLI.AOTCompiler.Metadata
{
    public class TypeInformation : IMemberInformation
    {
        public readonly Dictionary<MethodDefinition, MethodInformation> Methods = new Dictionary<MethodDefinition, MethodInformation>();
        public TypeInformation(TypeDefinition def)
        {
            this.definition = def;
            
            foreach(var method in def.Methods)
            {
                Methods.Add(method, new MethodInformation(method));
            }
        }

        public readonly TypeDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
    }
}