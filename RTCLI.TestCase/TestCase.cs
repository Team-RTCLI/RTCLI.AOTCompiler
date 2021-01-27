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
        /*void Method()
        {
            var reff = new RefClass("TestCase");
            refed = new RefClass("TestCase");

            string[] TestArray = { "1", "2", "3" };
            Array arr = TestArray;
            var test = (arr as string[])[1];
            string[][] arr_typed = { (string[])arr, (string[])arr };
        }
        float MethodWithConstRet()
        {
            return 5;
        }
        string MethodWithConstStringRet()
        {
            return "RTCLI Test: Const String";
        }*/
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
        RefClass refed;
    }
}
