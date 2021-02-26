using System;
using System.Collections.Generic;
using System.Text;


namespace RTCLI.AOTCompiler3
{
    public class HAttribute : System.Attribute
    {
        public HAttribute() {}
    }
    
    // doc: https://www.yuque.com/oy5oo6/su8qgw/hn6b42
    public class H0000 : HAttribute
    {
        public H0000() {}
        static string description => "Include Protect";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/repik3
    public class H0001 : HAttribute
    {
        public H0001() {}
        static string description => "Forward Declaration";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/xf2tgm
    public class H1000 : HAttribute
    {
        public H1000() {}
        static string description => "Uber Header";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/cb16dg
    public class H1001 : HAttribute
    {
        public H1001() {}
        static string description => "Strong Reference Headers";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/mzg1ps
    public class H2000 : HAttribute
    {
        public H2000() {}
        static string description => "Type Scope";
    }
    
    // doc: https://www.yuque.com/oy5oo6/su8qgw/ahm9xi
    public class H2001 : HAttribute
    {
        public H2001() {}
        static string description => "Method Signatures";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/ykpc5k
    public class H2003 : HAttribute
    {
        public H2003() {}
        static string description => "namespace";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/ui239g
    public class H2002 : HAttribute
    {
        public H2002() {}
        static string description => "Inner Types";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/bg2gtq
    public class H2004 : HAttribute
    {
        public H2004() {}
        static string description => "Boxed ValueType";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/su3nw1
    public class H2005 : HAttribute
    {
        public H2005() {}
        static string description => "Field Declaration";
    }
    
    public class H9999 : HAttribute
    {
        public H9999() {}
        static string description => "Copyright";
    }
}
