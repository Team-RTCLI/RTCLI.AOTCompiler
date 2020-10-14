using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using RTCLI.AOTCompiler.Internal;

namespace RTCLI.AOTCompiler
{
    public class CodeTextStorage
    {
        private readonly TextWriter logw;
        private readonly Stack<string> scopeNames = new Stack<string>();

        public CodeTextStorage(TextWriter logw, string basePath, string indent)
        {
            this.logw = logw;
        }

        public IDisposable EnterScope(string scopeName, bool splitScope = true)
        {
            scopeNames.Push(splitScope ? Utilities.GetCXXLanguageScopedPath(scopeName) : scopeName);
            return new ScopeDisposer(this);
        }

        private sealed class ScopeDisposer : IDisposable
        {
            private CodeTextStorage parent;
            public ScopeDisposer(CodeTextStorage parent)
            {
                this.parent = parent;
            }

            public void Dispose()
            {
                if (parent != null)
                {
                    parent.scopeNames.Pop();
                    parent = null;
                }
            }
        }
    }
}
