using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;

namespace RTCLI.AOTCompiler3
{
    public static class AssemblyRepo
    {
        public static AssemblyDefinition Store(string assemblyName, ReaderParameters parameter)
        {
            return ReadAssemblyRecursively(assemblyName, parameter);
        }

        static AssemblyRepo()
        {
            assemblyFindDir.Add("");
            assemblyFindDir.Add(Directory.GetCurrentDirectory());
        }

        private static AssemblyDefinition ReadAssemblyRecursively(
            string assemblyName, ReaderParameters parameter)
        {
            var pth = Path.GetDirectoryName(assemblyName);
            if(Directory.Exists(pth))
            {
                System.Console.WriteLine("Add " + pth + " to Assembly Find Dir.");
                assemblyFindDir.Add(pth);
            }
            AssemblyDefinition AssemblyLoadedRecursively = null;
            AssemblyLoadedRecursively = ReadAssemblyFromDisk(assemblyName, parameter);
            if (AssemblyLoadedRecursively != null)
            {
                var AssemblyLoadedRecursivelyName = AssemblyLoadedRecursively.RTCLIShortName();

                if (GlobalAssemblies.ContainsKey(AssemblyLoadedRecursivelyName))
                {
                    // Skip...
                }
                else
                {
                    System.Console.WriteLine(AssemblyLoadedRecursivelyName + " Loaded!");
                    GlobalAssemblies.Add(AssemblyLoadedRecursivelyName, AssemblyLoadedRecursively);

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
                            ReadAssemblyRecursively(reference.Name + ".dll", parameter);
                        }
                    }
                }
            }
            return AssemblyLoadedRecursively;
        }

        private static AssemblyDefinition ReadAssemblyFromDisk(string assemblyName, ReaderParameters parameter)
        {
            AssemblyDefinition loaded = null;
            foreach (var dir in assemblyFindDir)
            {
                if (File.Exists(Path.Combine(dir, assemblyName)))
                {
                    loaded = AssemblyDefinition.ReadAssembly(Path.Combine(dir, assemblyName), parameter);
                    if (loaded != null)
                        return loaded;
                }
            }
            return loaded;
        }

        private static List<string> assemblyFindDir = new List<string>();
        public static readonly Dictionary<string, AssemblyDefinition> GlobalAssemblies 
            = new Dictionary<string, AssemblyDefinition>(); 
    }
}
