using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    class TestReference
    {
        public struct Struct { }
        public class Class { public TestReference field; }
        public ref Struct RefStruct(ref Struct a) 
        { 
            a = new Struct(); 
            return ref a;
        }
        public ref Class RefClass(ref Class a) 
        { 
            a = new Class();
            return ref a;
        }
        public ref TestReference RefField()
        {
            var a = new Class();
            return ref a.field;
        }
    }
}
