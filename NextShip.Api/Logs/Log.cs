using System.Text;
using BepInEx.Logging;

namespace NextShip.Api.Logs;

public abstract class log
{
    public readonly ManualLogSource LogSource;

    static log()
    {
        ConsoleTextFC();
    }

    public log(ManualLogSource logSource)
    {
        LogSource = logSource;
        Instance = this;
    }

    public static log? Instance { get; set; }

    public static void ConsoleTextFC()
    {
        if (!System.Console.OutputEncoding.Equals(Encoding.UTF8)) System.Console.OutputEncoding = Encoding.UTF8;
    }

    private void SendToFile(string? tag, string? filename, string text, LogLevel level = LogLevel.Info)
    {
        var logger = Instance?.LogSource;
        var t = DateTime.Now.ToString("HH:mm:ss");
        var log_text = $"[{t}]";
        if (tag != null) log_text += $"[{tag}]";
        if (filename != null) log_text += $"[{filename}]";
        log_text += text;
        switch (level)
        {
            case LogLevel.Info:
                logger?.LogInfo(log_text);
                break;
            case LogLevel.Warning:
                logger?.LogWarning(log_text);
                break;
            case LogLevel.Error:
                logger?.LogError(log_text);
                break;
            case LogLevel.Fatal:
                logger?.LogFatal(log_text);
                break;
            case LogLevel.Message:
                logger?.LogMessage(log_text);
                break;
            case LogLevel.Debug:
                logger?.LogDebug(log_text);
                break;
            case LogLevel.None:
                logger?.LogInfo(log_text);
                break;
            case LogLevel.All:
                break;
            default:
                logger?.LogWarning("Error:Invalid LogLevel");
                logger?.LogInfo(log_text);
                break;
        }
    }

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
        Instance?.SendToFile(tag, filename, text);
    }

    public static void Warn(string text, string? tag = null, string? filename = null)
    {
        Instance?.SendToFile(tag, filename, text, LogLevel.Warning);
    }

    public static void Error(string text, string? tag = null, string? filename = null)
    {
        Instance?.SendToFile(tag, filename, text, LogLevel.Error);
    }

    public static void Fatal(string text, string? tag = null, string? filename = null)
    {
        Instance?.SendToFile(tag, filename, text, LogLevel.Fatal);
    }

    public static void Msg(string text, string? tag = null, string? filename = null)
    {
        Instance?.SendToFile(tag, filename, text, LogLevel.Message);
    }

    public static void Exception(Exception ex, string? tag = null, string? filename = null)
    {
        Instance?.SendToFile(tag, filename, ex.ToString(), LogLevel.Error);
    }

    public static void Debug(string text, string? tag = null, string? filename = null)
    {
        Instance?.SendToFile(tag, filename, text, LogLevel.Debug);
    }
}