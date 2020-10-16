using System.Reflection;
using System.Collections.Generic;
using System.IO;

using Mono.Cecil;

namespace RTCLI.AOTCompiler.Metadata
{
    public class AssemblyInformation : IMetadataInformation
    {
        public readonly Dictionary<ModuleDefinition, ModuleInformation> Modules = new Dictionary<ModuleDefinition, ModuleInformation>();
        public AssemblyInformation(AssemblyDefinition def)
        {
            this.definition = def;

            foreach(var module in def.Modules)
            {
                Modules.Add(module, new ModuleInformation(module));
            }
        }

        public readonly AssemblyDefinition definition = null;

        IMetadataTokenProvider IMetadataInformation.Definition => definition;
    }
}