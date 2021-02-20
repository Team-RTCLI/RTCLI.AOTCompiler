using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    class TestDelegate
    {
        public delegate void DelegateA(float arg);
        public static void Test()
        {
            DelegateA arg = null;
            arg(1.0f);
            float upvalue = 0;
            arg = arg2 => { upvalue = arg2; };
            arg(2.0f);
            Func<float, float> asd = arg2 => arg2;
        }
    }
}
