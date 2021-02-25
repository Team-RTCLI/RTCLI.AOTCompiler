﻿using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class LdindConvert
    {
        public static string Convert(Instruction instruction, MethodTranslateContextCXX methodContext, string type)
        {
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject} = RTCLI::Deref<{type}>({op1});";
        }
    }

    public class Ldind_RefConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_Ref;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "");
    }

    public class Ldind_IConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_I;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "int");
    }
    public class Ldind_I1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_I1;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "RTCLI::i8");
    }
    public class Ldind_I2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_I2;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "RTCLI::i16");
    }
    public class Ldind_I4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_I4;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "RTCLI::i32");
    }
    public class Ldind_I8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_I8;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "RTCLI::i64");
    }

    public class Ldind_R4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_R4;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "RTCLI::f32");
    }
    public class Ldind_R8ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_R8;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "RTCLI::f64");
    }

    public class Ldind_U1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_U1;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "RTCLI::u8");
    }
    public class Ldind_U2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_U2;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "RTCLI::u16");
    }
    public class Ldind_U4ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldind_U4;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdindConvert.Convert(instruction, methodContext, "RTCLI::u32");
    }

}
