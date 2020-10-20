namespace RTCLI.AOTCompiler
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
