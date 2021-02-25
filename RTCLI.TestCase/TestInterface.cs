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

        public interface Interface1
        {
            public float a();
        }

        public interface Interface2 : Interface1
        {
            public float a();
        }

        public interface Interface3 : Interface2
        {
            public new float a() => 3;
        }

        public interface Interface4 : Interface3
        {
            public float a() => 4;
        }

        public class A : Interface4
        {
            float Interface1.a() => 1;
            float Interface2.a() => 2;
        }

        bool TestFunc<T>(IEquatable<T> arg1, T arg2)
        {
            return arg1.Equals(arg2);
        }

        bool TestFunc2<T>(T arg1, T arg2) where T : IEquatable<T>
        {
            return arg1.Equals(arg2);
        }

        bool TestGeneric()
        {
            TestStruct a = new TestStruct();
            TestStruct b = new TestStruct();
            TestFunc2(a, b);
            return TestFunc(a, b);
        }

        void Test()
        {
            var Test = new TestInterface.A();
            var I1 = (TestInterface.Interface1)Test;
            var I2 = (TestInterface.Interface2)Test;
            var I3 = (TestInterface.Interface3)Test;
            var I4 = (TestInterface.Interface4)Test;
            Console.WriteLine(I1.a());
            Console.WriteLine(I2.a());
            Console.WriteLine(I3.a());
            Console.WriteLine(I4.a());
        }
    }
}
