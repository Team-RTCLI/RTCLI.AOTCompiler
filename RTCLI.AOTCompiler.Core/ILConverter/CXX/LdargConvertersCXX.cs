using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class LdargConvert
    {
        public static string Convert(MethodTranslateContext methodContext, int index)
        {
            if (!methodContext.MethodInfo.IsStatic)
                index -= 1;
            if(index < 0)
                return $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = *this;";
            else
                return $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = {methodContext.MethodInfo.Parameters[index].Name};";
        }
    }

    public class LdnullConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldnull;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI::null;";
    }
    public class Ldarg_0ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_0;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdargConvert.Convert(methodContext, 0);
    }
    public class Ldarg_1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_1;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdargConvert.Convert(methodContext, 1);
    }
    public class Ldarg_2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_2;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdargConvert.Convert(methodContext, 2);
    }
    public class Ldarg_3ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_3;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => LdargConvert.Convert(methodContext, 3);
    }
    public class LdargConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = {(instruction.Operand as ParameterDefinition).Name};";
    }
    public class Ldarg_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_S;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = {(instruction.Operand as ParameterDefinition).Name};";
    }
    public class LdargaConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarga;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI_ADDRESSOF({(instruction.Operand as ParameterDefinition).Name});";
    }
    public class Ldarga_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarga_S;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = RTCLI_ADDRESSOF({(instruction.Operand as ParameterDefinition).Name});";
    }
}
