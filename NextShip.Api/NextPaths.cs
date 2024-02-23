using BepInEx;

namespace NextShip.Api;

public static class NextPaths
{
    public static string RootDirectory;

    static NextPaths()
    {
        GetPaths();
        CreateDirectory();
    }

    public static string TIS_DataPath { get; private set; }

    public static string CreativityPath { get; private set; }

    public static string TIS_TempPath { get; private set; }

    public static string TIS_ConfigPath { get; private set; }

    public static string TIS_PluginsPath { get; private set; }

    public static string TIS_TORHats { get; private set; }

    public static string TIS_Lib { get; private set; }

    private static void GetPaths()
    {
        RootDirectory = Paths.GameRootPath;
        TIS_DataPath = $"{RootDirectory}/TIS_Data";
        CreativityPath = $"{RootDirectory}/Creativity";
        TIS_TempPath = $"{TIS_DataPath}/TEMP";
        TIS_ConfigPath = $"{TIS_DataPath}/Config";
        TIS_PluginsPath = $"{CreativityPath}/Plugins";
        TIS_TORHats = $"{CreativityPath}/TOR";
        TIS_Lib = $"{CreativityPath}/Dependents";
    }


    private static void CreateDirectory()
    {
        var paths = typeof(NextPaths).GetFields().Where(n => n.IsStatic && n.IsPublic && n.FieldType == typeof(string))
            .Select(n => n.GetValue(null)).Select(n => (string)n);

        foreach (var Path in paths)
        {
            if (Directory.Exists(Path)) continue;
            Directory.CreateDirectory(Path!);
        }
    }
}