using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
    class TestInterface
    {
        public struct TestStruct : IEquatable<TestStruct>
        {
            float a;

            public bool Equals(TestStruct other)
            {
                return a == other.a;
            }
        }

        bool TestFunc<T>(IEquatable<T> arg1, T arg2)
        {
            return arg1.Equals(arg2);
        }

        bool TestFunc2<T>(T arg1, T arg2) where T : IEquatable<T>
        {
            return arg1.Equals(arg2);
        }

        bool Test()
        {
            TestStruct a = new TestStruct();
            TestStruct b = new TestStruct();
            TestFunc2(a, b);
            return TestFunc(a, b);
        }
    }
}
