using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class BrConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"goto {instruction.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Br;
    }
    public class Br_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"goto {instruction.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Br_S;
    }

    public class BrtrueConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"if (RTCLI::Cond({op})) goto {instruction.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brtrue;
    }
    public class Brtrue_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"if (RTCLI::Cond({op})) goto {instruction.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brtrue_S;
    }

    public class BrfalseConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"if (!RTCLI::Cond({op})) goto {instruction.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brfalse;
    }
    public class Brfalse_SConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
        {
            var op = (methodContext as CXXMethodTranslateContext).CmptStackPopObject;
            return $"if (!RTCLI::Cond({op})) goto {instruction.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brfalse_S;
    }
}