using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class StindConvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContext methodContext, string type)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"RTCLI::Deref<{type}>({op0}) = {(methodContext as CXXMethodTranslateContext).CmptStackPushObject};";
        }
    }

    public class Stind_RefConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_Ref;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StindConvert.Convert(instruction, methodContext, "");
    }

    public class Stind_IConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_I;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StindConvert.Convert(instruction, methodContext, "int");
    }
    public class Stind_I1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_I1;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::i8");
    }
    public class Stind_I2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_I2;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::i16");
    }
    public class Stind_I4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_I4;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::i32");
    }
    public class Stind_I8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_I8;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::i64");
    }

    public class Stind_R4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_R4;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::f32");
    }
    public class Stind_R8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stind_R8;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StindConvert.Convert(instruction, methodContext, "RTCLI::f64");
    }
}
