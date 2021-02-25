using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;

namespace RTCLI.AOTCompiler3.Translators
{
    public class CXXSourceTranslator : IRTCLITranslator
    {
        // ${OutputPath}/${Assembly}/src
        public void Run(CodeTextStorage Storage, AssemblyDefinition FocusedAssembly)
        {
            foreach (var Module in FocusedAssembly.Modules)
            {
                foreach (var Type in Module.Types)
                {
                    var Writer = Storage.Wirter(Path.Combine(Type.CXXNamespaceToPath(), Type.CXXShortTypeName() + ".cpp"));

                    // [S9999] Copyright
                    CXXSourceRules.Copyright(Writer);
                    // [S1000] Include Uber Headers.
                    CXXSourceRules.IncludeUberHeaders(Writer, Type);

                    // [S0001] Close Unused-Label Warning
                    using (var no_unused_lables = new ScopeNoUnusedWarning(Writer))
                    {
                        Writer.WriteLine("");
                        WriteMethodRecursive(Writer, Type, Type.IsValueType);
                        if(Type.IsValueType)
                            WriteMethodRecursive(Writer, Type, false);
                        Writer.WriteLine("");


                        Writer.WriteLine("");
                        Writer.WriteLine("// [S0000] Generate Test Point.");
                        Writer.WriteLine($"#ifdef RTCLI_TEST_POINT");
                        Writer.WriteLine("int main(void){");
                        Writer.WriteLine($"\t{Type.CXXTypeName()}::Test();");
                        Writer.WriteLine("\treturn 0;");
                        Writer.WriteLine("}");
                        Writer.WriteLine($"#endif");
                    }

                    Writer.Flush();
                }
            }
        }

        public void WriteMethodRecursive(CodeTextWriter Writer, TypeDefinition Type, bool ValueType)
        {
            foreach (var Nested in Type.NestedTypes)
            {
                WriteMethodRecursive(Writer, Nested, ValueType);
            }
            foreach (var Method in Type.Methods)
            {
                if (Method.Body == null)
                    continue;
                // [S2000] Method Body
                List<MethodDefinition> overrided = new List<MethodDefinition>();
                if (!ValueType && Method.HasOverrides && Method.IsVirtual)
                {
                    foreach (var od in Method.Overrides)
                    {
                        var odd = od.Resolve();
                        overrided.Add(odd);
                        if (Type.HasGenericParameters)
                            Writer.WriteLine($"template<{Type.CXXTemplateParam()}>");
                        Writer.WriteLine(odd.CXXMethodImplSignature(ValueType, Type));
                        CXXSourceRules.WriteMethodBody(Writer, Method, ValueType);
                    }
                }
                else if(!ValueType && Method.IsVirtual)
                {
                    if (Type.HasGenericParameters)
                        Writer.WriteLine($"template<{Type.CXXTemplateParam()}>");
                    Writer.WriteLine($"{Method.CXXRetType()} {Type.CXXMethodDeclarePrefix(ValueType)}::{Method.CXXRowName()}_Impl{Method.CXXParamSequence(true)}");
                    CXXSourceRules.WriteMethodBody(Writer, Method, ValueType);
                    if (Method.IsNewSlot)
                    {
                        if (Type.HasGenericParameters)
                            Writer.WriteLine($"template<{Type.CXXTemplateParam()}>");
                        Writer.WriteLine(Method.CXXMethodImplSignature(ValueType));
                        Writer.WriteLine($"{{ return {Method.CXXRowName()}_Impl{Method.CXXArgSequence()}; }}");
                    }
                    if (!Type.IsInterface)
                    {
                        foreach (var i in Type.Interfaces)
                        {
                            var itype = i.InterfaceType.Resolve();
                            foreach (var mtdd in itype.Methods)
                            {
                                if (mtdd.Name == Method.Name && !overrided.Contains(mtdd))
                                {
                                    if (Type.HasGenericParameters)
                                        Writer.WriteLine($"template<{Type.CXXTemplateParam()}>");
                                    Writer.WriteLine(mtdd.CXXMethodImplSignature(ValueType, Type));
                                    Writer.WriteLine($"{{ return {Method.CXXRowName()}_Impl{Method.CXXArgSequence()}; }}");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Type.HasGenericParameters)
                        Writer.WriteLine($"template<{Type.CXXTemplateParam()}>");
                    if (Method.HasGenericParameters)
                        Writer.WriteLine($"template<{Method.CXXTemplateParam()}>");

                    Writer.WriteLine(Method.CXXMethodImplSignature(ValueType));

                    CXXSourceRules.WriteMethodBody(Writer, Method, ValueType);
                }

            }
        }

        private string EnvIncludes => "#include <RTCLI/RTCLI.hpp>";
    }
}
