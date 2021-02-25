using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public class MethodCallConvert
    {
        public static string GetMethodOwner(MethodReference mtd, MethodTranslateContextCXX methodContext)
        {
            return mtd.DeclaringType.CXXTypeName();
        }
        public static string Convert(Instruction instruction, MethodTranslateContextCXX methodContext, bool Virt)
        {
            var mtd = instruction.Operand as MethodReference;
            var mtdDef = mtd.Resolve();
            if(mtdDef is null) return "ERROR_METHOD_NAME";

            string args = "";
            List<string> argList = new List<string>();
            for (int i = 1; i <= mtd.Parameters.Count; i++)
                argList.Add(methodContext.CmptStackPopObject);
            argList.Reverse();
            args = string.Join(',', argList);
            if (mtd.FullName.StartsWith("!!0"))
            {
                var gargT = (mtd as GenericInstanceMethod).GenericArguments[0];
                
                return $"{gargT.CXXTypeName()}& {methodContext.CmptStackPushObject} = " +
                    $"\\n\t\tRTCLI::new_object<{gargT.CXXTypeName()}>({args});";
            }

            string genericArgs = "";
            if (mtd is GenericInstanceMethod gmtd)
                genericArgs = $"<{string.Join(',', gmtd.GenericArguments.Select(a => a.CXXTypeName()))}>";
            if (!mtdDef.IsStatic)
            {
                string caller = $"(({GetMethodOwner(mtd, methodContext)}&)" // Caster: ((DeclaringType&)
                    + $"{methodContext.CmptStackPopObject})."; // Caller: caller)
                string callBody =
                    $"{caller}" +
                    (Virt ? "" : $"{GetMethodOwner(mtd, methodContext)}::") +
                    $"{mtdDef.CXXShortMethodName() + genericArgs}({args});"; // Method Call body.
                return (mtd.ReturnType.FullName != "System.Void" ? $"auto {methodContext.CmptStackPushObject} = " : "")
                    + callBody;
            }
            if (mtdDef.IsStatic)
            {
                var type = mtd.DeclaringType.Resolve();
                string callBody = $"{mtdDef.CXXMethodCallName(type) + genericArgs}({args});"; // Method Call body.
                return type.CallStaticConstructor(methodContext) +
                (mtd.ReturnType.FullName != "System.Void" ? $"auto {methodContext.CmptStackPushObject} = " : "")
                    + callBody;
            }
            return "ERROR_METHOD_NAME";
        }
    }

    public class CallConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => MethodCallConvert.Convert(instruction, methodContext, false);
        public OpCode TargetOpCode() => OpCodes.Call;
    }
    public class CallVirtConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => MethodCallConvert.Convert(instruction, methodContext, true);
        public OpCode TargetOpCode() => OpCodes.Callvirt;
    }
    public class TailcallConverterCXX : ICXXILConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
            => "";
        public OpCode TargetOpCode() => OpCodes.Tail;
    }
}
