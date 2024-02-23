#nullable enable
using UnityEngine;

namespace NextShip.Api.Managers;

public class AssetManager
{
    public static AssetManager? Instance;
    private readonly List<AssetBundle?> _assetBundles = new();

    private readonly List<AssetLoader?> _assetLoaders = new();

    public static AssetManager Get()
    {
        return Instance ??= new AssetManager();
    }

    public IReadOnlyList<AssetLoader?> GetLoaders()
    {
        return _assetLoaders;
    }

    public IReadOnlyList<AssetBundle?> GetAssetS()
    {
        return _assetBundles;
    }

    public void Add(AssetLoader? assetLoader)
    {
        _assetLoaders.Add(assetLoader);
    }

    public void Add(AssetBundle? assetBundle)
    {
        _assetBundles.Add(assetBundle);
    }

    public void Remove(AssetLoader? assetLoader)
    {
        _assetLoaders.Remove(assetLoader);
    }

    public AssetLoader? GetLoader(string name)
    {
        return _assetLoaders.Find(n => name == n?.FileName || name == n?.LoaderName);
    }
}