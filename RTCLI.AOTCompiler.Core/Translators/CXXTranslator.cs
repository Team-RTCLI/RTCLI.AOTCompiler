using RTCLI.AOTCompiler;
using RTCLI.AOTCompiler.ILConverters;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using Mono.Cecil.Cil;

namespace RTCLI.AOTCompiler.Translators
{
    public struct CXXTranslateOptions
    {
        DebugInformationOptions debugInformationOption;
    }

    public class CXXTranslator
    {
        private static bool TypeNameFilter(Type typeObj, Object criteriaObj)
        {
            if (typeObj.ToString() == criteriaObj.ToString())
                return true;
            else
                return false;
        }

        public CXXTranslator(CXXTranslateOptions options)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ILConverters.IILConverter));
            TypeFilter typeNameFilter = new TypeFilter(TypeNameFilter);
            foreach (var type in assembly.GetTypes())
            {
                Type[] typeInterfaces = type.FindInterfaces(typeNameFilter, typeof(ILConverters.ICXXILConverter));
                if (typeInterfaces.Length > 0)
                {
                    var newConv = System.Activator.CreateInstance(type) as ILConverters.ICXXILConverter;
                    ConvertersCXX.Add(newConv.TargetOpCode(), newConv);
                }
            }
        }

        public string TranslateILInstruction(Instruction inst)
        {
            if (ConvertersCXX.TryGetValue(inst.OpCode, out ICXXILConverter targetConverter))
                return targetConverter.Convert(inst);
            return $"//{inst.ToString()} Has No Converter Implementation!\n\nstatic_assert(0, \"{inst.ToString()} Has No Converter Implementation!\");\n";
        }

        private readonly Dictionary<OpCode, ICXXILConverter> ConvertersCXX = new Dictionary<OpCode, ICXXILConverter>(); 
    }
}