namespace NextShip.Api.Utils;

public static class DirectoryUtils
{
    public static string GetDirectory(this string path)
    {
        if (Directory.Exists(path))
            return path;

        Directory.CreateDirectory(path!);

        return path;
    }
}