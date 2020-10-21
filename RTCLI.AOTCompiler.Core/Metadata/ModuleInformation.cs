using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public class ModuleInformation : IMemberInformation
    {
        public readonly Dictionary<string, TypeInformation> Types = new Dictionary<string, TypeInformation>();
        public ModuleInformation(ModuleDefinition def)
        {
            this.definition = def;

            foreach(var type in definition.Types)
            {
                if(type.FullName != "<Module>")
                    Types.Add(type.FullName, new TypeInformation(type));
            }
        }

        [JsonIgnore] private readonly ModuleDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
    }
}