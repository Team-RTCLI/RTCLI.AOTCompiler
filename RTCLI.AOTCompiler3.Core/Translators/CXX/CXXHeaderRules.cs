﻿using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using RTCLI.AOTCompiler3.Meta;
using System.Linq;

namespace RTCLI.AOTCompiler3.Translators
{
    public static class CXXHeaderRules
    {
        [H0000()]
        public static void WriteIncludeProtect(CodeTextWriter Writer)
        {
            Writer.WriteLine("// [H0000] Include Protect");
            Writer.WriteLine("#pragma once");
        }
        
        [H0001()]
        public static void WriteForwardDeclaration(CodeTextWriter Writer, TypeDefinition Type)
        {
            Writer.WriteLine("// [H0001] Forward Declaration");
            var rs = Type.WeakReferences();
            foreach (var r in rs)
            {
                if(r.HasGenericParameters || r.IsGenericInstance)
                {

                }
                else if(!r.IsImplementedByVM())
                {
                    Writer.WriteLine(
                    $"namespace {r.CXXNamespace()} " + "{ "
                    + $"struct {r.CXXShortTypeName()}; "
                    + "}");
                }
            }
        }

        [H1000()]
        public static void GenerateUberHeader(CodeTextStorage Storage, AssemblyDefinition FocusedAssembly)
        {
            var uberHeaderWriter = Storage.Wirter(Constants.CXXUberHeaderName);
            uberHeaderWriter.WriteLine("// [H1000] UberHeader");
            foreach (var Module in FocusedAssembly.Modules)
            {
                foreach (var Type in Module.Types)
                {
                    uberHeaderWriter.WriteLine($"#include \"{Type.CXXHeaderPath()}\"");
                }
            }
            uberHeaderWriter.Flush();
        }

        [H1001()]
        public static void IncludeStrongReferences(CodeTextWriter Writer, TypeDefinition Type)
        {
            var StrongRefernces = Type.StrongRefernces();
            if(StrongRefernces.Count != 0)
            {
                Writer.WriteLine("// [H1001] Strong Referenced Types Headers");
                foreach (var R in StrongRefernces)
                {
                    if (!R.IsImplementedByVM())
                        Writer.WriteLine($"#include <{R.CXXHeaderPath()}>");
                }
            }
        }

        public static string CondStr(bool Cond, string Str)
        {
            return Cond ? Str : "";
        }
        [H2001()]
        public static void WriteMethodSignatures(CodeTextWriter Writer, TypeDefinition Type, bool ValueType)
        {
            if (Type.Methods != null & Type.Methods.Count != 0)
            {
                Writer.WriteLine("// [H2001] Method Signatures");
                List<MethodDefinition> overrided = new List<MethodDefinition>();
                foreach (var method in Type.Methods)
                {
                    if(!ValueType && method.HasOverrides && method.IsVirtual)
                    {
                        foreach(var od in method.Overrides)
                        {
                            var odd = od.Resolve();
                            overrided.Add(odd);
                            Writer.WriteLine($"{odd.CXXMethodSignature()} override{CondStr(method.IsAbstract, "= 0")}{CondStr(method.IsFinal, " final")};");
                        }
                    }
                    else if (!ValueType && method.IsVirtual)
                    {
                        if(method.IsNewSlot)
                            Writer.WriteLine($"virtual {method.CXXMethodSignature()}{CondStr(method.IsAbstract, " = 0")}{CondStr(method.IsFinal, " final")};");
#if ENABLE_EXPLICT_OVERIDE
                        if(!Type.IsInterface && !method.IsAbstract)
                        {
                            Writer.WriteLine($"{method.CXXRetType()} {method.CXXRowName()}_Impl{method.CXXParamSequence(true)};");
                            foreach (var i in Type.Interfaces)
                            {
                                var itype = i.InterfaceType.Resolve();
                                foreach (var mtdd in itype.Methods)
                                {
                                    if (mtdd.Name == method.Name && !overrided.Contains(mtdd))
                                    {
                                        Writer.WriteLine($"virtual {mtdd.CXXMethodSignature()}{CondStr(method.IsAbstract, " = 0")}{CondStr(method.IsFinal, " final")};");
                                    }
                                }
                            }
                        }
#endif
                    }
                    else
                    {
                        if (method.HasGenericParameters)
                            Writer.WriteLine($"template<{method.CXXTemplateParam()}>");

                        Writer.WriteLine($"{method.CXXMethodSignature()};");
                    }
                }
                Writer.WriteLine();
            }
        }

