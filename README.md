# RTCLI.AOTCompiler
用于将MSIL编译到C++/MacroCode的AOT编译器。

## 如何编译与测试：

### 生成代码：
- 打开sln，以RTCLI.AOTCompiler为启动项目;
- usage: RTCLI.AOTCompiler.exe <output_path> <assembly_paths>;
- 内置了一个TestCase项目进行开发前期代码生成测试，在调试选项中为RTCLI.AOTCompiler附加调试参数$(SolutionDir)RTCLI.Generated $(SolutionDir)RTCLI.TestCase/bin/Debug/netstandardX.X/RTCLI.TestCase.dll;
- 代码会被生成在./RTCLI.CXXTest文件夹下.

### 安装运行时：
- 确保您的环境具有CMake(Version >= 3.19.0);
- 运行时仓库：[RTCLI.Runtime](https://github.com/Team-RTCLI/RTCLI.Runtime);
- 打开运行时仓库，运行./gen_${YourIDE}.bat，此时Solution会在/build下被生成;
- 运行./install.bat进行安装，需要管理员权限.

### 运行生成代码
- 打开RTCLI.CXXTest文件夹，运行./gen_${YourIDE}.bat，此时Solution会在/build下被生成;
- 打开Solution，选择对应的Test项目运行即可。

### 注意：
- 默认使用netstandard2.1的标准meta文件

source：
``` c#
public static void Test()
{
    String nullStr = null;
    String str = "Static String";

    System.Console.WriteLine(str);
    System.Console.WriteLine(str.Length);
    String lowerStr = str.ToLower();
    System.Console.Write("ToLower: ");
    System.Console.WriteLine(lowerStr);
}
```

generated：
``` c++
RTCLI::System::Void RTCLI::TestCase::TestString::Test()
{
	RTCLI::TRef<RTCLI::System::String> v0 = RTCLI::null;
	RTCLI::TRef<RTCLI::System::String> v1 = RTCLI::null;
	RTCLI::TRef<RTCLI::System::String> v2 = RTCLI::null;
	// IL_0000: nop
	IL_0000: RTCLI::nop();
	// IL_0001: ldnull
	IL_0001: auto& s0 = RTCLI::null;
	// IL_0002: stloc.0
	IL_0002: v0 = s0;
	// IL_0003: ldstr \"Static String\"
	IL_0003: RTCLI::System::String s1 = RTCLI_NATIVE_STRING("Static String");
	// IL_0008: stloc.1
	IL_0008: v1 = s1;
	// IL_0009: ldloc.1
	IL_0009: auto& s2 = v1.Get();
	// IL_000a: call System.Void System.Console::WriteLine(System.String)
	IL_000a: RTCLI::System::Console::WriteLine(s2);
	// IL_000f: nop
	IL_000f: RTCLI::nop();
	// IL_0010: ldloc.1
	IL_0010: auto& s3 = v1.Get();
	// IL_0011: callvirt System.Int32 System.String::get_Length()
	IL_0011: auto s4 = ((RTCLI::System::String&)s3).get_Length();
	// IL_0016: call System.Void System.Console::WriteLine(System.Int32)
	IL_0016: RTCLI::System::Console::WriteLine(s4);
	// IL_001b: nop
	IL_001b: RTCLI::nop();
	// IL_001c: ldloc.1
	IL_001c: auto& s5 = v1.Get();
	// IL_001d: callvirt System.String System.String::ToLower()
	IL_001d: auto s6 = ((RTCLI::System::String&)s5).ToLower();
	// IL_0022: stloc.2
	IL_0022: v2 = s6;
	// IL_0023: ldstr \"ToLower: \"
	IL_0023: RTCLI::System::String s7 = RTCLI_NATIVE_STRING("ToLower: ");
	// IL_0028: call System.Void System.Console::Write(System.String)
	IL_0028: RTCLI::System::Console::Write(s7);
	// IL_002d: nop
	IL_002d: RTCLI::nop();
	// IL_002e: ldloc.2
	IL_002e: auto& s8 = v2.Get();
	// IL_002f: call System.Void System.Console::WriteLine(System.String)
	IL_002f: RTCLI::System::Console::WriteLine(s8);
	// IL_0034: nop
	IL_0034: RTCLI::nop();
	// IL_0035: ret
	IL_0035: return ;
}
```

## IL-Converters

| OpCode | Binary | CXXCvt | CXXRuntime | Test |
| ------------------------------------------------------------ | ------ | ------ | ---------- | ---- |
| [add](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.add) | 0x58   | ✅      |  ✅          |      |
| [add.ovf](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.add_ovf) | 0xd6   | ✅      |    ❗        |      |
| [add.ovf.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.add_ovf_un) | 0xd7   | ✅       |    ❗        |      |
| [and](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.and) | 0x5f   | ✅ |            |      |
| [arglist](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.arglist) | 0xfe00 |        |            |      |
| [beq](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.beq) | 0x3b   | ✅ |            |      |
| [beq.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.beq_s) | 0x2e   | ✅ |            |      |
| [bge](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bge) | 0x3c   | ✅ |            |      |
| [bge.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bge_s) | 0x2f   | ✅ |            |      |
| [bge.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bge_un) | 0x41   | ✅ |            |      |
| [bge.un.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bge_un_s) | 0x34   | ✅ |            |      |
| [bgt](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bgt) | 0x3d   | ✅ |            |      |
| [bgt.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bgt_s) | 0x30   | ✅ |            |      |
| [bgt.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bgt_un) | 0x42   | ✅ |            |      |
| [bgt.un.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bgt_un_s) | 0x35   | ✅ |            |      |
| [ble](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ble) | 0x3e   | ✅ |            |      |
| [ble.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ble_s) | 0x31   | ✅ |            |      |
| [ble.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ble_un) | 0x43   | ✅ |            |      |
| [ble.un.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ble_un_s) | 0x36   | ✅ |            |      |
| [blt](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.blt) | 0x3f   | ✅ |            |      |
| [blt.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.blt_s) | 0x32   | ✅ |            |      |
| [blt.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.blt_un) | 0x44   | ✅ |            |      |
| [blt.un.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.blt_un_s) | 0x37   | ✅ |            |      |
| [bne.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bne_un) | 0x40   | ✅ |            |      |
| [bne.un.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bne_un_s) | 0x33   | ✅ |            |      |
| [box](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.box) | 0x8c   |        |            |      |
| [br](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.br) | 0x38   |  ✅       |            |  🔷    |
| [br.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.br_s) | 0x2b   | ✅    |            |   🔷   |
| [break](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.break) | 0x1    |        |            |      |
| [brfalse](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.brfalse) | 0x39   | ✅     |   🔷         |      |
| [brfalse.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.brfalse_s) | 0x2c   | ✅     |    🔷        |      |
| [brtrue](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.brtrue) | 0x3a   | ✅      |     🔷       |      |
| [brtrue.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.brtrue_s) | 0x2d   | ✅     |    🔷        |      |
| [call](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.call) | 0x28   | ✅       |    🔷        |      |
| [calli](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.calli) | 0x29   |      |            |      |
| [callvirt](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.callvirt) | 0x6f   | ✅      |   🔷         |      |
| [castclass](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.castclass) | 0x74   | ✅ |            |      |
| [ceq](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ceq) | 0xfe01 | ✅ |            |      |
| [cgt](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.cgt) | 0xfe02 | ✅ |  ✅  |      |
| [cgt.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.cgt_un) | 0xfe03 | ✅ |  ✅       |      |
| [ckfinite](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ckfinite) | 0xc3   | ✅ |            |      |
| [clt](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.clt) | 0xfe04 | ✅ |            |      |
| [clt.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.clt_un) | 0xfe05 | ✅ |            |      |
| [constrained](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.constrained) | 0xfe16 |        |            |      |
| [conv.i](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_i) | 0xd3   |        |            |      |
| [conv.i1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_i1) | 0x67   |        |            |      |
| [conv.i2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_i2) | 0x68   |        |            |      |
| [conv.i4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_i4) | 0x69   |        |            |      |
| [conv.i8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_i8) | 0x6a   |        |            |      |
| [conv.ovf.i](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_i) | 0xd4   |        |            |      |
| [conv.ovf.i.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_i_un) | 0x8a   |        |            |      |
| [conv.ovf.i1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_i1) | 0xb3   |        |            |      |
| [conv.ovf.i1.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_i1_un) | 0x82   |        |            |      |
| [conv.ovf.i2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_i2) | 0xb5   |        |            |      |
| [conv.ovf.i2.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_i2_un) | 0x83   |        |            |      |
| [conv.ovf.i4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_i4) | 0xb7   |        |            |      |
| [conv.ovf.i4.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_i4_un) | 0x84   |        |            |      |
| [conv.ovf.i8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_i8) | 0xb9   |        |            |      |
| [conv.ovf.i8.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_i8_un) | 0x85   |        |            |      |
| [conv.ovf.u](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u) | 0xd5   |        |            |      |
| [conv.ovf.u.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u_un) | 0x8b   |        |            |      |
| [conv.ovf.u1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u1) | 0xb4   |        |            |      |
| [conv.ovf.u1.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u1_un) | 0x86   |        |            |      |
| [conv.ovf.u2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u2) | 0xb6   |        |            |      |
| [conv.ovf.u2.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u2_un) | 0x87   |        |            |      |
| [conv.ovf.u4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u4) | 0xb8   |        |            |      |
| [conv.ovf.u4.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u4_un) | 0x88   |        |            |      |
| [conv.ovf.u8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u8) | 0xba   |        |            |      |
| [conv.ovf.u8.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u8_un) | 0x89   |        |            |      |
| [conv.r.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_r_un) | 0x76   |        |            |      |
| [conv.r4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_r4) | 0x6b   |        |            |      |
| [conv.r8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_r8) | 0x6c   |        |            |      |
| [conv.u](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_u) | 0xe0   |        |            |      |
| [conv.u1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_u1) | 0xd2   |        |            |      |
| [conv.u2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_u2) | 0xd1   |        |            |      |
| [conv.u4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_u4) | 0x6d   |        |            |      |
| [conv.u8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_u8) | 0x6e   |        |            |      |
| [cpblk](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.cpblk) | 0xfe17 | ✅ |            |      |
| [cpobj](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.cpobj) | 0x70   | ✅ |            |      |
| [div](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.div) | 0x5b   | ✅ |            |      |
| [div.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.div_un) | 0x5c   | ✅ |            |      |
| [dup](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.dup) | 0x25   | ✅ |            |      |
| [endfilter](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.endfilter) | 0xfe11 |        |            |      |
| [endfinally](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.endfinally) | 0xdc   |        |            |      |
| [initblk](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.initblk) | 0xfe18 | ✅ |            |      |
| [initobj](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.initobj) | 0xfe15 | ✅ |            |      |
| [isinst](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.isinst) | 0x75   | ✅ |            |      |
| [jmp](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.jmp) | 0x27   |        |  🔷          |      |
| [ldarg](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg) | 0xfe09 | ✅      |  🔷          |      |
| [ldarg.0](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg_0) | 0x2    | ✅      |🔷            |      |
| [ldarg.1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg_1) | 0x3    | ✅      |  🔷          |      |
| [ldarg.2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg_2) | 0x4    | ✅      |  🔷          |      |
| [ldarg.3](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg_3) | 0x5    | ✅      |  🔷          |      |
| [ldarg.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg_s) | 0xe    | ✅      |   🔷         |      |
| [ldarga](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarga) | 0xfe0a | ✅      |   🔷         |      |
| [ldarga.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarga_s) | 0xf    | ✅      |   🔷         |      |
| [ldc.i4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4) | 0x20   | ✅      |            |      |
| [ldc.i4.0](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_0) | 0x16   | ✅      |            |      |
| [ldc.i4.1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_1) | 0x17   | ✅      |            |      |
| [ldc.i4.2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_2) | 0x18   | ✅      |            |      |
| [ldc.i4.3](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_3) | 0x19   | ✅      |            |      |
| [ldc.i4.4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_4) | 0x1a   | ✅      |            |      |
| [ldc.i4.5](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_5) | 0x1b   | ✅      |            |      |
| [ldc.i4.6](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_6) | 0x1c   | ✅      |            |      |
| [ldc.i4.7](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_7) | 0x1d   | ✅      |            |      |
| [ldc.i4.8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_8) | 0x1e   | ✅      |            |      |
| [ldc.i4.m1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_m1) | 0x15   | ✅      |            |      |
| [ldc.i4.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_s) | 0x1f   | ✅      |            |      |
| [ldc.i8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i8) | 0x21   | ✅      |            |      |
| [ldc.r4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_r4) | 0x22   | ✅      |            |      |
| [ldc.r8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_r8) | 0x23   | ✅      |            |      |
| [ldelem.any](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem) | 0xa3   | ✅ |            |      |
| [ldelem.i](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_i) | 0x97   | ✅ |            |      |
| [ldelem.i1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_i1) | 0x90   | ✅ |            |      |
| [ldelem.i2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_i2) | 0x92   | ✅ |            |      |
| [ldelem.i4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_i4) | 0x94   | ✅ |            |      |
| [ldelem.i8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_i8) | 0x96   | ✅ |            |      |
| [ldelem.r4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_r4) | 0x98   | ✅ |            |      |
| [ldelem.r8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_r8) | 0x99   | ✅ |            |      |
| [ldelem.ref](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_ref) | 0x9a   | ✅ |            |      |
| [ldelem.u1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_u1) | 0x91   | ✅ |            |      |
| [ldelem.u2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_u2) | 0x93   | ✅ |            |      |
| [ldelem.u4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_u4) | 0x95   | ✅ |            |      |
| [ldelema](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelema) | 0x8f   | ✅ |            |      |
| [ldfld](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldfld) | 0x7b   | ✅ |            |      |
| [ldflda](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldflda) | 0x7c   | ✅ |            |      |
| [ldftn](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldftn) | 0xfe06 | ✅ |            |      |
| [ldind.i](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_i) | 0x4d   | ✅ |            |      |
| [ldind.i1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_i1) | 0x46   | ✅ |            |      |
| [ldind.i2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_i2) | 0x48   | ✅ |            |      |
| [ldind.i4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_i4) | 0x4a   | ✅ |            |      |
| [ldind.i8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_i8) | 0x4c   | ✅ |            |      |
| [ldind.r4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_r4) | 0x4e   | ✅ |            |      |
| [ldind.r8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_r8) | 0x4f   | ✅ |            |      |
| [ldind.ref](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_ref) | 0x50   | ✅ |            |      |
| [ldind.u1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_u1) | 0x47   | ✅ |            |      |
| [ldind.u2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_u2) | 0x49   | ✅ |            |      |
| [ldind.u4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_u4) | 0x4b   | ✅ |            |      |
| [ldlen](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldlen) | 0x8e   | ✅ |            |      |
| [ldloc](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc) | 0xfe0c | ✅      |    🔷        |      |
| [ldloc.0](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc_0) | 0x6    | ✅      |     🔷      |      |
| [ldloc.1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc_1) | 0x7    | ✅      |   🔷         |      |
| [ldloc.2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc_2) | 0x8    | ✅      |   🔷         |      |
| [ldloc.3](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc_3) | 0x9    | ✅      |  🔷          |      |
| [ldloc.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc_s) | 0x11   | ✅      |  🔷          |      |
| [ldloca](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloca) | 0xfe0d | ✅ | 🔷           |      |
| [ldloca.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloca_s) | 0x12   | ✅ |🔷            |      |
| [ldnull](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldnull) | 0x14   | ✅     |            |      |
| [ldobj](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldobj) | 0x71   | ✅ |            |      |
| [ldsfld](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldsfld) | 0x7e   | ✅ |            |      |
| [ldsflda](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldsflda) | 0x7f   | ✅ |            |      |
| [ldstr](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldstr) | 0x72   | ✅      |            |      |
| [ldtoken](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldtoken) | 0xd0   |        |            |      |
| [ldvirtftn](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldvirtftn) | 0xfe07 |        |            |      |
| [leave](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.leave) | 0xdd   | ✅       |            |      |
| [leave.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.leave_s) | 0xde   | ✅      |            |      |
| [localloc](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.localloc) | 0xfe0f |        |            |      |
| [mkrefany](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.mkrefany) | 0xc6   |        |            |      |
| [mul](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.mul) | 0x5a   | ✅ |            |      |
| [mul.ovf](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.mul_ovf) | 0xd8   | ✅ |            |      |
| [mul.ovf.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.mul_ovf_un) | 0xd9   | ✅ |            |      |
| [neg](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.neg) | 0x65   | ✅ |            |      |
| [newarr](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.newarr) | 0x8d   | ✅ |            |      |
| [newobj](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.newobj) | 0x73   | ✅      |    ❗        |      |
| no                                                           | 0xfe19 |        |            |      |
| [nop](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.nop) | 0x0    | ✅  |  ✅        |      |
| [not](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.not) | 0x66   | ✅ |            |      |
| [or](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.or) | 0x60   | ✅ |            |      |
| [pop](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.pop) | 0x26   | ✅      |            |      |
| [readonly](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.readonly) | 0xfe1e |        |            |      |
| [refanytype](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.refanytype) | 0xfe1d |        |            |      |
| [refanyval](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.refanyval) | 0xc2   |        |            |      |
| [rem](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.rem) | 0x5d   | ✅ |            |      |
| [rem.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.rem_un) | 0x5e   | ✅ |            |      |
| [ret](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ret) | 0x2a   | ✅      |  🔷      |      |
| [rethrow](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.rethrow) | 0xfe1a |        |            |      |
| [shl](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.shl) | 0x62   | ✅ |            |      |
| [shr](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.shr) | 0x63   | ✅ |            |      |
| [shr.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.shr_un) | 0x64   | ✅ |            |      |
| [sizeof](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.sizeof) | 0xfe1c | ✅ |            |      |
| [starg](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.starg) | 0xfe0b | ✅ |  🔷          |      |
| [starg.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.starg_s) | 0x10   | ✅ |    🔷        |      |
| [stelem.any](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem) | 0xa4   | ✅ |            |      |
| [stelem.i](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem_i) | 0x9b   | ✅ |            |      |
| [stelem.i1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem_i1) | 0x9c   | ✅ |            |      |
| [stelem.i2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem_i2) | 0x9d   | ✅ |            |      |
| [stelem.i4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem_i4) | 0x9e   | ✅ |            |      |
| [stelem.i8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem_i8) | 0x9f   | ✅ |            |      |
| [stelem.r4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem_r4) | 0xa0   | ✅ |            |      |
| [stelem.r8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem_r8) | 0xa1   | ✅ |            |      |
| [stelem.ref](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem_ref) | 0xa2   | ✅ |            |      |
| [stfld](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stfld) | 0x7d   | ✅      |   🔷         |      |
| [stind.i](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stind_i) | 0xdf   | ✅ |            |      |
| [stind.i1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stind_i1) | 0x52   | ✅ |            |      |
| [stind.i2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stind_i2) | 0x53   | ✅ |            |      |
| [stind.i4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stind_i4) | 0x54   | ✅ |            |      |
| [stind.i8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stind_i8) | 0x55   | ✅ |            |      |
| [stind.r4](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stind_r4) | 0x56   | ✅ |            |      |
| [stind.r8](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stind_r8) | 0x57   | ✅ |            |      |
| [stind.ref](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stind_ref) | 0x51   | ✅ |            |      |
| [stloc](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stloc) | 0xfe0e | ✅      |            |      |
| [stloc.0](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stloc_0) | 0xa    | ✅      |            |      |
| [stloc.1](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stloc_1) | 0xb    | ✅      |            |      |
| [stloc.2](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stloc_2) | 0xc    | ✅      |            |      |
| [stloc.3](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stloc_3) | 0xd    | ✅      |            |      |
| [stloc.s](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stloc_s) | 0x13   | ✅      |            |      |
| [stobj](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stobj) | 0x81   | ✅ |            |      |
| [stsfld](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stsfld) | 0x80   | ✅ |   🔷         |      |
| [sub](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.sub) | 0x59   | ✅      |            |      |
| [sub.ovf](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.sub_ovf) | 0xda   | ✅      |            |      |
| [sub.ovf.un](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.sub_ovf_un) | 0xdb   | ✅     |            |      |
| [switch](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.switch) | 0x45   |        |            |      |
| [tail](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.tailcall) | 0xfe14 | ✅ |            |      |
| [throw](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.throw) | 0x7a   |        |            |      |
| [unaligned](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.unaligned) | 0xfe12 |        |            |      |
| [unbox](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.unbox) | 0x79   |        |            |      |
| [unbox.any](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.unbox_any) | 0xa5   |        |            |      |
| [volatile](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.volatile) | 0xfe13 |        |            |      |
| [xor](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.xor) | 0x61   | ✅ |            |      |