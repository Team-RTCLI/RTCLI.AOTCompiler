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
        public static MetadataContext NetStandardMetadataContext = null;
        public Dictionary<string, AssemblyInformation> Assemblies = new Dictionary<string, AssemblyInformation>();
        public Dictionary<string, TypeInformation> Types = new Dictionary<string, TypeInformation>();
        private List<string> assemblyFindDir = new List<string>();

        static MetadataContext()
        {
            // Initialize NetStandard.
            NetStandardMetadataContext = new MetadataContext("netstandard.dll", false);
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

            foreach (var assembly in Assemblies.Values)
                foreach (var module in assembly.Modules.Values)
                    foreach (var type in module.Types.Values)
                        BuildTypeMapRecursively(type);
            FocusedAssembly = FocusedAssemblyLoaded.Name.Name;
        }

        private AssemblyDefinition ReadAssemblyFromEnv(string assemblyName, ReaderParameters parameter)
        {
            AssemblyDefinition loaded = null;
            if (File.Exists(assemblyName))
            {
                System.Console.WriteLine($"Reading {assemblyName}...");
                loaded = AssemblyDefinition.ReadAssembly(assemblyName, parameter);
            }
            else
            {
                foreach (var dir in assemblyFindDir)
                {
                    if (File.Exists(Path.Combine(dir, assemblyName)))
                    {
                        System.Console.WriteLine($"Reading {assemblyName}...");
                        loaded = AssemblyDefinition.ReadAssembly(Path.Combine(dir, assemblyName), parameter);
                    }
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
                        if(NetStandardMetadataContext is null)
                        {
                            ReadAssemblyRecursively(reference.Name + ".dll", sys_parameter);
                        }
                        else if (reference.Name != "netstandard" && reference.Name != "mscorlib")
                        {
                            ReadAssemblyRecursively(reference.Name + ".dll", parameter);
                        }
                        else
                        {
                            Assemblies = Assemblies.Concat(NetStandardMetadataContext.Assemblies).ToDictionary(k => k.Key, v => v.Value);
                        }
                    }
                }
            }
            return AssemblyLoadedRecursively;
        }

        public void BuildTypeMapRecursively(TypeInformation type)
        {
            Types.Add(type.FullName, type);
            foreach (var nested in type.Nested)
                BuildTypeMapRecursively(nested);
        }

        public TypeInformation GetTypeInformation(string inType)
        {
            // CTS
            if (Types.ContainsKey(inType))
                return Types[inType];
            return null;
        }
        public TypeInformation GetTypeInformation(TypeReference inType)
        {
            if (inType.IsArray)
                return new TypeInformation(inType as ArrayType, this);
            if(inType.IsGenericInstance)
                return new TypeInformation(inType as GenericInstanceType, this);
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