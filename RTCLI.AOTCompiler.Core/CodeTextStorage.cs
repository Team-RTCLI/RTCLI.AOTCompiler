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
        public readonly string BasePath;
        private readonly Stack<string> scopeNames = new Stack<string>();

        public CodeTextStorage(TextWriter logw, string basePath, string indent)
        {
            this.logw = logw;
            this.BasePath = basePath;
        }

        public IDisposable EnterScope(string scopeName, bool splitScope = true)
        {
            scopeNames.Push(splitScope ? Utilities.GetCXXLanguageScopedPath(scopeName) : scopeName);
            return new ScopeDisposer(this);
        }

        public CodeTextWriter Wirter(string FileName)
        {
            string fullFileName = BasePath;
            foreach(var scope in scopeNames)
            {
                fullFileName = Path.Combine(fullFileName, scope);
            }
            return new CodeTextWriter(Path.Combine(fullFileName, FileName));
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
