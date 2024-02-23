#nullable enable
namespace NextShip.Api.Utilities;

public class GithubAnalyzer(string RepoUrl)
{
    public GithubData GetData()
    {
        return new GithubData();
    }
}

public class GithubData
{
    public T? Get<T>(string key, T? def) where T : class
    {
        return null;
    }
}