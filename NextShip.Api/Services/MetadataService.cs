using Microsoft.Extensions.Logging;

namespace NextShip.Api.Services;

public class MetadataService(
    DownloadService _downloadService,
    ILogger<MetadataService> _logger,
    GithubAnalyzer analyzer)
{
    private string Url;

    public void SetRepo(string url)
    {
        Url = url;
    }

    public void GetMetadataInfo()
    {
    }
}