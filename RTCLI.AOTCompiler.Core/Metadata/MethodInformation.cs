using System;
using System.Collections.Generic;
using System.Globalization;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Newtonsoft.Json;
using System.Linq;

namespace RTCLI.AOTCompiler.Metadata
{
    public partial class MethodInformation : IMemberInformation
    {
        public MethodInformation(MethodDefinition def, MetadataContext metadataContext)
        {
            this.definition = def;
            this.MetadataContext = metadataContext;

            if (def.HasBody)
            {
                foreach (var Inst in def.Body.Instructions)
                {
                    Instructions.Add(new InstructionInformation(Inst));
                }
                if(def.Body.HasVariables)
                {
                    foreach (var localVar in def.Body.Variables)
                    {
                        LocalVariables.Add(new VariableInformation(localVar, metadataContext));
                    }
                }
                if(def.HasParameters)
                {
                    foreach (ParameterDefinition param in def.Parameters)
                    {
                        Parameters.Add(new ParameterInformation(param, metadataContext));
                    }
                }
                if(def.HasGenericParameters)
                    genericParameterTypes = def.GenericParameters.Select(a => MetadataContext.GetTypeInformation(a)).ToArray();
            }
        }

        [JsonIgnore] public MethodBody Body => definition.Body;
        [JsonIgnore] private readonly MethodDefinition definition = null;

        public bool InitLocals => definition.Body!=null ? definition.Body.InitLocals : false;
        
        public bool IsPrivate => definition.IsPrivate;
        public bool IsStatic => definition.IsStatic;
        public bool IsPublic => definition.IsPublic;
        public bool IsFamily => definition.IsFamily;
        public bool HasGenericParameters => definition.HasGenericParameters;

        public readonly List<InstructionInformation> Instructions = new List<InstructionInformation>();
        public readonly List<VariableInformation> LocalVariables = new List<VariableInformation>();
        public readonly List<ParameterInformation> Parameters = new List<ParameterInformation>();
        public readonly TypeInformation[] genericParameterTypes = null;
        public IMetadataTokenProvider Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}