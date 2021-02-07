using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class StelemConvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContext methodContext, string type)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto " +
                $" = RTCLI::ArraySet<{type}>({op2}, {op1}, {op0});";
        }
    }

    public class StelemConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_Any;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
            return StelemConvert.Convert(instruction, methodContext, typeInformation.CXXTypeName);
        }
    }
    public class Stelem_RefConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_Ref;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StelemConvert.Convert(instruction, methodContext, "");
    }

    public class Stelem_IConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_I;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StelemConvert.Convert(instruction, methodContext, "int");
    }
    public class Stelem_I1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_I1;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::i8");
    }
    public class Stelem_I2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_I2;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::i16");
    }
    public class Stelem_I4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_I4;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::i32");
    }
    public class Stelem_I8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_I8;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::i64");
    }

    public class Stelem_R4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_R4;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::f32");
    }
    public class Stelem_R8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_R8;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::f64");
    }
}