        public static void WriteEnumType(CodeTextWriter Writer, TypeDefinition Type)
        {
            if (!Type.IsEnum)
                return;
            Writer.WriteLine($"using {Type.CXXShortTypeName()} = {Type.Fields.First().FieldType.CXXTypeName()};");
            string classDef = $"struct {Type.CXXShortTypeName()}_V : public RTCLI::System::Enum";
            using (var classScope = new CXXScopeDisposer(Writer, classDef, true,
                $"// [H2004] Boxed ValueType {Type.CXXTypeName()}_V ",
                $"// [H2004] Exit Boxed ValueType {Type.CXXTypeName()}_V"))
            {
                Writer.unindent().WriteLine("public:").indent();
                Writer.WriteLine($"using ValueType = {Type.CXXShortTypeName()};");
                //Writer.WriteLine($"using ValueType = struct {type.CXXShortTypeName()};");
                Writer.WriteLine($"{Type.CXXShortTypeName()} value;");
                foreach(var Field in Type.Fields.Skip(1))
                    Writer.WriteLine($"static constexpr {Type.CXXShortTypeName()} {Field.Name} = {Field.Constant};");
            }
        }

        [H2004()]
        public static void WriteBoxedValueType(CodeTextWriter Writer, TypeDefinition Type)
        {
            if (!Type.IsValueType)
                return;
            var solved = Type.InterfacesSolved();
            string Interfaces = string.Join(',', solved.Select(a => $"public {a.InterfaceType.CXXTypeName()}"));

            // [H2004] Boxed ValueType
            if (Type.HasGenericParameters)
                Writer.WriteLine($"template<{Type.CXXTemplateParam()}>");
            string classDef = $"struct {Type.CXXShortTypeName()}_V : public RTCLI::System::ValueType{(solved.Count == 0 ? "" : "," + Interfaces)}";
            using (var classScope = new CXXScopeDisposer(Writer, classDef, true,
                $"// [H2004] Boxed ValueType {Type.CXXTypeName()}_V ",
                $"// [H2004] Exit Boxed ValueType {Type.CXXTypeName()}_V"))
            {
                Writer.unindent().WriteLine("public:").indent();
                Writer.WriteLine($"using ValueType = {Type.CXXShortTypeName()};");
                //Writer.WriteLine($"using ValueType = struct {type.CXXShortTypeName()};");
                Writer.WriteLine($"{Type.CXXShortTypeName()} value;");
                WriteMethodSignatures(Writer, Type, false);
            }
        }

        [H2005()]
        public static void WriteFieldDeclaration(CodeTextWriter Writer, TypeDefinition Type)
        {
            if (Type.Fields != null & Type.Fields.Count != 0)
            {
                Writer.WriteLine("// [H2005] Field Declarations");
                foreach (var field in Type.Fields)
                {
                    Writer.WriteLine(field.CXXFieldDeclaration());
                }
                Writer.WriteLine();
            }
        }

        [H9999()]
        public static void CopyWrite(CodeTextWriter Writer)
        {
            Writer.WriteLine("// [H9999] Copyright");
            Writer.WriteLine(Constants.CopyRight);
        }


        [C0001()]
        public static string StructDeclaration(TypeDefinition type)
        {
            var solved = type.InterfacesSolved();
            string Interfaces = string.Join(',', solved.Select(a => $"public {a.InterfaceType.CXXTypeName()}"));

            return $"/*[C0001]*/struct {type.CXXShortTypeName()}";
        }

        [C0002()]
        public static string InterfaceDeclaration(TypeDefinition type)
        {
            var solved = type.InterfacesSolved();
            string Interfaces = string.Join(',', solved.Select(a => $"public {a.InterfaceType.CXXTypeName()}"));

            return $"/*[C0002]*/interface {type.CXXShortTypeName()} {(type.Interfaces.Count > 0 ? ": " + Interfaces : "")}";
        }

        [C0003()]
        public static string ClassDeclaration(TypeDefinition type)
        {
            var solved = type.InterfacesSolved();
            string Interfaces = string.Join(',', solved.Select(a => $"public {a.InterfaceType.CXXTypeName()}"));
            string BaseType = type.BaseType != null ? type.BaseType.CXXTypeName() : "RTCLI::System::Object";

            return $"/*[C0003]*/struct {type.CXXShortTypeName()} : public {BaseType}{(type.Interfaces.Count > 0 ? "," + Interfaces : "")}";
        }

        [C0004()]
        public static string GenericDeclaration(TypeDefinition type)
        {
            return $"/*[C0004]*/template<{type.CXXTemplateParam()}>";
        }
    }

    [H2000]
    public class CXXTypeScope : CXXScopeDisposer
    {
        public CXXTypeScope(CodeTextWriter parent, TypeDefinition type)
            : base(parent, 
                  type.IsValueType ? CXXHeaderRules.StructDeclaration(type) : // [C0001]
                  type.IsInterface ? CXXHeaderRules.InterfaceDeclaration(type) : // [C0002]
                  CXXHeaderRules.ClassDeclaration(type), //[C0003]
                  true,
                  $"// [H2000] TypeScope {type.CXXTypeName()} ",
                  $"// [H2000] Exit TypeScope {type.CXXTypeName()}")
        {

        }
    }
    
    [H2003()]
    public class CXXNamespaceScope : CXXScopeDisposer
    {
        public CXXNamespaceScope(CodeTextWriter parent, string CXXNamespace)
            :base(parent, "namespace " + CXXNamespace,
                 false, "// [H2003] namespace", "// [H2003] Exit namespace")
        {

        }
    }
}
