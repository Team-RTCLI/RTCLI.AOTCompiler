using System;
using System.Collections.Generic;
using System.Globalization;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Newtonsoft.Json;

namespace RTCLI.AOTCompiler.Metadata
{
    public class MethodInformation : IMemberInformation
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
            }
        }

        [JsonIgnore] public MethodBody Body => definition.Body;
        [JsonIgnore] private readonly MethodDefinition definition = null;

        public bool InitLocals => definition.Body.InitLocals;


        private string CXXParamsSequence()
        {
            string sequence = "\n\t";
            //Since LdArg.0 -> this, start argument index from 1
            uint i = 1;
            foreach(var param in Parameters)
            {
                sequence += param.CXXParamDecorated + " " + param.Name;
                if (i++ != Parameters.Count)
                    sequence = sequence + ", " + ((i%3==1)?"\n\t":"");
            }
            return sequence;
        }
        public string CXXMethodName 
            =>  // Return Type
             MetadataContext.GetTypeInformation(definition.DeclaringType)?.CXXTypeName //Type Name
             + "::" + definition?.Name;//MethodName
        public string CXXMethodNameShort => definition?.Name;
        public string CXXParamSequence => "(" + CXXParamsSequence() + ")"; //Param Sequence
        public string CXXRetType => MetadataContext.GetTypeInformation(definition.ReturnType)?.CXXTypeName;
        public string CXXStackName => $"{string.Join("_", CXXMethodName.Split("::"))}__Stack";

        public readonly List<InstructionInformation> Instructions = new List<InstructionInformation>();
        public readonly List<VariableInformation> LocalVariables = new List<VariableInformation>();
        public readonly List<ParameterInformation> Parameters = new List<ParameterInformation>();
        public IMetadataTokenProvider Definition => definition;
        public MetadataContext MetadataContext { get; }
    }
}