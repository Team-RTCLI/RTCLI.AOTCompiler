using System;

using RTCLITestCase.Reference;

namespace RTCLITestCase
{
    public struct PureStruct
    {
        public String name;
        public float val;
    }

    public class TestCaseClass
    {
        public class TestInnerClass
        {

        };
        public TestCaseClass(string str)
        {

        }
        public static TestCaseClass operator +(TestCaseClass b, TestCaseClass c)
        {
            return b;
        }
        public void MethodWithArgsAccess(int argInt, string argStr, RefClass argClass, RefStruct argStruct)
        {
            TestInnerClass test;
            var argI = argInt;
            var argSt = argStr;
            var argS = argStruct;
            var argC = argClass;
        }
   
        public float prop { get; set; }

        public void MethodWithArgsOps(int arg0, int arg1)
        {
            var arg = arg0 + arg1;
            arg -= arg0;
        }
        public void MethodWithArgsFieldAccess(int argInt, string argStr, RefClass argClass, RefStruct argStruct)
        {
            //var arg6Name = arg666.Name;
            int a = 11111111;
            int b = 11111111;
            int c = a + b;
            Single.Parse("1.22");
            argClass.CallTest(9);
            argClass.CallTestF(9);
            argStruct.Name = " Accessed";
        }
        public void MethodWithArgsVirtCall(int argInt, string argStr, RefClass argClass, RefStruct argStruct)
        {
            var argC = argClass;
            var argCName = argClass.Name;
        }
        public object ThrowTest(object value)
        {
            try
            {
                if(value != null)
                    throw new Exception();
            }
            catch
            {
                return value;
            }
            return null;
        }
        public void ArrayTest(int argInt)
        {
            int[] list = new int[argInt];
            list[0] = 1111111;
            list[argInt] = 111111;
            list[0] = argInt;
            argInt = list[0];
            argInt = list[argInt];
        }
        public float StructTest(PureStruct arg)
        {
            PureStruct local = new PureStruct();
            local.name = "asd";
            arg = local;
            arg = new PureStruct();
            arg = new PureStruct { val = 10f };
            return arg.val;
        }

        public delegate void TestDelegate(float arg);
        public float DelegateTest(TestDelegate arg)
        {
            arg(1.0f);
            float upvalue = 0;
            arg = arg2 => { upvalue = arg2; };
            arg(2.0f);
            Func<float, float> asd = arg2 => arg2;
            return upvalue;
        }
        RefClass refed;
    }
}
