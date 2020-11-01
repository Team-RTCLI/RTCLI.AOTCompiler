# RTCLI.AOTCompiler
用于将MSIL编译到C++/MicroCode的AOT编译器。

旨在生成易于扩展实现的富模板代码。如：

``` c++
    //[2-1] Here Begins Stack Declaration
    struct RTCLI_RTCLITestCase_TestCaseClass_Method__Stack
    {
        RTCLI::RTCLITestCase::Reference::RefClass v0;
        RTCLI::System::ConstArray<RTCLI::System::String> v1;
        RTCLI::System::Array v2;
        RTCLI::System::String v3;
        RTCLI::System::ConstArray<RTCLI::System::ConstArray<RTCLI::System::String>> v4;
        template<bool InitLocals> static void Init();//Active with MethodBody.InitLocals Property.
        template<typename T, int index> void Store(RTCLI::StackFwd<T> toStore); //Store to Stack.
        template<typename T, int index> RTCLI::StackFwd<T> Load(void); //Load from Stack.
    };
    
    //[2-2] Here Begins Method Body
    RTCLI::System::Void RTCLI::RTCLITestCase::TestCaseClass::Method(void)
    {
        RTCLI_RTCLITestCase_TestCaseClass_Method__Stack stack =
            RTCLI_RTCLITestCase_TestCaseClass_Method__Stack::Init<true/*InitLocals*/>();
        //...
        stack.v0.Store<RTCLI::decay_t<decltype(s1)>, 1>(s1);
    }
```

## IL-Converters

已经支持的IL(CXX后端)：

- nop
- ldarg.0/1/2/s
- ldarga
- stfld
- stloc.0/1/2/s
- ldloc.0/1/2/s
- Add/Sub 
- newobj
- ldstr
- ret