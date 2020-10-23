# RTCLI.AOTCompiler
用于将MSIL编译到C++/MicroCode的AOT编译器。

旨在生成易于扩展实现的富模板代码。如：

```cpp
    RTCLI_RTCLITestCase_TestCaseClass_Method__Stack stack =
		RTCLI_RTCLITestCase_TestCaseClass_Method__Stack::Init<true/*InitLocals*/>();
    //...
    stack.v0.Store<RTCLI::decay_t<decltype(s1)>, 1>(s1);
```

## IL-Converters

已经支持的IL(CXX后端)：

- nop
- newobj
- ldarg.0
- ldstr
- stloc.0 