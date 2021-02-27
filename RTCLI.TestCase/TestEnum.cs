using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    class TestEnum
    {
        enum MyEnum
        {
            A,B,C
        }
        enum FlagEnum
        {
            A = 0x1, B = 0x2, C = 0x4
        }
        enum LongEnum : ulong
        {
            A, B, C
        }
        void Test()
        {
            MyEnum a = MyEnum.A;
            MyEnum b = MyEnum.B;
            int c;
            LongEnum d = LongEnum.C;
            c = (int)a;
            a = b;
            b = (MyEnum)c;
            c = (int)d;
            FlagEnum E = FlagEnum.A;
            FlagEnum F = FlagEnum.B;
            E = E | F;
        }
    }
}
