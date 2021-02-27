using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    class TestDelegate
    {
        public delegate void DelegateA(float arg);
        event DelegateA e;
        public static float StaticFunc(int a) { return 1; }
        public static void Test(TestDelegate obj)
        {
            DelegateA arg = null;
            arg(1.0f);
            float upvalue = 0;
            arg = arg2 => { upvalue = arg2; };
            arg(2.0f);
            obj.e += arg => { };
            Func<float, float> asd = arg2 => arg2;
            Func<int, float> aaa = StaticFunc;
        }
    }
}
