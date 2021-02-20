using System;
using System.Collections.Generic;
using System.Text;

namespace RTCLI.TestCase
{
     public class TestInnerClass
    {
        public class Inner
        {
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
        };

        public void Test(TestInnerClass argClass)
        {
            TestInnerClass test = argClass;
        }

    }
}
