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
            var methodInformation = mtd.GetMetaInformation(methodContext.MetadataContext);
            if (!methodInformation.IsStatic)
            {
                string caller = $"(({GetMethodOwner(mtd, methodContext)}&)" // Caster: ((DeclaringType&)
                    + $"{(methodContext as CXXMethodTranslateContext).CmptStackPopObject})."; // Caller: caller)
                string callBody =
                    $"{caller}" +
                    (Virt ? "" : $"{GetMethodOwner(mtd, methodContext)}::") +
                    $"{mtd.GetMetaInformation(methodContext.MetadataContext).CXXMethodNameShort}({args});"; // Method Call body.
                return (mtd.ReturnType.FullName != "System.Void" ? $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = " : "")
                    + callBody;
            }
            if(methodInformation.IsStatic)
            {
                string callBody = $"{mtd.GetMetaInformation(methodContext.MetadataContext).CXXMethodName}({args});"; // Method Call body.
                return (mtd.ReturnType.FullName != "System.Void" ? $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = " : "")
                    + callBody;
            }
            return "ERROR_METHOD_NAME";
        }
    }

    public class CallConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => MethodCallConvert.Convert(instruction, methodContext, false);
        public OpCode TargetOpCode() => OpCodes.Call;
    }
    public class CallVirtConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => MethodCallConvert.Convert(instruction, methodContext, true);
        public OpCode TargetOpCode() => OpCodes.Callvirt;
    }
}
