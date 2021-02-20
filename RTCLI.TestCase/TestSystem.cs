using System;

namespace RTCLI.TestCase
{
    public class TestSystem
    {
        public class TestInnerClass
        {
            public TestInnerClass(int id)
            {
                this.ID = id;
            }
            public void CallTest(int u)
            {
                System.Console.WriteLine("Call Test");
                return;
            }
            public void CallTestF(float u)
            {
                System.Console.WriteLine("Call Test F");
                return;
            }
            public int ID { get; }
        };
        public TestSystem(int val)
        {

        }
        public static TestSystem operator +(TestSystem b, TestSystem c)
        {
            return b;
        }
        public void MethodWithArgsAccess(int argInt, TestInnerClass argClass)
        {
            TestInnerClass test;
            var argI = argInt;
            var argC = argClass;
        }
   
        public float prop { get; set; }

        public void MethodWithArgsOps(int arg0, int arg1)
        {
            var arg = arg0 + arg1;
            arg -= arg0;
        }
        public void MethodWithArgsFieldAccess(int argInt, TestInnerClass argClass)
        {
            //var arg6Name = arg666.Name;
            int a = 11111111;
            int b = 11111111;
            int c = a + b;
            argClass.CallTest(9);
            argClass.CallTestF(9);
        }
    }
}
