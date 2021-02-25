using System;
using System.Collections.Generic;
using System.Text;

namespace RTCLI.AOTCompiler3
{
    public class HAttribute : System.Attribute
    {

        public HAttribute()
        {

        }
    }
    
    // doc: https://www.yuque.com/oy5oo6/su8qgw/hn6b42
    public class H0000 : HAttribute
    {
        public H0000()
        {

        }
        static string description => "Include Protect";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/repik3
    public class H0001 : HAttribute
    {
        public H0001()
        {

        }
        static string description => "Forward Declaration";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/xf2tgm
    public class H1000 : HAttribute
    {
        public H1000()
        {

        }
        static string description => "Uber Header";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/ahm9xi
    public class H2001 : HAttribute
    {
        public H2001()
        {

        }
        static string description => "Method Signatures";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/ykpc5k
    public class H2003 : HAttribute
    {
        public H2003()
        {

        }
        static string description => "namespace";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/bg2gtq
    public class H2004 : HAttribute
    {
        public H2004()
        {

        }
        static string description => "Boxed ValueType";
    }

}
