using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class LdelemConvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContextCXX methodContext, string type)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::ArrayGet<{type}>({op1}, {op2});";
        }
    }

    public class LdelemConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_Any;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            return LdelemConvert.Convert(instruction, methodContext, typeReference.CXXTypeName());
        }
    }
    public class Ldelem_RefConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_Ref;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "");
    }

    public class Ldelem_IConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_I;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext) 
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::NativeInt");
    }
    public class Ldelem_I1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_I1;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext) 
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::System::Int8");
    }
    public class Ldelem_I2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_I2;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::System::Int16");
    }
    public class Ldelem_I4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_I4;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::System::Int32");
    }
    public class Ldelem_I8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_I8;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::System::Int64");
    }

    public class Ldelem_R4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_R4;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::System::Single");
    }
    public class Ldelem_R8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_R8;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::System::Double");
    }

    public class Ldelem_U1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_U1;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::System::UInt8");
    }
    public class Ldelem_U2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_U2;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::System::UInt16");
    }
    public class Ldelem_U4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_U4;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::System::UInt32");
    }


    public class LdelemaConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelema;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::ArrayGet_Addr({op1}, {op2});";
        }
    }
}
 