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
        public TestCaseClass(string str)
        {

        }
        public static TestCaseClass operator +(TestCaseClass b, TestCaseClass c)
        {
            return b;
        }
        public void MethodWithArgsAccess(int argInt, string argStr, RefClass argClass, RefStruct argStruct)
        {
            var argI = argInt;
            var argSt = argStr;
            var argS = argStruct;
            var argC = argClass;
        }
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
        RefClass refed;
    }
}
