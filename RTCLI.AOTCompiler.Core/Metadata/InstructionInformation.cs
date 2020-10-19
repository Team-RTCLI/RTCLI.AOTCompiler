using Mono.Cecil;
using Mono.Cecil.Cil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTCLI.AOTCompiler.Metadata
{
    public struct InstructionInformation 
    {
        public InstructionInformation(Instruction inst)
        {
            instruction = inst;
        }

        [JsonIgnore] Instruction instruction;
        public string Body => instruction.ToString();
    }
}
