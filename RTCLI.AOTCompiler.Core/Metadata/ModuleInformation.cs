using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

namespace RTCLI.AOTCompiler.Metadata
{
    public class ModuleInformation : IMemberInformation
    {
        public readonly Dictionary<TypeDefinition, TypeInformation> Types = new Dictionary<TypeDefinition, TypeInformation>();
        public ModuleInformation(ModuleDefinition def)
        {
            this.definition = def;

            foreach(var type in definition.Types)
            {
                Types.Add(type, new TypeInformation(type));
            }
        }

        public readonly ModuleDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
    }
}