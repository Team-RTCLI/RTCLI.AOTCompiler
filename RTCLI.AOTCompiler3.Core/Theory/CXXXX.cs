using System;
using System.Collections.Generic;
using System.Text;

namespace RTCLI.AOTCompiler3
{
    public class CAttribute : System.Attribute
    {
        public CAttribute()
        {

        }
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/mr3l89
    public class C0001 : CAttribute
    {
        public C0001()
        {

        }
        static string description => "value type";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/teuw4w
    public class C0002 : CAttribute
    {
        public C0002()
        {

        }
        static string description => "interface";
    }

    // doc: https://www.yuque.com/oy5oo6/su8qgw/hx1ev5
    public class C0003 : CAttribute
    {
        public C0003()
        {

        }
        static string description => "class";
    }
    
    // doc: https://www.yuque.com/oy5oo6/su8qgw/lh5pw3
    public class C0004 : CAttribute
    {
        public C0004()
        {

        }
        static string description => "generic";
    }



}