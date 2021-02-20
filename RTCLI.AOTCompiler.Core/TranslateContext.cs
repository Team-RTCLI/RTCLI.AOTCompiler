using System.Reflection;
using System.Collections.Generic;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler
{
    public sealed class TranslateContext
    {
#region Constructors
        public TranslateContext(Assembly assembly, bool readSymbols, MetadataContext metadataContext)
            : this(assembly.Location, readSymbols, metadataContext)
        {
        }

        public TranslateContext(string assemblyPath, bool readSymbols, MetadataContext metadataContext)
        {
            this.FocusedAssembly = metadataContext.FocusedAssembly;
            this.MetadataContext = metadataContext;
        }
#endregion
        public MetadataContext MetadataContext { get; }

        public string FocusedAssembly;
        public AssemblyInformation FocusedAssemblyInformation => MetadataContext.Assemblies[FocusedAssembly];
    }
}

