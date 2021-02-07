using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class LdelemConvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContext methodContext, string type)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::ArrayGet<{type}>({op1}, {op0});";
        }
    }

    public class LdelemConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_Any;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var typeReference = instruction.Operand as TypeReference;
            TypeInformation typeInformation = methodContext.TranslateContext.MetadataContext.GetTypeInformation(typeReference);
            return LdelemConvert.Convert(instruction, methodContext, typeInformation.CXXTypeName);
        }
    }
    public class Ldelem_RefConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_Ref;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "");
    }

    public class Ldelem_IConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_I;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext) 
            => LdelemConvert.Convert(instruction, methodContext, "int");
    }
    public class Ldelem_I1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_I1;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext) 
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::i8");
    }
    public class Ldelem_I2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_I2;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::i16");
    }
    public class Ldelem_I4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_I4;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::i32");
    }
    public class Ldelem_I8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_I8;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::i64");
    }

    public class Ldelem_R4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_R4;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::f32");
    }
    public class Ldelem_R8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_R8;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::f64");
    }

    public class Ldelem_U1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_U1;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::u8");
    }
    public class Ldelem_U2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_U2;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::u16");
    }
    public class Ldelem_U4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelem_U4;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdelemConvert.Convert(instruction, methodContext, "RTCLI::u32");
    }


    public class LdelemaConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldelema;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op0 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::ArrayGet_Addr({op0}, {op1});";
        }
    }
}
