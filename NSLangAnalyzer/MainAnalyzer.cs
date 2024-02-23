namespace NSLangAnalyzer;

public class MainAnalyzer
{
    private readonly FileFinder _Finder = new();

    public void Find(string path)
    {
        if (!Directory.Exists(path))
            throw new NSLException("不存在指定文件夹");

        var finder = _Finder.Get(new DirectoryInfo(path)).ReadFiles();
    }
}