using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Linq;
using RTCLI.AOTCompiler3.Meta;
using System.Collections.Generic;

namespace RTCLI.AOTCompiler3.ILConverters
{
    public interface IControlFlowConverter : ICXXILConverter
    {
        void ICXXILConverter.Visit(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            methodContext.LableReference.Add(to.GetLabel());
        }
    }

    public class BrConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            return $"goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Br;
    }
    public class Br_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            return $"goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Br_S;
    }

    public class BrtrueConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            var op = methodContext.CmptStackPopObject;
            return $"if(RTCLI::Cond({op}))" +
                $" goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brtrue;
    }
    public class Brtrue_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            var op = methodContext.CmptStackPopObject;
            return $"if(RTCLI::Cond({op})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brtrue_S;
    }

    public class BrfalseConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            var op = methodContext.CmptStackPopObject;
            return $"if(!RTCLI::Cond({op})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brfalse;
    }
    public class Brfalse_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            var op = methodContext.CmptStackPopObject;
            return $"if(!RTCLI::Cond({op})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Brfalse_S;
    }


    public class LeaveConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            return $"RTCLI_LEAVE {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Leave;
    }
    public class Leave_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            Instruction to = instruction.Operand as Instruction;
            return $"RTCLI_LEAVE {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Leave_S;
    }

    public class BeqConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Ceq({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Beq;
    }
    public class Beq_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Ceq({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Beq_S;
    }

    public class BgeConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Clt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bge;
    }
    public class Bge_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Clt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bge_S;
    }
    public class Bge_UnConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Clt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bge_Un;
    }
    public class Bge_Un_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Clt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bge_Un_S;
    }

    public class BgtConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Cgt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bgt;
    }
    public class Bgt_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Cgt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bgt_S;
    }
    public class Bgt_UnConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Cgt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bgt_Un;
    }
    public class Bgt_Un_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Cgt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bgt_Un_S;
    }

    public class BleConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Cgt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Ble;
    }
    public class Ble_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Cgt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Ble_S;
    }
    public class Ble_UnConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Cgt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Ble_Un;
    }
    public class Ble_Un_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Cgt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Ble_Un_S;
    }


    public class BltConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Clt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Blt;
    }
    public class Blt_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Clt({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Blt_S;
    }
    public class Blt_UnConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Clt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Blt_Un;
    }
    public class Blt_Un_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(RTCLI::Clt_Un({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Blt_Un_S;
    }
    public class Bne_UnConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction;
            return $"if(!RTCLI::Ceq({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bne_Un;
    }
    public class Bne_Un_SConverterCXX : IControlFlowConverter
    {
        public string Convert(Instruction instruction, MethodTranslateContextCXX methodContext)
        {
            var op2 = methodContext.CmptStackPopObject;
            var op1 = methodContext.CmptStackPopObject;
            Instruction to = instruction.Operand as Instruction; 
            return $"if(!RTCLI::Ceq({op1}, {op2})) goto {to.GetLabel()};";
        }
        public OpCode TargetOpCode() => OpCodes.Bne_Un_S;
    }
}