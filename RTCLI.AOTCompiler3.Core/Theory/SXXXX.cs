using System;
using System.Collections.Generic;
using System.Text;


namespace RTCLI.AOTCompiler3
{
    public class SAttribute : System.Attribute
    {
        public SAttribute()
        {

        }
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/ggxmal
    public class S0001 : SAttribute
    {
        public S0001()
        {

        }
        static string description => "Close Unused-Label Warnings";
    }
    
    // doc: https://www.yuque.com/oy5oo6/su8qgw/ggxmal
    public class S1000 : SAttribute
    {
        public S1000()
        {

        }
        static string description => "Include Uber Headers";
    }
    
    // doc: https://www.yuque.com/oy5oo6/su8qgw/ggxmal
    public class S2000 : SAttribute
    {
        public S2000()
        {

        }
        static string description => "Method Body";
    }
    
    // doc: https://www.yuque.com/oy5oo6/su8qgw/ggxmal
    public class S9999 : SAttribute
    {
        public S9999()
        {

        }
        static string description => "Copyright";
    }
}