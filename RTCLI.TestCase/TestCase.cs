using System;

using RTCLITestCase.Reference;

namespace RTCLITestCase
{
    public class TestCaseClass
    {
        void Method()
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
        }
        void MethodWithArgsAccess(int arg0, float arg1, char arg2, byte arg3, string arg4, RefClass arg5, RefStruct arg666)
        {
            var arg00 = arg0;
            var arg66 = arg666;
            var arg222 = arg2;
            var arg11 = arg1;
            var arg55 = arg5;
        }
        void MethodWithArgsOps(int arg0, int arg1)
        {
            var arg = arg0 + arg1;
            arg -= arg0;
        }
        void MethodWithArgsFieldAccess(int arg0, float arg1, char arg2, byte arg3, string arg4, RefClass arg5, RefStruct arg666)
        {
            //var arg6Name = arg666.Name;
            arg666.Name = " Accessed";
        }
        void MethodWithArgsVirtCall(int arg0, float arg1, char arg2, byte arg3, string arg4, RefClass arg5, RefStruct arg666)
        {
            var arg55 = arg5;
            var arg5Name = arg5.Name;
        }
        RefClass refed;
    }
}
