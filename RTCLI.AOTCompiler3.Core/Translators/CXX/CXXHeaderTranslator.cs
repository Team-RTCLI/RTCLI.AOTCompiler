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
            // [H1000] UberHeader
            CXXHeaderRules.GenerateUberHeader(Storage, FocusedAssembly);
            foreach (var Module in FocusedAssembly.Modules)
            {
                foreach(var Type in Module.Types)
                {
                    var codeWriter = Storage.Wirter(Type.CXXHeaderPath());
                    codeWriter.WriteLine(Constants.CopyRight);
                    // [H0000] Include Protect
                    CXXHeaderRules.WriteIncludeProtect(codeWriter, Type);
                    codeWriter.WriteLine(EnvIncludes);
                    codeWriter.WriteLine();

                    // [H0001] Forward Declaration
                    CXXHeaderRules.WriteForwardDeclaration(codeWriter, Type);
                    codeWriter.WriteLine();

                    // [H2003] namespace
                    using (var ___ = new CXXNamespaceScope(codeWriter, Type.CXXNamespace()))
                    {
                        WriteTypeRecursively(codeWriter, Type);
                    }
                    codeWriter.Flush();
                }
            }
        }

        private void WriteTypeRecursively(CodeTextWriter codeWriter, TypeDefinition type)
        {
            if (type.HasGenericParameters)
                codeWriter.WriteLine(CXXHeaderRules.GenericDeclaration(type));

            string TypeDecl = type.IsValueType ? CXXHeaderRules.StructDeclaration(type) :
                                 type.IsInterface ? CXXHeaderRules.InterfaceDeclaration(type) :
                                 CXXHeaderRules.ClassDeclaration(type);

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
                // [H2001] Method Signatures
                CXXHeaderRules.WriteMethodSignatures(codeWriter, type);

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

            // [H2004] Boxed ValueType
            if (!type.IsValueType)
                return;
            CXXHeaderRules.WriteBoxedValueType(codeWriter, type);
        }

        private string EnvIncludes => "#include <RTCLI/RTCLI.hpp>";
    }
}
