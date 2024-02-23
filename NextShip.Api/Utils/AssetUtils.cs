#nullable enable
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace NextShip.Api.Utils;

public static class AssetUtils
{
    private static readonly CosmeticsCache _CosmeticsCache = ShipStatus.Instance.CosmeticsCache;
    private static readonly HatManager _hatManager = FastDestroyableSingleton<HatManager>.Instance;

    public static PetBehaviour GetPetBehaviour(this string id)
    {
        var asset = _CosmeticsCache.GetPet(id);
        if (asset.Data.ProdId != "pet_EmptyPet" ||
            _hatManager.allPets.FirstOrDefault(n => n.ProdId == id) == null) return asset;

        var Asset = _hatManager.GetPetById(id).CreateAddressableAsset();
        Asset.LoadAsync((Action)(() => asset = Asset.GetAsset()));

        return asset;
    }

    public static HatViewData GetHatViewData(this string id)
    {
        var asset = _CosmeticsCache.GetHat(id);
        return asset;
    }

    public static SkinViewData GetSkinViewData(this string id)
    {
        var asset = _CosmeticsCache.GetSkin(id);
        return asset;
    }

    public static VisorViewData GetVisorViewData(this string id)
    {
        var asset = _CosmeticsCache.GetVisor(id);
        return asset;
    }

    public static NamePlateViewData GetNamePlateViewData(this string id)
    {
        var asset = _CosmeticsCache.GetNameplate(id);
        return asset;
    }


    public static T? LoadAsset<T>(this AssetBundle? bundle, string name) where T : Il2CppObjectBase
    {
        return bundle != null ? bundle.LoadAsset(name, Il2CppType.Of<T>()).Cast<T>() : null;
    }

    public static T[] LoadAllAsset<T>(this AssetBundle bundle) where T : Il2CppObjectBase
    {
        var assets = bundle.LoadAllAssets(Il2CppType.Of<T>());
        var assetArray = new T[assets!.Length];
        var count = 0;
        foreach (var asset in assets)
        {
            assetArray[count] = asset.Cast<T>();
            count++;
        }

        return assetArray;
    }
}