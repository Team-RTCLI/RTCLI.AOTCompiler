using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace RTCLI.AOTCompiler3.Meta
{
    public static class RTCLIAssembly
    {
        public static string RTCLIShortName(this AssemblyDefinition assembly)
        {
            return assembly.Name.Name.Replace('.', '_');
        }
        
        public static string RTCLIShortName(this AssemblyNameReference assembly)
        {
            return assembly.Name.Replace('.', '_');
        }

        public static string RTCLIFullName(this AssemblyDefinition assembly)
        {
            return assembly.FullName;
        }

    }

    public sealed class BasePathAssemblyResolver : IAssemblyResolver
    {
        private readonly DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();

        public BasePathAssemblyResolver(string basePath)
        {
            resolver.AddSearchDirectory(basePath);
        }

        public void Dispose()
        {
            resolver.Dispose();
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            return resolver.Resolve(name);
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            return resolver.Resolve(name, parameters);
        }
    }
}
