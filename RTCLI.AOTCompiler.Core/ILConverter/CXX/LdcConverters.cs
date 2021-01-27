using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class Ldc_Convert
    {
        public static string I4Convert(Instruction instruction, MethodTranslateContext methodContext, Int32 val)
        {
            CXXMethodTranslateContext ctx = methodContext as CXXMethodTranslateContext;
            return $"{ctx.CmptStackPushObject} = RTCLI::StaticCast<{/*ctx.MetadataContext.UInt32Type.CXXTypeName*/"RTCLI::System::Int32"}>({val});";
        }
        public static string I8Convert(Instruction instruction, MethodTranslateContext methodContext, Int64 val)
        {
            CXXMethodTranslateContext ctx = methodContext as CXXMethodTranslateContext;
            return $"{ctx.CmptStackPushObject} = RTCLI::StaticCast<{/*ctx.MetadataContext.UInt32Type.CXXTypeName*/"RTCLI::System::Int64"}>({val});";
        }
        public static string R4Convert(Instruction instruction, MethodTranslateContext methodContext, Single val)
        {
            CXXMethodTranslateContext ctx = methodContext as CXXMethodTranslateContext;
            return $"{ctx.CmptStackPushObject} = RTCLI::StaticCast<{/*ctx.MetadataContext.UInt32Type.CXXTypeName*/"RTCLI::System::Single"}>({val});";
        }
        public static string R8Convert(Instruction instruction, MethodTranslateContext methodContext, Double val)
        {
            CXXMethodTranslateContext ctx = methodContext as CXXMethodTranslateContext;
            return $"{ctx.CmptStackPushObject} = RTCLI::StaticCast<{/*ctx.MetadataContext.UInt32Type.CXXTypeName*/"RTCLI::System::Double"}>({val});";
        }
    }

    public class Ldc_I4ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
                    => Ldc_Convert.I4Convert(instruction, methodContext, Int32.Parse(instruction.Operand.ToString()));

        public OpCode TargetOpCode() => OpCodes.Ldc_I4;
    }
    public class Ldc_I4_0ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, 0);

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_0;
    }
    public class Ldc_I4_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, Int32.Parse(instruction.Operand.ToString()));

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_S;
    }
    public class Ldc_I4_1ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, 1);

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_1;
    }
    public class Ldc_I4_2ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, 2);

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_2;
    }
    public class Ldc_I4_3ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, 3);

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_3;
    }
    public class Ldc_I4_4ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, 4);

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_4;
    }
    public class Ldc_I4_5ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, 5);

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_5;
    }
    public class Ldc_I4_6ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, 6);

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_6;
    }
    public class Ldc_I4_7ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, 7);

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_7;
    }
    public class Ldc_I4_8ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, 8);

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_8;
    }
    public class Ldc_I4_M1ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I4Convert(instruction, methodContext, -1);

        public OpCode TargetOpCode() => OpCodes.Ldc_I4_M1;
    }

    public class Ldc_I8ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.I8Convert(instruction, methodContext, Int64.Parse(instruction.Operand.ToString()));

        public OpCode TargetOpCode() => OpCodes.Ldc_I8;
    }
    public class Ldc_R4ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.R4Convert(instruction, methodContext, Single.Parse(instruction.Operand.ToString()));

        public OpCode TargetOpCode() => OpCodes.Ldc_R4;
    }
    public class Ldc_R8ConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => Ldc_Convert.R8Convert(instruction, methodContext, Double.Parse(instruction.Operand.ToString()));

        public OpCode TargetOpCode() => OpCodes.Ldc_R8;
    }
}
