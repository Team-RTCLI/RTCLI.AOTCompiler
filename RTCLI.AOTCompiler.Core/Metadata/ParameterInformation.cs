using Mono.Cecil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTCLI.AOTCompiler.Metadata
{
    public class ParameterInformation
    {
        public ParameterInformation(ParameterDefinition defination, MetadataContext context)
        {
            this.MetadataContext = context;
            this.Definition = defination;
        }
        public string CXXParamDecorated
        {
            get
            {
                if (Definition.ParameterType.IsGenericParameter)
                    return $"RTCLI::TVar<{CXXTypeName}>";
                if (!IsValueType || (Definition.IsOut || Definition.IsIn))
                    return CXXTypeName + "&";

                return CXXTypeName;
            }
        }
        public string CXXTypeName
            => MetadataContext.GetTypeInformation(Definition.ParameterType).CXXTypeName;

        public bool IsByReference => Definition.ParameterType.IsByReference;

        public string Name => Definition.Name;
        public bool IsValueType => Definition.ParameterType.IsValueType;
        [JsonIgnore] public TypeInformation Type => MetadataContext.GetTypeInformation(Definition.ParameterType);
        [JsonIgnore] public readonly ParameterDefinition Definition = null;
        [JsonIgnore] public MetadataContext MetadataContext { get; }
    }
}
