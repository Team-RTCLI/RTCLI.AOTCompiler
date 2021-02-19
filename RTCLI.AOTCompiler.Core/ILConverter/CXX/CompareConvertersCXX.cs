using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class CgtConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Cgt;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"RTCLI::i32 {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Cgt({op1}, {op2});";
        }
    }

    public class Cgt_UnConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"RTCLI::i32 {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Cgt_Un({op1}, {op2});";
        }

        public OpCode TargetOpCode() => OpCodes.Cgt_Un;
    }

    public class CeqConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ceq;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Ceq({op1}, {op2});";
        }
    }

    public class CltConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Clt;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Clt({op1}, {op2});";
        }
    }

    public class Clt_UnConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Clt_Un;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op2 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            var op1 = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject}" +
                $" = RTCLI::Clt_Un({op1}, {op2});";
        }
    }
}