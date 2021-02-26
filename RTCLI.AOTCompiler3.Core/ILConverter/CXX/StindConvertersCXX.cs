using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class StindConvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContextCXX methodContext, string type)
        {
            var op1 = methodContext.CmptStackPopObject;
            var op2 = methodContext.CmptStackPopObject;
            return $"RTCLI::Deref<{type}>({op2}) = {op1};";
        }
    }

    public class Stind_RefConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_Ref;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StindConvert.Convert(instruction, methodContext, "");
    }

    public class Stind_IConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_I;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StindConvert.Convert(instruction, methodContext, "int");
    }
    public class Stind_I1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_I1;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::i8");
    }
    public class Stind_I2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_I2;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::i16");
    }
    public class Stind_I4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_I4;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::i32");
    }
    public class Stind_I8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_I8;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::i64");
    }

    public class Stind_R4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_R4;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::f32");
    }
    public class Stind_R8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_R8;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::f64");
    }
}
