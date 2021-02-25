using System;
using System.Text;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;
using System.Linq;

namespace RTCLI.AOTCompiler3.Translators
{
    public class CXXHeaderTranslator : IRTCLITranslator
    {
        // ${OutputPath}/${Assembly}/include
        public void Run(CodeTextStorage Storage, AssemblyDefinition FocusedAssembly)
        {
            var uberHeaderWriter = Storage.Wirter("_UberHeader_.h");
            uberHeaderWriter.WriteLine("// [H1000] UberHeader"); 
            foreach (var Module in FocusedAssembly.Modules)
            {
                foreach(var Type in Module.Types)
                {
                    var codeWriter = Storage.Wirter(Type.CXXHeaderPath());
                    codeWriter.WriteLine(Constants.CopyRight);
                    codeWriter.WriteLine("// [H0000] Include Protect");
                    codeWriter.WriteLine("#pragma once");
                    codeWriter.WriteLine(EnvIncludes);

                    codeWriter.WriteLine();
                    codeWriter.WriteLine("// [H0001] Forward Declaration");

                    codeWriter.WriteLine();
                    using (var ___ = new CXXScopeDisposer(codeWriter,
                        "namespace " + Type.CXXNamespace(), false,
                        "// [H2003] namespace",
                        "// [H2003] Exit namespace"))
                    {
                        WriteTypeRecursively(codeWriter, Type);
                    }

                    codeWriter.Flush();
                    uberHeaderWriter.WriteLine($"#include \"{Type.CXXHeaderPath()}\"");
                }
            }
            uberHeaderWriter.Flush();
        }

        public void WriteTypeRecursively(CodeTextWriter codeWriter, TypeDefinition type)
        {
            if (type.HasGenericParameters)
                codeWriter.WriteLine($"template<{type.CXXTemplateParam()}>");
            string Interfaces = string.Join(',', type.Interfaces.Select(a => $"public {a.InterfaceType.CXXTypeName()}"));
            string BaseType = type.BaseType != null ? type.BaseType.CXXTypeName() : "RTCLI::System::Object";
            string TypeDecl = type.IsValueType ?
                                 $"struct {type.CXXShortTypeName()}" :
                                 type.IsInterface ?
                                 $"interface {type.CXXShortTypeName()} {(type.Interfaces.Count > 0 ? ": " +  Interfaces : "")}" :
                                 $"class {type.CXXShortTypeName()} : public {BaseType}{(type.Interfaces.Count > 0 ? "," + Interfaces : "")}";
            using (var classScope = new CXXScopeDisposer(codeWriter, TypeDecl, true,
                $"// [H2000] TypeScope {type.CXXTypeName()} ",
                $"// [H2000] Exit TypeScope {type.CXXTypeName()}"))
            {
                codeWriter.unindent().WriteLine("public:").indent();
                foreach (var nested in type.NestedTypes)
                {
                    codeWriter.WriteLine($"// [H2002] Inner Classes {nested.CXXShortTypeName()}");
                    WriteTypeRecursively(codeWriter, nested);
                }
                if(type.Methods != null & type.Methods.Count != 0)
                {
                    codeWriter.WriteLine("// [H2001] Method Signatures");
                    foreach (var method in type.Methods)
                    {
                        if (method.HasGenericParameters)
                            codeWriter.WriteLine($"template<{method.CXXTemplateParam()}>");
                        codeWriter.WriteLine($"{(method.IsNewSlot ? "virtual " : "")}{method.CXXMethodSignature(true)};");
                    }
                    codeWriter.WriteLine();
                }
                if (type.Fields != null & type.Fields.Count != 0)
                {
                    codeWriter.WriteLine("// [H2005] Field Declarations");
                    foreach (var field in type.Fields)
                    {
                        codeWriter.WriteLine(field.CXXFieldDeclaration());
                    }
                    codeWriter.WriteLine();
                }
            }

            if (!type.IsValueType)
                return;

            // [H2004] Boxed ValueType
            if (type.HasGenericParameters)
                codeWriter.WriteLine($"template<{type.CXXTemplateParam()}>");
            string classDef = $"class {type.CXXShortTypeName()}_V : public RTCLI::System::ValueType{Interfaces}";
            using (var classScope = new CXXScopeDisposer(codeWriter, classDef, true,
                $"// [H2004] Boxed ValueType {type.CXXTypeName()}_V ",
                $"// [H2004] Exit Boxed ValueType {type.CXXTypeName()}_V"))
            {
                codeWriter.unindent().WriteLine("public:").indent();
                codeWriter.WriteLine($"using ValueType = {type.CXXShortTypeName()};");
                //codeWriter.WriteLine($"using ValueType = struct {type.CXXShortTypeName()};");
                codeWriter.WriteLine($"{type.CXXShortTypeName()} value;");
                foreach (var method in type.Methods)
                {
                    if (method.HasGenericParameters)
                        codeWriter.WriteLine($"template<{method.CXXTemplateParam()}>");
                    codeWriter.WriteLine($"RTCLI_FORCEINLINE {method.CXXMethodSignature(true)} {{ value.{method.CXXShortMethodName()}{method.CXXArgSequence()}; }}");
                }
            }

        }

        private string EnvIncludes => "#include <RTCLI/RTCLI.hpp>";
    }
}
