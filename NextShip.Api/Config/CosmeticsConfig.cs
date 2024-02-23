namespace NextShip.Api.Config;

public class CosmeticsConfig
{
    public CosmeticsConfig(CosmeticRepoType cosmeticRepoType, CosmeticType cosmeticType,
        CosmeticLoadType cosmeticLoadType, string repoName, string repoURL)
    {
        CosmeticRepoType = cosmeticRepoType;
        CosmeticType = cosmeticType;
        CosmeticLoadType = cosmeticLoadType;
        RepoName = repoName;
        RepoURL = repoURL;
    }

    public CosmeticRepoType CosmeticRepoType { get; set; }
    public CosmeticType CosmeticType { get; set; }
    public CosmeticLoadType CosmeticLoadType　{ get; set; }
    public string RepoName　{ get; set; }
    public string RepoURL　{ get; set; }
}