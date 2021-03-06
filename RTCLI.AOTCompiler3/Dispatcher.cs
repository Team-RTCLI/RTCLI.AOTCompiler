﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;
using RTCLI.AOTCompiler3.Translators;

namespace RTCLI.AOTCompiler3
{
    // ${OutputPath}/${Assembly}/[include/src]/${namespaces}.../${classname}.h/cpp
    public static class Dispatcher
    {
        public static void Translate(
            CodeTextStorage storage,
            DispatchArgs dispatchArgs,
            AssemblyDefinition assembly)
        {
            // ${OutputPath}/${Assembly}
            using (var _ = storage.EnterScope(assembly.RTCLIShortName()))
            {
                // ${OutputPath}/${Assembly}/include
                using (var _h = storage.EnterScope("include"))
                {
                    CXXHeaderTranslator translator = new CXXHeaderTranslator();
                    translator.Run(storage, assembly);
                }
            }
            using (var _ = storage.EnterScope(assembly.RTCLIShortName()))
            {
                // ${OutputPath}/${Assembly}/src
                using (var _cpp = storage.EnterScope("src"))
                {
                    CXXSourceTranslator translator = new CXXSourceTranslator();
                    translator.Run(storage, assembly);
                }
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

            if(dispatchArgs.recursivelyCompileAll)
            {
                Parallel.ForEach(AssemblyRepo.GlobalAssemblies, assembly =>
                {
                    var storage = new CodeTextStorage(
                            logw,
                            outputPath,
                            "    ");
                    {
                        Translate(
                            storage,
                            dispatchArgs,
                            assembly.Value);
                    }
                });;
            }
            else
            {
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
                        {
                            Translate(
                                storage,
                                dispatchArgs,
                                assembly);
                        }
                    }
                });
            }
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
