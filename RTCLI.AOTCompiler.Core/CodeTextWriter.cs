using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RTCLI.AOTCompiler
{
    public class CodeTextWriter
    {
        public CodeTextWriter(string relatedPath)
        {
            sw = new StreamWriter(relatedPath, false, Encoding.UTF8);
        }

        public void WriteLine(string toWrite)
        {
            sw.WriteLine(toWrite);
            sw.Flush();
        }

        private StreamWriter sw = null;
    }
}
