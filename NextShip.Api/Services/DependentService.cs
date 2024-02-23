using System.Reflection;

namespace NextShip.Api.Services;

public class DependentService(HttpClient _client)
{
    public readonly HashSet<(Assembly, FileInfo)> Dlls = [];

    public readonly Queue<(Uri, string)> DownloadDependents = new();

    public readonly Queue<(Stream, string)> GenerateDependents = new();

    public readonly Queue<string> RepoDownloadDependents = new();

    private DirectoryInfo Directory;

    private string RepoURL;

    private string RootPath;

    public void SetPath(DirectoryInfo directoryInfo)
    {
        RootPath = directoryInfo.FullName;
        Directory = directoryInfo;
    }

    public void SetRepoURL(string uri)
    {
        RepoURL = uri;
    }

    public void BuildDependent()
    {
        while (RepoDownloadDependents.Count > 0)
        {
            var name = RepoDownloadDependents.Dequeue();
            var url = RepoURL + $"/{name}";
            DownloadDependents.Enqueue((new Uri(url), name));
        }

        while (DownloadDependents.Count > 0)
        {
            var (url, name) = DownloadDependents.Dequeue();
            var stream = _client.GetStreamAsync(url).Result;
            GenerateDependents.Enqueue((stream, name));
        }

        while (GenerateDependents.Count > 0)
        {
            var (stream, name) = GenerateDependents.Dequeue();
            var generateFile = File.Create(RootPath + "/" + name);
            stream.CopyToAsync(generateFile);
        }
    }

    public void LoadDependent()
    {
        var files = Directory.GetFiles();
        foreach (var file in files)
        {
            if (file.Extension == ".dll")
            {
                var assembly = Assembly.LoadFile(file.FullName);
                Dlls.Add((assembly, file));
                continue;
            }

            file.Delete();
        }
    }
}