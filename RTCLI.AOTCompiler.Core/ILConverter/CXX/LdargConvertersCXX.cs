using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class Ldarg_0ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_0;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = *this;";
    }
    public class Ldarg_1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_1;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = {methodContext.MethodInfo.Parameters[0].Name};";
    }
    public class Ldarg_2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_2;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = {methodContext.MethodInfo.Parameters[1].Name};";
    }
    public class Ldarg_3ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_3;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = {methodContext.MethodInfo.Parameters[2].Name};";
    }
    public class Ldarg_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_S;
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => $"auto& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = {(instruction.Operand as ParameterDefinition).Name};";
    }
}
