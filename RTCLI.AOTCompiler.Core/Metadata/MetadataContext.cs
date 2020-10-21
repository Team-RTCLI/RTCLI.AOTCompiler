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
        public readonly Dictionary<string, AssemblyInformation> Assemblies = new Dictionary<string, AssemblyInformation>(); 
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
            var FocusedAssemblyLoaded = AssemblyDefinition.ReadAssembly(assemblyPath, parameter);
            FocusedAssembly = AssemblyDefinition.ReadAssembly(assemblyPath, parameter).FullName;
            foreach(var module in FocusedAssemblyLoaded.Modules)
            {
                var references = module.AssemblyReferences;
                foreach(var reference in references)
                {
                    if (reference.Name != "netstandard" && reference.Name != "mscorlib")
                    {
                        var dep = AssemblyDefinition.ReadAssembly(Path.Combine(AssemblyBase, reference.Name + ".dll"), parameter);
                        Assemblies.Add(dep.FullName, new AssemblyInformation(dep));
                    }
                }
            }

            Assemblies.Add(FocusedAssembly, new AssemblyInformation(FocusedAssemblyLoaded));
        }

        public TypeInformation GetTypeInformation(string inType)
        {
            foreach (var assembly in Assemblies.Values)
            {
                foreach (var module in assembly.Modules.Values)
                {
                    foreach (var type in module.Types.Keys)
                        if (type == inType)
                            return module.Types[type];
                }
            }
            return null;
        }

        public TypeInformation GetTypeInformation(TypeReference inType) 
            => GetTypeInformation(inType.FullName);

        [JsonIgnore] public string FocusedAssembly;
    }
}