using Microsoft.Extensions.Logging;

namespace NextShip.Api.Services;

public class DownloadService(ILogger<DownloadService> logger, HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<DownloadService> _logger = logger;

    public Task Download(Uri uri)
    {
        return Task.CompletedTask;
    }

    public Task Download(string url)
    {
        return Download(new Uri(url));
    }
}