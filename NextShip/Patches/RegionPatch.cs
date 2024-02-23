using System;
using System.Collections.Generic;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace NextShip.Patches;

[HarmonyPatch(typeof(RegionMenu), nameof(RegionMenu.Open))]
public static class RegionMenuOpenPatch
{
    public static int ye = 1;
    public static int maxye;
    private static GameObject shangButton;
    private static GameObject xiaButton;
    private static readonly Vector3 pos = new(0f, 2.5f, -100f);

    public static void Postfix(RegionMenu __instance)
    {
        var template = GameObject.Find("NormalMenu/BackButton");
        if (!template) return;
        if (xiaButton == null || xiaButton.gameObject == null)
        {
            xiaButton = template.CreateButton(
                "XiaButton",
                "下一页",
                pos - new Vector3(0f, 3f, 0f),
                __instance.transform,
                () =>
                {
                    if (ye >= maxye) return;
                    ye++;
                    Menu.Update(__instance);
                }
            );

            xiaButton.gameObject.SetActive(!(Main.serverManager.AvailableRegions.Count <= 6));
        }

        if (shangButton != null && shangButton.gameObject != null) return;
        shangButton = template.CreateButton(
            "shangButton",
            "上一页",
            pos - new Vector3(0f, 2.5f, 0f),
            __instance.transform,
            () =>
            {
                if (ye <= 1) return;
                ye--;
                Menu.Update(__instance);
            }
        );

        xiaButton.gameObject.SetActive(!(Main.serverManager.AvailableRegions.Count <= 6));
    }

    private static GameObject CreateButton(this GameObject template, string name, string text, Vector3 Position,
        Transform Preant, Action action)
    {
        var Button = Object.Instantiate(template, Preant);
        Button.name = name;
        Button.transform.position = Position;

        Button.DestroyTranslator();
        Button.DestroyAspectPosition();
        Button.DestroyComponents<SceneChanger>();
        Button.DestroyComponents<ButtonRolloverHandler>();

        var ButtonText = Button.transform.GetComponentInChildren<TMP_Text>();
        var ButtonPassiveButton = Button.GetComponent<PassiveButton>();

        ButtonPassiveButton.OnClick = new Button.ButtonClickedEvent();
        ButtonPassiveButton.OnClick.AddListener(action);
        ButtonText.SetText(text);

        return Button;
    }
}

[HarmonyPatch]
public static class Menu
{
    private static readonly ServerManager serverManager = DestroyableSingleton<ServerManager>.Instance;

    [HarmonyPatch(typeof(RegionMenu), nameof(RegionMenu.OnEnable))]
    [HarmonyPrefix]
    public static bool RegionMenuOnEnablePatch(RegionMenu __instance)
    {
        if (serverManager.AvailableRegions.Count <= 3) return true;
        CreateRegionMenu(__instance);
        return false;
    }

    private static void CreateRegionMenu(RegionMenu __instance)
    {
        CreateServerOption(__instance);
        ControllerManager.Instance.OpenOverlayMenu(__instance.name, __instance.BackButton,
            __instance.defaultButtonSelected, __instance.controllerSelectable);
    }

    private static void CreateServerOption(RegionMenu __instance)
    {
        RegionMenuOpenPatch.maxye = serverManager.AvailableRegions.Count / 3 +
                                    (serverManager.AvailableRegions.Count % 3 == 0 ? 0 : 1);
        var regionInfos = Array.Empty<IRegionInfo>();
        if (serverManager.AvailableRegions.Count < 6)
            regionInfos = serverManager.AvailableRegions;
        else
            UpdateYer();

        __instance.controllerSelectable.Clear();
        List<UiElement> List = [];
        var num = 0;
        foreach (var regionInfo in regionInfos)
        {
            var region = regionInfo;
            var serverListButton = __instance.ButtonPool.Get<ServerListButton>();
            serverListButton.transform.localPosition = new Vector3(0f, 1.5f - 0.5f * num, 0f);
            serverListButton.SetTextTranslationId(regionInfo.TranslateName, regionInfo.Name);
            serverListButton.Text.text =
                DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(regionInfo.TranslateName,
                    regionInfo.Name);
            serverListButton.Text.ForceMeshUpdate();
            serverListButton.Button.OnClick.RemoveAllListeners();
            serverListButton.Button.OnClick.AddListener((Action)(() => { __instance.ChooseOption(region); }));
            serverListButton.SetSelected(serverManager.CurrentRegion.Name == regionInfo.Name);
            List.Add(serverListButton.Button);
            num++;
        }

        List.Do(n => __instance.controllerSelectable.Add(n));
        return;

        void UpdateYer()
        {
            var list = new List<IRegionInfo>();
            for (var i = 0; i < 3; i++)
            {
                var s = RegionMenuOpenPatch.ye * 3 - i;
                if (s <= serverManager.AvailableRegions.Count) list.Add(serverManager.AvailableRegions[s - 1]);
                regionInfos = list.ToArray();
            }
        }
    }

    public static void Update(RegionMenu __instance)
    {
        __instance.ButtonPool.ReclaimAll();
        CreateServerOption(__instance);
    }
}