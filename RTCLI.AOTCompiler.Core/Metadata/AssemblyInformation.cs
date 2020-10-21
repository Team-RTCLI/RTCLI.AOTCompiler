using System.Reflection;
using System.Collections.Generic;
using System.IO;

using Mono.Cecil;
using Newtonsoft.Json;
using System;

namespace RTCLI.AOTCompiler.Metadata
{
    public class AssemblyInformation : IMetadataInformation
    {
        public string IdentName => definition.Name.Name.Replace('.', '_') + ".v" + definition.Name.Version.ToString().Replace('.', '_');
        public string FullName => definition.FullName;
        public string Name => definition.Name.Name;
        
        public readonly Dictionary<ModuleDefinition, ModuleInformation> Modules = new Dictionary<ModuleDefinition, ModuleInformation>();
        public AssemblyInformation(AssemblyDefinition def, MetadataContext metadataContext)
        {
            this.definition = def;
            this.MetadataContext = metadataContext;

            foreach (var module in def.Modules)
            {
                if(module.HasTypes)
                    Modules.Add(module, new ModuleInformation(module, metadataContext));
            }
        }

        [JsonIgnore] private readonly AssemblyDefinition definition = null;

        IMetadataTokenProvider IMetadataInformation.Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}