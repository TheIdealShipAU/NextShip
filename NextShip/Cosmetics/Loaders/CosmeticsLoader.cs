using System.Collections.Generic;
using System.Threading.Tasks;
using NextShip.Api.Config;
using NextShip.Api.Enums;
using UnityEngine;

namespace NextShip.Cosmetics.Loaders;

public abstract class CosmeticsLoader
{
    public const string HatJsonName = "CustomHats.json";
    public readonly List<CosmeticsInfo> AllCosmeticsInfo;
    public readonly List<HatViewData> AllHat;
    public readonly List<NamePlateViewData> AllNamePlate;
    public readonly List<Sprite> AllSprite;
    public readonly Dictionary<string, Dictionary<HatData, HatViewData>> Hats = new();


    public List<VisorViewData> AllVisor;

    protected CosmeticsLoader()
    {
        CustomCosmeticsManager.AllLoaders.Add(this);
        AllHat = [];
        AllVisor = [];
        AllNamePlate = [];
        AllSprite = [];
        AllCosmeticsInfo = [];
    }

    protected CosmeticsLoader(List<HatViewData> allHat)
    {
        AllHat = allHat;
    }

    public bool Loaded { get; protected set; }
    public bool AddToList { get; protected set; }

    public abstract CosmeticType GetCosmeticType();
    public abstract CosmeticRepoType GetCosmeticRepoType();

    public void Load()
    {
    }

    public virtual void Remove()
    {
    }

    public virtual void LoadFormDisk()
    {
    }

    public virtual void LoadFormResources()
    {
    }

    public virtual Task LoadFormOnRepo(CosmeticsConfig cosmeticsConfig)
    {
        return Task.CompletedTask;
    }
}