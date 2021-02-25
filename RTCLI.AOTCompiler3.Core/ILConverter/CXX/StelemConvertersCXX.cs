using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class StelemConvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContextCXX methodContext, string type)
        {
            var op3 = methodContext.CmptStackPopObject;
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"RTCLI::ArraySet<{type}>({op1}, {op2}, {op3});";
        }
    }

    public class StelemConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_Any;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            return StelemConvert.Convert(instruction, methodContext, typeReference.CXXTypeName());
        }
    }
    public class Stelem_RefConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_Ref;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StelemConvert.Convert(instruction, methodContext, "");
    }

    public class Stelem_IConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_I;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StelemConvert.Convert(instruction, methodContext, "int");
    }
    public class Stelem_I1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_I1;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::System::Int8");
    }
    public class Stelem_I2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_I2;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::System::Int16");
    }
    public class Stelem_I4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_I4;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::System::Int32");
    }
    public class Stelem_I8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_I8;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::System::Int64");
    }

    public class Stelem_R4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_R4;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::System::Single");
    }
    public class Stelem_R8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Stelem_R8;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => StelemConvert.Convert(instruction, methodContext, "RTCLI::System::Double");
    }
}
