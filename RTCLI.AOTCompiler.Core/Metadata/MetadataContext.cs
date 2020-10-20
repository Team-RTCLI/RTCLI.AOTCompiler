using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Mono.Cecil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public sealed class MetadataContext
    {
        public readonly Dictionary<AssemblyDefinition, AssemblyInformation> Assemblies = new Dictionary<AssemblyDefinition, AssemblyInformation>(); 
        public MetadataContext(string assemblyPath, bool readSymbols)
        {
            // Initialize Assembly Resolver.
            var resolver = new BasePathAssemblyResolver(Path.GetDirectoryName(assemblyPath));
            var parameter = new ReaderParameters
            {
                AssemblyResolver = resolver,
                ReadSymbols = readSymbols
            };
            // Read Assembly
            string AssemblyBase = Path.GetDirectoryName(assemblyPath);
            FocusedAssembly = AssemblyDefinition.ReadAssembly(assemblyPath, parameter);
            foreach(var module in FocusedAssembly.Modules)
            {
                var references = module.AssemblyReferences;
                foreach(var reference in references)
                {
                    if (reference.Name != "netstandard" && reference.Name != "mscorlib")
                    {
                        var dep = AssemblyDefinition.ReadAssembly(Path.Combine(AssemblyBase, reference.Name + ".dll"), parameter);
                        Assemblies.Add(dep, new AssemblyInformation(dep));
                    }
                }
            }

            Assemblies.Add(FocusedAssembly, new AssemblyInformation(FocusedAssembly));
        }

        public TypeInformation GetTypeInformation(TypeReference inType)
        {
            foreach(var assembly in Assemblies.Values)
            {
                foreach(var module in assembly.Modules.Values)
                {
                    foreach (var type in module.Types.Keys)
                        if (type.FullName == inType.FullName)
                            return module.Types[type];
                }
            }
            return null;
        }
        [JsonIgnore] public AssemblyDefinition FocusedAssembly;
    }
}