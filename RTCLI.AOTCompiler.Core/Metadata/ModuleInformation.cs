using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public class ModuleInformation : IMemberInformation
    {
        public readonly Dictionary<string, TypeInformation> Types = new Dictionary<string, TypeInformation>();
        public ModuleInformation(ModuleDefinition def, MetadataContext metadataContext)
        {
            this.definition = def;
            this.MetadataContext = metadataContext;

            foreach(var type in definition.Types)
            {
                if(type.FullName != "<Module>")
                {
                    if(Types.ContainsKey(type.FullName))
                    {
                        Console.WriteLine(type.Module.Name
                            + "!="
                            + (Types[type.FullName].Definition as TypeDefinition).Module.Name
                        );
                    }
                    else
                        Types.Add(type.FullName, new TypeInformation(type, metadataContext));
                }
            }
        }

        [JsonIgnore] private readonly ModuleDefinition definition = null;
        public IMetadataTokenProvider Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}