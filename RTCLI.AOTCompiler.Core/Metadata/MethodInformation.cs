using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public class MethodInformation : IMemberInformation
    {
        public MethodInformation(MethodDefinition def)
        {
            this.definition = def;

            if(def.HasBody)
            foreach (var Inst in def.Body.Instructions)
            {
                Instructions.Add(new InstructionInformation(Inst));
            }
        }

        [JsonIgnore] public MethodBody Body => definition.Body;
        [JsonIgnore] private readonly MethodDefinition definition = null;
        public readonly List<InstructionInformation> Instructions = new List<InstructionInformation>();
        public IMetadataTokenProvider Definition => definition;
    }
}