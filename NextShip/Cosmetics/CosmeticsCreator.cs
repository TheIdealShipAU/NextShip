using System.Collections.Generic;
using System.Linq;
using NextShip.Cosmetics.Loaders;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NextShip.Cosmetics;

public class CosmeticsCreator
{
    private static Material hatShader;

    public List<Sprite> AllSprites;

    public CosmeticsCreator(List<Sprite> allSprites)
    {
        AllSprites = allSprites;
    }

    private Sprite Get(string name)
    {
        return AllSprites.FirstOrDefault(n => n.name == name);
    }

    public (HatViewData, HatData) CreateHat(CosmeticsInfo info)
    {
        if (hatShader == null) hatShader = FastDestroyableSingleton<HatManager>.Instance.PlayerMaterial;

        var hatData = ScriptableObject.CreateInstance<HatData>();
        var hatView = ScriptableObject.CreateInstance<HatViewData>();

        hatView.MainImage = hatView.FloorImage = Get(info.Resource);
        if (info.ClimbResource != null) hatView.LeftClimbImage = hatView.ClimbImage = Get(info.ClimbResource);
        if (info.Adaptive && hatShader != null) hatView.AltShader = hatShader;
        if (info.BackResource != null)
        {
            hatView.BackImage = Get(info.BackResource);
            info.Behind = true;
        }

        hatData.name = info.Name;
        hatData.displayOrder = 99;
        hatData.ProductId = info.Id;
        hatData.InFront = !info.Behind;
        hatData.NoBounce = !info.Bounce;
        hatData.ChipOffset = new Vector2(0f, 0.2f);
        hatData.Free = true;

        var assetRef = new AssetReference(hatView.Pointer);

        hatData.ViewDataRef = assetRef;
        hatData.CreateAddressableAsset();

        return (hatView, hatData);
    }

    public (NamePlateViewData, NamePlateData) CreateNamePlate(CosmeticsInfo info)
    {
        var namePlateData = ScriptableObject.CreateInstance<NamePlateData>();
        var namePlateView = ScriptableObject.CreateInstance<NamePlateViewData>();

        namePlateView.Image = Get(info.Resource);
        namePlateData.name = info.Name;
        namePlateData.displayOrder = 99;
        namePlateData.ProductId = info.Id;
        namePlateData.Free = true;

        var assetRef = new AssetReference(namePlateView.Pointer);

        namePlateData.ViewDataRef = assetRef;
        namePlateData.CreateAddressableAsset();

        return (namePlateView, namePlateData);
    }

    public (SkinViewData, SkinData) CreateSkin(CosmeticsInfo info)
    {
        var skinData = ScriptableObject.CreateInstance<SkinData>();
        var skinView = ScriptableObject.CreateInstance<SkinViewData>();

        skinView.IdleFrame = Get(info.Resource);
        skinView.EjectFrame = Get(info.BackResource);

        skinData.name = info.Name;
        skinData.displayOrder = 99;
        skinData.ProductId = info.Id;
        skinData.Free = true;

        var assetRef = new AssetReference(skinView.Pointer);

        skinData.ViewDataRef = assetRef;
        skinData.CreateAddressableAsset();
        return (skinView, skinData);
    }

    public (VisorViewData, VisorData) CreateVisor(CosmeticsInfo info)
    {
        var visorData = ScriptableObject.CreateInstance<VisorData>();
        var visorView = ScriptableObject.CreateInstance<VisorViewData>();

        visorView.FloorFrame = Get(info.Resource);
        visorView.ClimbFrame = Get(info.ClimbResource);
        visorView.IdleFrame = Get(info.FlipResource);
        visorView.LeftIdleFrame = Get(info.BackFlipResource);

        visorData.name = info.Name;
        visorData.displayOrder = 99;
        visorData.ProductId = info.Id;
        visorData.Free = true;

        var assetRef = new AssetReference(visorData.Pointer);

        visorData.ViewDataRef = assetRef;
        visorData.CreateAddressableAsset();
        return (visorView, visorData);
    }
}