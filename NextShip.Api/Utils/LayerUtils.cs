using UnityEngine;

namespace NextShip.Api.Utils;

// form https://github.com/Dolly1016/Nebula/blob/master/Nebula/Expansions/LayersExpansion.cs 

public static class LayerUtils
{
    private static int? defaultLayer;
    private static int? shortObjectsLayer;
    private static int? objectsLayer;
    private static int? uiLayer;
    private static int? shipLayer;
    private static int? shadowLayer;

    public static int GetFormName(this string name)
    {
        return LayerMask.NameToLayer(name);
    }

    public static string GetName(this int layer)
    {
        return LayerMask.LayerToName(layer);
    }

    public static int GetDefaultLayer()
    {
        defaultLayer ??= LayerMask.NameToLayer("Default");
        return defaultLayer.Value;
    }

    public static int GetShortObjectsLayer()
    {
        shortObjectsLayer ??= LayerMask.NameToLayer("ShortObjects");
        return shortObjectsLayer.Value;
    }

    public static int GetObjectsLayer()
    {
        objectsLayer ??= LayerMask.NameToLayer("Objects");
        return objectsLayer.Value;
    }

    public static int GetUILayer()
    {
        uiLayer ??= LayerMask.NameToLayer("UI");
        return uiLayer.Value;
    }

    public static int GetShipLayer()
    {
        shipLayer ??= LayerMask.NameToLayer("Ship");
        return shipLayer.Value;
    }

    public static int GetShadowLayer()
    {
        shadowLayer ??= LayerMask.NameToLayer("Shadow");
        return shadowLayer.Value;
    }

    public static int GetShadowObjectsLayer()
    {
        return 30;
    }
}