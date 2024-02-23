using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace NextShip.Manager;

[HarmonyPatch]
public static class LoadManager
{
    public static bool Loaded;
    private static bool Started;
    private static bool HasError;

    public static readonly List<IEnumerator> AllLoad = new();

    [HarmonyPatch(typeof(SplashManager), nameof(SplashManager.Update))]
    [HarmonyPrefix]
    public static bool SplashManager_Update_Patch_Load(SplashManager __instance)
    {
        if (Loaded || Started || !__instance.doneLoadingRefdata ||
            !(Time.time - __instance.startTime > __instance.minimumSecondsBeforeSceneChange)) return Loaded;
        __instance.StartCoroutine(Load(__instance));
        Started = true;
        return Loaded;
    }

    [HarmonyPatch(typeof(SplashManager), nameof(SplashManager.Start))]
    [HarmonyPostfix]
    public static void SplashManager_InitializeRefdata_Patch_OnVanillaManagerLoaded(SplashManager __instance)
    {
        Loaded = false;
    }

    public static IEnumerator Load(SplashManager __instance)
    {
        if (AllLoad.Count == 0 || AllLoad == null)
        {
            Loaded = true;
            yield break;
        }

        float t = 1;
        var c2 = Color.white;
        c2.a = 0;

        var logo = GameObjectUtils.CreateCGameObject<SpriteRenderer>();
        var text = GameObjectUtils.CreateCGameObject<TextMeshPro>();

        logo.gameObject.transform.localPosition = new Vector3(0, 0, -5);
        text.gameObject.transform.localPosition = new Vector3(0.2f, -0.9f, -5);

        text.text = "Loading.....";
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 4.5f;

        while (t > 0)
        {
            t -= 0.01f;
            text.color = logo.color = Color.Lerp(Color.white, c2, t);
            yield return null;
        }

        using var enumerator = AllLoad.GetEnumerator();
        var Co = new StackFullCoroutine(enumerator);

        while (Co.CanMove())
        {
            try
            {
                Co.MoveNext();
            }
            catch (Exception e)
            {
                HasError = true;
                StartErrorScreen(e);
                yield break;
            }

            yield return null;
        }

        if (HasError) yield break;

        t = 1;
        text.text = "Load Complete";

        while (t > 0)
        {
            t -= 0.01f;
            text.color = logo.color = Color.Lerp(c2, Color.white, t);
            yield return null;
        }

        Loaded = true;
    }

    private static void StartErrorScreen(Exception exception)
    {
    }

    [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.Awake))]
    [HarmonyPostfix]
    public static void AccountManager_Awake_Patch()
    {
        var fill = GameObject.Find("BlockFill");
        var color = new Color(0, 0, 0)
        {
            a = 0.9f
        };
        fill.GetComponent<SpriteRenderer>().color = color;

        var text = new GameObject("Text");
        text.transform.SetParent(fill.transform);
        text.transform.localPosition = new Vector3(0.02f, -0.04f, -5);
        var TMP = text.AddComponent<TextMeshPro>();
        TMP.text = "登录中...";
        TMP.alignment = TextAlignmentOptions.Center;
        TMP.fontSize = 5;
    }
}