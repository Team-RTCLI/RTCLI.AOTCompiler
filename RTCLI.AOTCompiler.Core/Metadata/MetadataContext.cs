using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Mono.Cecil;

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
            FocusedAssembly = AssemblyDefinition.ReadAssembly(assemblyPath, parameter);

            Assemblies.Add(FocusedAssembly, new AssemblyInformation(FocusedAssembly));
        }
        public AssemblyDefinition FocusedAssembly;
    }
}