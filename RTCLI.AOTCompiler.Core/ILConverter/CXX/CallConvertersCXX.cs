﻿using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using RTCLI.AOTCompiler.Translators;
using Mono.Cecil;
using RTCLI.AOTCompiler.Metadata;
using System.Linq;

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
            List<string> argList = new List<string>();
            for (int i = 1; i <= mtd.Parameters.Count; i++)
                argList.Add((methodContext as CXXMethodTranslateContext).CmptStackPopObject);
            argList.Reverse();
            args = string.Join(',', argList);
            if(mtd.FullName.StartsWith("!!0"))
            {
                var garg = (mtd as GenericInstanceMethod).GenericArguments[0];
                var type = methodContext.MetadataContext.GetTypeInformation(garg);
                return $"{type.CXXTypeName}& {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = \n\t\tRTCLI::new_object<{type.CXXTypeName}>({args});";
            }
                
            var methodInformation = mtd.GetMetaInformation(methodContext.MetadataContext);
            string genericArgs = "";
            if (mtd is GenericInstanceMethod gmtd)
                genericArgs = $"<{string.Join(',', gmtd.GenericArguments.Select(a => methodContext.MetadataContext.GetTypeInformation(a).CXXTypeName))}>";
            if (!methodInformation.IsStatic)
            {
                string caller = $"(({GetMethodOwner(mtd, methodContext)}&)" // Caster: ((DeclaringType&)
                    + $"{(methodContext as CXXMethodTranslateContext).CmptStackPopObject})."; // Caller: caller)
                string callBody =
                    $"{caller}" +
                    (Virt ? "" : $"{GetMethodOwner(mtd, methodContext)}::") +
                    $"{methodInformation.CXXMethodNameShort + genericArgs}({args});"; // Method Call body.
                return (mtd.ReturnType.FullName != "System.Void" ? $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = " : "")
                    + callBody;
            }
            if(methodInformation.IsStatic)
            {
                var type = methodContext.MetadataContext.GetTypeInformation(mtd.DeclaringType);
                string callBody = $"{methodInformation.CXXMethodCallName(type) + genericArgs}({args});"; // Method Call body.
                return type.CallStaticConstructor(methodContext) +
                (mtd.ReturnType.FullName != "System.Void" ? $"auto {(methodContext as CXXMethodTranslateContext).CmptStackPushObject} = " : "")
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
    public class TailcallConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContext methodContext)
            => "";
        public OpCode TargetOpCode() => OpCodes.Tail;
    }
}
