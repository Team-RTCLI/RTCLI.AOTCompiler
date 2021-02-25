using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class LdargConvert
    {
        public static string Convert(MethodTranslateContextCXX methodContext, int index)
        {
            if (!methodContext.Method.IsStatic)
                index -= 1;
            if(index < 0)
                return $"auto& {methodContext.CmptStackPushObject} = *this;";
            else
                return $"auto& {methodContext.CmptStackPushObject} = {methodContext.Method.Parameters[index].Name};";
        }
    }

    public class LdnullConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldnull;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"auto& {methodContext.CmptStackPushObject} = RTCLI::null;";
    }
    public class Ldarg_0ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_0;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdargConvert.Convert(methodContext, 0);
    }
    public class Ldarg_1ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_1;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdargConvert.Convert(methodContext, 1);
    }
    public class Ldarg_2ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_2;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdargConvert.Convert(methodContext, 2);
    }
    public class Ldarg_3ConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_3;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => LdargConvert.Convert(methodContext, 3);
    }
    public class LdargConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"auto& {methodContext.CmptStackPushObject} = {(instruction.Operand as ParameterDefinition).Name};";
    }
    public class Ldarg_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarg_S;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"auto& {methodContext.CmptStackPushObject} = {(instruction.Operand as ParameterDefinition).Name};";
    }
    public class LdargaConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarga;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"auto& {methodContext.CmptStackPushObject} = RTCLI_ADDRESSOF({(instruction.Operand as ParameterDefinition).Name});";
    }
    public class Ldarga_SConverterCXX : ICXXILConverter
    {
        public OpCode TargetOpCode() => OpCodes.Ldarga_S;
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => $"auto& {methodContext.CmptStackPushObject} = RTCLI_ADDRESSOF({(instruction.Operand as ParameterDefinition).Name});";
    }
}
