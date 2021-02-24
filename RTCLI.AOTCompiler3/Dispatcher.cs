using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;

namespace RTCLI.AOTCompiler3
{
    public static class Dispatcher
    {
        public static void Translate(
            CodeTextStorage storage,
            DispatchArgs dispatchArgs,
            string assemblyPath,
            AssemblyDefinition assembly)
        {
            //System.Console.WriteLine("AOTCompiler: Preparing assembly: \"{0}\" ...", Path.GetFullPath(assemblyPath));
            using (var _ = storage.EnterScope("include"))
            {
                
            }
            using (var _ = storage.EnterScope("src"))
            {
                
            }

            System.Console.WriteLine($"Processing {assembly.RTCLIShortName()} done.");
        }

        public static void TranslateAll(
            TextWriter logw,
            string outputPath,
            DispatchArgs dispatchArgs,
            IEnumerable<string> assemblyPaths)
        {
            Dictionary<string, AssemblyDefinition> assemblyPathMapping 
                = new Dictionary<string, AssemblyDefinition>();
            // Collect all assemblies
            foreach (var path in assemblyPaths)
            {
                var resolver = new BasePathAssemblyResolver(Path.GetDirectoryName(path));
                var parameter = new ReaderParameters
                {
                    AssemblyResolver = resolver,
                    ReadSymbols = dispatchArgs.readSymbols
                };
                if(!assemblyPathMapping.ContainsKey(path))
                {
                    assemblyPathMapping[path] = AssemblyRepo.Store(path, parameter);
                }
            }

            // Translate
            Parallel.ForEach(assemblyPaths, aseemblyPath => {
                if (!assemblyPathMapping.ContainsKey(aseemblyPath))
                {
                    
                }
                else
                {
                    var assembly = assemblyPathMapping[aseemblyPath];
                    var storage = new CodeTextStorage(
                        logw,
                        outputPath,
                        "    ");
                    Translate(
                        storage,
                        dispatchArgs,
                        aseemblyPath,
                        assembly);
                }
            });
        }

        public static void TranslateAll(
            TextWriter logw,
            string outputPath,
            DispatchArgs dispatchArgs,
            params string[] assemblyPaths)
        {
            TranslateAll(
                logw,
                outputPath,
                dispatchArgs,
                (IEnumerable<string>)assemblyPaths
            );
        }
    }
}
