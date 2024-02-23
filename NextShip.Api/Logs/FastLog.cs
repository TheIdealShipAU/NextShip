#nullable enable
using BepInEx.Logging;

namespace NextShip.Api.Logs;

public static class FastLog
{
    /*
    各消息作用:

    发生了致命错误，无法从中恢复 : A fatal error has occurred, which cannot be recovered from
    Fatal

    发生错误，但可以从中恢复 : An error has occured, but can be recovered from
    Error

    已发出警告，但并不一定意味着发生了错误 : A warning has been produced, but does not necessarily mean that something wrong has happened
    Warning

    应向用户显示的重要消息 : An important message that should be displayed to the user
    Message

    重要性较低的消息 :  A message of low importance
    Info

    可能只有开发人员感兴趣的消息 : A message that would likely only interest a developer
    Debug,
    */

    public static void Info(string text, string? tag = null, string? filename = null)
    {
        Log.Instance?.SendToFile(tag, filename, text);
    }

    public static void Warn(string text, string? tag = null, string? filename = null)
    {
        Log.Instance?.SendToFile(tag, filename, text, LogLevel.Warning);
    }

    public static void Error(string text, string? tag = null, string? filename = null)
    {
        Log.Instance?.SendToFile(tag, filename, text, LogLevel.Error);
    }

    public static void Fatal(string text, string? tag = null, string? filename = null)
    {
        Log.Instance?.SendToFile(tag, filename, text, LogLevel.Fatal);
    }

    public static void Msg(string text, string? tag = null, string? filename = null)
    {
        Log.Instance?.SendToFile(tag, filename, text, LogLevel.Message);
    }

    public static void Exception(Exception ex, string? tag = null, string? filename = null)
    {
        Log.Instance?.SendToFile(tag, filename, ex.ToString(), LogLevel.Error);
    }

    public static void Debug(string text, string? tag = null, string? filename = null)
    {
        Log.Instance?.SendToFile(tag, filename, text, LogLevel.Debug);
    }
}