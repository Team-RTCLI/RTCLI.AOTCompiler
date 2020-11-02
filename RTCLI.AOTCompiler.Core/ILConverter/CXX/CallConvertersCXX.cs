using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;

namespace RTCLI.AOTCompiler.ILConverters
{
    public class MethodCallConvert
    {
        public static string GetMethodName(MethodReference mtd, MethodTranslateContext methodContext)
        {
            var type = methodContext.MetadataContext.GetTypeInformation(mtd.DeclaringType);
            return mtd.Name;
        }
        public static string GetMethodOwner(MethodReference mtd, MethodTranslateContext methodContext)
        {
            var type = methodContext.MetadataContext.GetTypeInformation(mtd.DeclaringType);
            return type.CXXTypeName;
        }
        public static string Convert(Instruction instruction, MethodTranslateContext methodContext, bool Virt)
        {
            var mtd = (instruction.Operand as MethodReference);
            string args = "";
            for (int i = 1; i <= mtd.Parameters.Count; i++)
            {
                args += $"{(methodContext as CXXMethodTranslateContext).CmptStackPopObject}"
                     + (i == mtd.Parameters.Count ? "" : ", ");
            }
            return $"(({GetMethodOwner(instruction.Operand as MethodReference, methodContext)}&)" // Caster: ((DeclaringType&)
                + $"{(methodContext as CXXMethodTranslateContext).CmptStackPopObject})" // Caller: caller)
                + $".{MethodCallConvert.GetMethodName(instruction.Operand as MethodReference, methodContext)}({args});"; // Method Call body.
        }
    }
    public class CallConverters : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => MethodCallConvert.Convert(instruction, methodContext, false);
        public OpCode TargetOpCode() => OpCodes.Call;
    }
    public class CallVirtConverters : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => MethodCallConvert.Convert(instruction, methodContext, true);
        public OpCode TargetOpCode() => OpCodes.Callvirt;
    }
}
