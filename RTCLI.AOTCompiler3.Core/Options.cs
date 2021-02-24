using System;

namespace RTCLI.AOTCompiler3
{
    public enum DebugInformationOptions
    {
        None,
        CommentOnly,
        Full
    }

    public enum TargetPlatforms
    {
        NativeCpp,
        InterpreterCode,
        Generic = NativeCpp
    }
}
