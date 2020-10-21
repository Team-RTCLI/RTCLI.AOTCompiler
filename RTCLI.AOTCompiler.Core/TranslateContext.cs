using System.Reflection;
using System.Collections.Generic;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler
{
    public sealed class TranslateContext
    {
#region Constructors
        public TranslateContext(Assembly assembly, bool readSymbols)
            : this(assembly.Location, readSymbols)
        {
        }

        public TranslateContext(string assemblyPath, bool readSymbols)
        {
            var context = new MetadataContext(assemblyPath, readSymbols);
            this.FocusedAssembly = context.FocusedAssembly;
            this.MetadataContext = context;
        }
#endregion
        public MetadataContext MetadataContext { get; }

        public string FocusedAssembly;
        public AssemblyInformation FocusedAssemblyInformation => MetadataContext.Assemblies[FocusedAssembly];
    }
}

