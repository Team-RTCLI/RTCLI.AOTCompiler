using System;
using System.Collections.Generic;
using System.Text;

namespace TestCase
{
     public class TestInnerClass
    {
        public class Inner
        {
            public class InnerInner
            {
                public InnerInner(int id)
                {

                }
                public int ID;
            }
            public Inner(int id)
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
            public int ID2 { get; }
        };

        public static void Test()
        {
            TestInnerClass test = null;
        }

    }
}
