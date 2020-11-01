using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;
using System.Reflection;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class StlocConvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContext methodContext, int index)
        {
            return $"stack.Store<RTCLI::decay_t<decltype({(methodContext as CXXMethodTranslateContext).CmptStackObjectName})>, {index}>({(methodContext as CXXMethodTranslateContext).CmptStackPopObject});";
        }
    }
    public class Stloc_0ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stloc_0;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StlocConvert.Convert(instruction, methodContext, 0);
    }
    public class Stloc_1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stloc_1;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StlocConvert.Convert(instruction, methodContext, 1);
    }
    public class Stloc_2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stloc_2;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StlocConvert.Convert(instruction, methodContext, 2);
    }
    public class Stloc_3ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stloc_3;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StlocConvert.Convert(instruction, methodContext, 3);
    }
    public class Stloc_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stloc_S;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StlocConvert.Convert(instruction, methodContext, (instruction.Operand as Mono.Cecil.Cil.VariableDefinition).Index);
    }

}