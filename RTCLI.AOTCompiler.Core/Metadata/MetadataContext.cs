using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Mono.Cecil;
using Newtonsoft.Json;
using System.Diagnostics;

namespace RTCLI.AOTCompiler.Metadata
{
    public sealed class MetadataContext
    {
        public readonly Dictionary<string, AssemblyInformation> Assemblies = new Dictionary<string, AssemblyInformation>(); 
        public readonly Dictionary<string, TypeInformation> Types = new Dictionary<string, TypeInformation>();

        private List<string> assemblyFindDir = new List<string>();
        private AssemblyDefinition ReadAssemblyFromEnv(string assemblyName, ReaderParameters parameter)
        {
            AssemblyDefinition loaded = null;
            if (File.Exists(assemblyName))
            {
                loaded = AssemblyDefinition.ReadAssembly(assemblyName, parameter);
            }
            foreach (var dir in assemblyFindDir)
            {
                if (File.Exists(Path.Combine(dir, assemblyName)))
                {
                    loaded = AssemblyDefinition.ReadAssembly(Path.Combine(dir, assemblyName), parameter);
                }
            }

            if (Assemblies.ContainsKey(loaded.Name.Name))//Already Exists this Assembly
            {
                if (Assemblies[loaded.Name.Name].FullName != loaded.FullName)//Version Diff
                    Trace.Assert(false, "Assembly: " + loaded.Name.Name + " already exists!");
                return null;
            }
            else
                Assemblies.Add(loaded.Name.Name, new AssemblyInformation(loaded, this));

            return loaded;
        }

        private AssemblyDefinition ReadAssemblyRecursively(string assemblyName, ReaderParameters parameter)
        {
            AssemblyDefinition AssemblyLoadedRecursively = null;
            AssemblyLoadedRecursively = ReadAssemblyFromEnv(assemblyName, parameter);
            if(AssemblyLoadedRecursively != null)
            {
                var AssemblyLoadedRecursivelyName = AssemblyLoadedRecursively.Name.Name;
                ReaderParameters sys_parameter = new ReaderParameters
                {
                    AssemblyResolver = parameter.AssemblyResolver,
                    ReadSymbols = false
                };

                foreach (var module in AssemblyLoadedRecursively.Modules)
                {
                    var references = module.AssemblyReferences;
                    foreach (var reference in references)
                    {
                        if (reference.Name != "netstandard" && reference.Name != "mscorlib")
                        {
                            ReadAssemblyRecursively(reference.Name + ".dll", parameter);
                        }
                        else
                        {
                            ReadAssemblyRecursively(reference.Name + ".dll", sys_parameter);
                        }
                    }
                }
            }
            return AssemblyLoadedRecursively;
        }

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
            assemblyFindDir.Add(Path.GetDirectoryName(assemblyPath));
            assemblyFindDir.Add("");
            assemblyFindDir.Add(Directory.GetCurrentDirectory());

            var FocusedAssemblyLoaded = ReadAssemblyRecursively(assemblyPath, parameter);
            FocusedAssembly = FocusedAssemblyLoaded.Name.Name;
        }

        public TypeInformation GetTypeInformation(string inType)
        {
            // CTS
            if (Types.ContainsKey(inType))
                return Types[inType];
            else
            {
                foreach (var assembly in Assemblies.Values)
                    foreach (var module in assembly.Modules.Values)
                        foreach (var type in module.Types.Keys)
                            if (type == inType)
                            {
                                Types.Add(type, module.Types[type]);
                                return module.Types[type];
                            }
            }
            return null;
        }
        public TypeInformation GetTypeInformation(TypeReference inType)
        {
            if (inType.IsArray)
                return new TypeInformation(inType as ArrayType, this);
            return GetTypeInformation(inType.FullName);
        }

        [JsonIgnore] public string FocusedAssembly;
    }

    public static class MetaExtension
    {
        public static MethodInformation GetMetaInformation(
            this MethodReference methodReference, MetadataContext context)
        {
            var type = context.GetTypeInformation(methodReference.DeclaringType);
            foreach (var mtdInfo in type.Methods)
            {
                if (mtdInfo.Definition is MethodReference mtdRef)
                    if (mtdRef.FullName == methodReference.FullName)
                        return mtdInfo;
            }
            return null;
        }
    }
}