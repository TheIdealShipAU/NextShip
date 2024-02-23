namespace NSLangAnalyzer;

internal class FileFinder
{
    private static string[] Extensions = [FileSuffix, FileSuffix2, FileSuffix3 , FileSuffix4];
    private readonly List<FileInfo> _fileInfos = [];
    private readonly List<ReadInfo> _readInfos = [];

    public FileFinder ReadFiles()
    {
        if (_fileInfos.Count == 0)
        {
            throw new NSLException("FileInfo = 0");
        }

        foreach (var info in _fileInfos)
        {
            using var readStream = info.OpenRead();
            using var reader = new StreamReader(readStream);
            var strings = string.Empty;

            var OneLine = reader.ReadLine();
            if (OneLine == null || !OneLine.StartsWith('[') || !OneLine.EndsWith(']'))
            {
                continue;
            }

            var texts = OneLine
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Split(":");

            var name = texts[0];
            var author = texts[1];
            
            var line = reader.ReadLine();
            while (line != null)
            {
                strings += line;
                line = reader.ReadLine();
            }
            
            _readInfos.Add(new ReadInfo
            {
                Info = info,
                ReadStrings = strings,
                name = name,
                author = author
            });
        }
        
        return this;
    }
    
    public FileFinder Get(DirectoryInfo directory)
    {
        _readInfos.Clear();
        _fileInfos.AddRange(directory.GetFiles().Where(isFile));
        return this;
    }

    private static bool isFile(FileInfo fileInfo)
    {
        return Extensions.Any(varExtension => fileInfo.Extension == "." + varExtension);
    }

    private class ReadInfo
    {
        public string? ReadStrings { get; set; }

        public FileInfo? Info { get; set; }
        
        public string? name { get; set; }
        
        public string? author { get; set; }
    }
}