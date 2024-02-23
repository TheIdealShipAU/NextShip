using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using NextShip.Api.Config;
using NextShip.Api.Enums;
using UnityEngine;

namespace NextShip.Cosmetics.Loaders;

public class TORCosmeticsLoader : CosmeticsLoader
{
    private const string HatDirectoryName = "hats";
    private readonly CosmeticType CosmeticType = CosmeticType.Hat;

    public override CosmeticType GetCosmeticType()
    {
        return CosmeticType;
    }

    public override CosmeticRepoType GetCosmeticRepoType()
    {
        return CosmeticRepoType.TOR;
    }

    public override async Task LoadFormOnRepo(CosmeticsConfig cosmeticsConfig)
    {
        Info("Loading from TOR");
        var download = await Download(cosmeticsConfig.RepoURL);
        if (download != HttpStatusCode.OK) return;

        var creator = new CosmeticsCreator(AllSprite);
        AllCosmeticsInfo.Do(n =>
        {
            var hat = creator.CreateHat(n);
            AllHat.Add(hat.Item1);
            CustomCosmeticsManager.AllCustomCosmeticNameAndInfo.Add(n.Name, n);
            CustomCosmeticsManager.AllCosmeticId.Add(n.Id);
            CustomCosmeticsManager.AllCustomHatViewData.Add(n.Id, hat.Item1);
            Hats[n.Name][hat.Item2] = hat.Item1;
        });
    }

    private async Task<HttpStatusCode> Download(string Url)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

        var response = await client.GetAsync($"{Url}/{HatJsonName}", HttpCompletionOption.ResponseContentRead);
        if (response.StatusCode != HttpStatusCode.OK) return response.StatusCode;

        var jsonString = await response.Content.ReadAsStringAsync();
        var JObj = JObject.Parse(jsonString)["hats"];
        var jsonFile = File.CreateText(NextPaths.TIS_TORHats + $"/{HatJsonName}");
        await jsonFile.WriteAsync(jsonString);

        if (!JObj.HasValues) return HttpStatusCode.ExpectationFailed;

        try
        {
            AddCosmeticInfoFromJson(JObj, out var infos);
            AllCosmeticsInfo.AddRange(infos);

            DownLoadSprites(client, Url, infos);
        }
        catch (Exception e)
        {
            Exception(e, $"Load{GetCosmeticType()} {GetCosmeticRepoType()} {Url}");
        }

        return HttpStatusCode.OK;
    }

    private static void AddCosmeticInfoFromJson(JToken JObj, out List<CosmeticsInfo> cosmeticsInfos)
    {
        cosmeticsInfos = [];
        for (var current = JObj.First; current != null; current = current.Next)
        {
            if (!current.HasValues) continue;
            var info = new CosmeticsInfo
            {
                Name = current.GetString("name"),
                Resource = sanitizeResourcePath(current.GetString("resource"))
            };

            // required 
            if (info.Resource == null || info.Name == null) continue;

            info.BackResource = sanitizeResourcePath(current.GetString("backresource"));
            info.ClimbResource = sanitizeResourcePath(current.GetString("climbresource"));
            info.FlipResource = sanitizeResourcePath(current.GetString("flipresource"));
            info.BackFlipResource = sanitizeResourcePath(current.GetString("backflipresource"));

            info.ResHash = current.GetString("reshasha");
            info.ResHashBack = current.GetString("reshashb");
            info.ResHashClimb = current.GetString("reshashc");
            info.ResHashFlip = current.GetString("reshashf");
            info.ResHashBackFlip = current.GetString("reshashbf");

            info.Author = current.GetString("author");
            info.Package = current.GetString("package");
            info.Condition = current.GetString("condition");
            info.Bounce = current["bounce"] != null;
            info.Adaptive = current["adaptive"] != null;
            info.Behind = current["behind"] != null;

            info.Id = "Mod_TOR_Hat_" + info.Name;
            cosmeticsInfos.Add(info);
        }
    }

    private async void DownLoadSprites(HttpClient httpClient, string url, List<CosmeticsInfo> infos)
    {
        var hatStrings = new List<string>();
        var md5 = MD5.Create();

        var directoryPath = $"{NextPaths.TIS_TORHats}/Cache".GetDirectory();

        foreach (var info in infos)
        {
            CheckHash(info.Resource, info.ResHash);

            if (info.BackResource != null)
                CheckHash(info.BackResource, info.ResHashBack);

            if (info.ClimbResource != null)
                CheckHash(info.ClimbResource, info.ResHashClimb);

            if (info.FlipResource != null)
                CheckHash(info.FlipResource, info.ResHashFlip);

            if (info.BackFlipResource != null)
                CheckHash(info.BackFlipResource, info.ResHashBackFlip);
        }

        foreach (var hat in hatStrings)
        {
            var Response = await httpClient.GetAsync($"{url}/{HatDirectoryName}/{hat}",
                HttpCompletionOption.ResponseContentRead);
            if (Response.StatusCode != HttpStatusCode.OK) continue;
            await using var responseStream = await Response.Content.ReadAsStreamAsync();
            await using var fileStream = File.Create(Path.Combine(directoryPath, hat));
            await responseStream.CopyToAsync(fileStream);

            var texture = SpriteUtils.LoadTextureFromByte(fileStream.ReadFully());
            if (texture == null) continue;

            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.53f, 0.575f), texture.width * 0.375f);
            sprite.name = hat;
            if (sprite == null) continue;

            AllSprite.Add(sprite);
            CustomCosmeticsManager.AllCustomCosmeticSprites.Add(sprite);

            texture.DontDestroyAndUnload();
            sprite.DontDestroyAndUnload();
        }

        return;

        void CheckHash(string name, string hashString)
        {
            var path = Path.Combine(directoryPath, name);
            if (hashString == null || !File.Exists(path))
            {
                hatStrings.Add(name);
            }
            else
            {
                using var stream = File.OpenRead(path);
                var hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();

                if (!hashString.Equals(hash))
                    hatStrings.Add(name);
            }
        }
    }

    private static string sanitizeResourcePath(string res)
    {
        if (res == null || !res.EndsWith(".png"))
            return null;

        res = res
            .Replace("\\", "")
            .Replace("/", "")
            .Replace("*", "")
            .Replace("..", "");
        return res;
    }
}