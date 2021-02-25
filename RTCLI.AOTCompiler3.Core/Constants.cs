using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using System.Reflection;
using RTCLI.AOTCompiler3.ILConverters;

namespace RTCLI.AOTCompiler3
{
    public static class Constants
    {
        public static string CopyRight => "// COPYRIGHT STRING";
        public static string CXXStaticCtorName => "StaticConstructor";
        public static string CXXCtorName => "Constructor";
        public static string CXXUberHeaderName => "_UberHeader_.h";

        static Constants()
        {
            InitAllCXXILConverters();
        }
        public static Dictionary<OpCode, ICXXILConverter> CXXILConverters = new Dictionary<OpCode, ICXXILConverter>();



        private static void InitAllCXXILConverters()
        {
            System.Console.WriteLine("Preparing ConvertersCXX...");
            Assembly assembly = Assembly.GetAssembly(typeof(ILConverters.ICXXILConverter));
            TypeFilter typeNameFilter = new TypeFilter(TypeNameFilter);
            int currentLineCursor = Console.CursorTop;
            int index = 0;
            foreach (var type in assembly.GetTypes())
            {
                Type[] typeInterfaces = type.FindInterfaces(typeNameFilter, typeof(ILConverters.ICXXILConverter));
                if (typeInterfaces.Length > 0)
                {
                    var newConv = System.Activator.CreateInstance(type) as ILConverters.ICXXILConverter;
                    CXXILConverters.Add(newConv.TargetOpCode(), newConv);

                    System.Console.SetCursorPosition(0, currentLineCursor);
                    Console.Write(new string(' ', Console.WindowWidth));
                    System.Console.SetCursorPosition(0, currentLineCursor);
                    System.Console.WriteLine($"Registered: {newConv.TargetOpCode()}, {++index} / {Enum.GetValues(typeof(Code)).Length}");
                }
            }
            System.Console.WriteLine($"Total Converters: {CXXILConverters.Count} / {Enum.GetValues(typeof(Code)).Length}");
        }
        private static bool TypeNameFilter(Type typeObj, Object criteriaObj)
        {
            if (typeObj.ToString() == criteriaObj.ToString())
                return true;
            else
                return false;
        }
    }
}
