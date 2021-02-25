using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class CgtConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Cgt;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"RTCLI::i32 {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Cgt({op1}, {op2});";
        }
    }

    public class Cgt_UnConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"RTCLI::i32 {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Cgt_Un({op1}, {op2});";
        }

        public OpCode TargetOpCode() => OpCodes.Cgt_Un;
    }

    public class CeqConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ceq;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Ceq({op1}, {op2});";
        }
    }

    public class CltConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Clt;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Clt({op1}, {op2});";
        }
    }

    public class Clt_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Clt_Un;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            return $"auto {methodContext.CmptStackPushObject}" +
                $" = RTCLI::Clt_Un({op1}, {op2});";
        }
    }
}