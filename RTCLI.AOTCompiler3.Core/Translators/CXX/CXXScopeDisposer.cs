using System;
using System.Collections.Generic;
using System.Text;

namespace RTCLI.AOTCompiler3.Translators
{
    public class CXXScopeDisposer : IDisposable
    {
        private CodeTextWriter parent;
        private bool EndWithSemicolon = false;
        private string onExit = null;
        public CXXScopeDisposer(CodeTextWriter parent, string Scope, bool bEndWithSemicolon = false, string onEnter = null, string onExit = null)
        {
            this.parent = parent;
            this.EndWithSemicolon = bEndWithSemicolon;
            this.onExit = onExit;
            if (onEnter != null) parent.WriteLine(onEnter);
            parent.WriteLine(Scope);
            parent.WriteLine("{");
            parent.indent();
        }

        public void Dispose()
        {
            if (parent != null)
            {
                parent.unindent();
                parent.WriteLine("}" + (EndWithSemicolon ? ";" : ""));
                if (onExit != null) parent.WriteLine(onExit);
                parent.WriteLine();
                parent = null;
            }
        }
    }
}
