#nullable enable
using System.Reflection;
using Il2CppInterop.Runtime.InteropTypes;
using NextShip.Api.Managers;
using UnityEngine;
using Il2cppFile = Il2CppSystem.IO.File;

namespace NextShip.Api.Utilities;

public sealed class AssetLoader
{
    private AssetBundle? Asset;
    public bool loaded;

    public AssetLoader(string fileName = "", string loaderName = "")
    {
        FileName = fileName;
        LoaderName = loaderName;
        if (fileName != "" || loaderName != "") Add();
    }

    public string LoaderName { get; }
    public string FileName { get; private set; }

    public AssetLoader LoadFile(string fileName)
    {
        FileName = fileName;
        return this;
    }

    public T? Load<T>(string name) where T : Il2CppObjectBase
    {
        return Asset.LoadAsset<T>(name);
    }

    public List<T> LoadAll<T>() where T : Il2CppObjectBase
    {
        return Asset!.LoadAllAsset<T>().ToList();
    }

    public AssetLoader LoadFromDisk()
    {
        if (FileName is "" or null) return this;

        var directory = GetDir("Assets");
        var bytes = Il2cppFile.ReadAllBytes(Path.Combine(directory.FullName, FileName));
        Asset = AssetBundle.LoadFromMemory(bytes);
        loaded = true;
        AssetManager.Get().Add(Asset);
        return this;
    }

    public AssetLoader LoadFromResources(Assembly assembly)
    {
        if (FileName is "" or null) return this;

        var name = assembly.GetManifestResourceNames().FirstOrDefault(n => n.Contains($"Assets.{FileName}"));
        if (name == null) return this;
        var Stream = assembly.GetManifestResourceStream(name);
        Asset = AssetBundle.LoadFromMemory(Stream.ReadFully());
        loaded = true;
        AssetManager.Get().Add(Asset);
        return this;
    }

    private void Add()
    {
        AssetManager.Get().Add(this);
    }

    public void Remove()
    {
        AssetManager.Get().Remove(this);
    }
}