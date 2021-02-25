using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class StlocConvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContextCXX methodContext, int index)
        {
            return $"v{index} = {methodContext.CmptStackPopObject};";
        }
    }
    public class Stloc_0ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stloc_0;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StlocConvert.Convert(instruction, methodContext, 0);
    }
    public class Stloc_1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stloc_1;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StlocConvert.Convert(instruction, methodContext, 1);
    }
    public class Stloc_2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stloc_2;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StlocConvert.Convert(instruction, methodContext, 2);
    }
    public class Stloc_3ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stloc_3;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StlocConvert.Convert(instruction, methodContext, 3);
    }
    public class Stloc_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stloc_S;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StlocConvert.Convert(instruction, methodContext, (instruction.Operand as VariableDefinition).Index);
    }

}