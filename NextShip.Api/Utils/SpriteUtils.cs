#nullable enable
using System.Reflection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace NextShip.Api.Utils;

public static class SpriteUtils
{
    public static Dictionary<string, Sprite> CachedSprites = new();
    public static HashSet<Sprite> CacheSprite = new();

    public static void CaChe(this Sprite sprite, string name = "")
    {
        if (name != "") sprite.name = name;

        sprite.DontDestroyAndUnload();
        CacheSprite.Add(sprite);
    }

    public static Sprite? GetCache(string name, bool NoCache = false)
    {
        var sprite = CacheSprite.FirstOrDefault(n => n.name == name);
        if (sprite == null || sprite == default)
            sprite = CachedSprites[name];
        if (sprite == null || sprite == default) return null;
        if (!NoCache) return sprite;

        sprite.hideFlags |= HideFlags.None;
        CacheSprite.Remove(sprite);

        return sprite;
    }

    public static Sprite? LoadSpriteFromResources(string path, float pixelsPerUnit)
    {
        try
        {
            if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;
            var texture = LoadTextureFromResources(path);
            sprite = Sprite.Create(texture, new Rect(0, 0, texture!.width, texture.height), new Vector2(0.5f, 0.5f),
                pixelsPerUnit);
            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            return CachedSprites[path + pixelsPerUnit] = sprite;
        }
        catch
        {
            Warn("加载图片失败路径:" + path, filename: "Helpers");
        }

        return null;
    }

    public static Sprite? LoadSpriteFromResources(string path, float pixelsPerUnit, Vector2 pivot, uint extrude,
        SpriteMeshType meshType, Vector4 border)
    {
        try
        {
            if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;
            var texture = LoadTextureFromResources(path);
            sprite = Sprite.Create(texture, new Rect(0, 0, texture!.width, texture.height), pivot,
                pixelsPerUnit, extrude, meshType, border);
            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            return CachedSprites[path + pixelsPerUnit] = sprite;
        }
        catch
        {
            Warn("加载图片失败路径:" + path, filename: "Helpers");
        }

        return null;
    }

    public static Sprite ToFullRect(this Sprite sprite, string name = "")
    {
        var FullRectSprite = Sprite.Create(sprite.texture, sprite.textureRect, new Vector2(0.5f, 0.5f),
            sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect, sprite.border);
        if (name != "") FullRectSprite.name = name;
        return FullRectSprite;
    }

    public static unsafe Texture2D? LoadTextureFromResources(string path)
    {
        try
        {
            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(path);
            if (stream == null) return texture;
            var length = stream.Length;
            var byteTexture = new Il2CppStructArray<byte>(length);
            var _ = stream.Read(byteTexture.ToSpan());
            ImageConversion.LoadImage(texture, byteTexture, false);

            return texture;
        }
        catch
        {
            Warn("加载图片失败路径:" + path, filename: "Helpers");
        }

        return null;
    }

    public static Texture2D LoadTextureFromByte(Il2CppStructArray<byte> bytes)
    {
        var texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
        var byteTexture = bytes;
        ImageConversion.LoadImage(texture, byteTexture, false);
        return texture;
    }

    public static Texture2D? LoadTextureFromDisk(string path)
    {
        try
        {
            if (File.Exists(path))
                return LoadTextureFromByte(Il2CppSystem.IO.File.ReadAllBytes(path));
        }
        catch
        {
            Warn("加载图片失败路径:" + path, filename: "Helpers");
        }

        return null;
    }

    public static Sprite? LoadSpriteFromDisk(string path, float pixelsPerUnit)
    {
        try
        {
            if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;
            var texture = LoadTextureFromDisk(path);
            sprite = Sprite.Create(texture, new Rect(0, 0, texture!.width, texture.height), new Vector2(0.5f, 0.5f),
                pixelsPerUnit);
            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            return CachedSprites[path + pixelsPerUnit] = sprite;
        }
        catch
        {
            Warn("加载图片失败路径:" + path, filename: "Helpers");
        }

        return null;
    }

    public static Sprite? LoadSpriteFromResources(string FileName)
    {
        var TheAssembly = Assembly.GetExecutingAssembly();
        var names = TheAssembly.GetManifestResourceNames();
        var name = names.FirstOrDefault(isFile);
        if (name is null or "") return null;
        var TextureByte = TheAssembly.GetManifestResourceStream(name).ReadFully();
        var texture = LoadTextureFromByte(TextureByte);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f),
            100f);

        bool isFile(string _name)
        {
            if (_name.EndsWith(FileName)) return true;

            var strings = _name.Split(".");
            var str = names[strings.Length - 2];
            var n = _name.Replace(".png", "");
            return n == str;
        }
    }
}