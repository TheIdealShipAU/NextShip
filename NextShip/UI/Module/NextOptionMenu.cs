using System;
using System.Collections.Generic;
using HarmonyLib;
using NextShip.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace NextShip.UI.Module;

[HarmonyPatch]
public class NextOptionMenu
{
    public static NextOptionMenu Instance;
    public readonly GameObject NextMenuParent;
    public OptionsConsole __instance;
    private List<GameObject> AllListButton = new();
    public GameObject CloneButton;
    public GameObject GeneralSettingOption;
    public bool Initd;
    public Transform List;
    public GameObject RoleSettingOption;
    public Il2CppSystem.Collections.Generic.List<UiElement> UiElements;

    public NextOptionMenu(OptionsConsole __instance, GameObject NextMenuParent)
    {
        Initd = false;

        this.__instance = __instance;
        this.NextMenuParent = NextMenuParent;
        Instance = this;
        UiElements = new Il2CppSystem.Collections.Generic.List<UiElement>();
    }

    public void Init()
    {
        if (Initd) return;

        try
        {
            var BackGround = new GameObject("BackGround");
            BackGround.transform.SetParent(NextMenuParent.transform);
            var BackGroundBaffle =
                GameObjectUtils.CreateCGameObject<BoxCollider2D>("BackGroundBaffle", BackGround.transform);
            BackGroundBaffle.size = new Vector2(13f, 9);
            BackGroundBaffle.gameObject.CreatePassiveButton();

            var tint = Object.Instantiate(__instance.MenuPrefab.transform.Find("Tint").gameObject,
                BackGround.transform);
            Object.Instantiate(__instance.MenuPrefab.transform.Find("Background").transform.GetChild(1).gameObject,
                BackGround.transform);

            var Templates = new GameObject("Templates");
            Templates.transform.SetParent(NextMenuParent.transform);


            var ScrollMenu = CreateScrollMenu
            (
                "NextScrollMenu",
                NextMenuParent.transform,
                new Vector3(1.6f, 2.9f, -5),
                new Vector3(0, 0, -5f),
                new Vector3(0, 0, -5f),
                new Vector2(0.03f, 6),
                new Vector2(0.05f, 1),
                new FloatRange(-5.4f, 0.4f),
                new FloatRange(-1, 1)
            );
            List = ScrollMenu.Item2;

            var tempOption =
                Object.Instantiate(Object.FindObjectOfType<StringOption>().gameObject, List, true);
            tempOption.AddComponent<ShipOptionBehaviour>();
            tempOption.DestroyComponents<StringOption>();
            tempOption.DestroyComponents<UIScrollbarHelper>();
            tempOption.DestroyComponents<ButtonRolloverHandler>();
            tempOption.GetComponentInChildren<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
            var text = tempOption.GetComponentInChildren<TextMeshPro>();
            text.text = "text";


            GeneralSettingOption = CreateLargeButton("GeneralSettingOption", NextMenuParent.transform,
                new Vector3(-3.5f, 1.5f, 0), "常规设置", "关于设置的选项", () => OpenOptionMenu(MenuIndex.GeneralSetting));
            RoleSettingOption = CreateLargeButton("RoleSettingOption", NextMenuParent.transform,
                new Vector3(-3.5f, 0, 0), "职业设置", "关于职业的选项设置", () => OpenOptionMenu(MenuIndex.RoleSetting));
            CloneButton = CreateSmallButton("CloneButton", NextMenuParent.transform, new Vector3(-3.5f, -1.1f, 0), "关闭",
                CloseMenu);

            NextMenuParent.SetActive(false);
            NextMenuParent.AllGameObjectDo(n => n.layer = tint.gameObject.layer);

            NextMenuParent.AddComponent<NextMenuOption>().__OptionMenu = this;

            Templates.SetActive(false);
            Initd = true;
        }
        catch (Exception e)
        {
            Exception(e);
            Error("生成错误");
            Initd = false;
        }
    }

    private void OpenOptionMenu(MenuIndex menu)
    {
        Info("Open OptionMenu");
    }

    private void CloseOptionMenu(MenuIndex menu)
    {
    }

    public bool OpenMenu(Vector3 pos)
    {
        if (!NextMenuParent) Initd = false;
        if (!Initd) return false;
        PlayerControl.LocalPlayer.NetTransform.Halt();
        NextMenuParent.transform.localPosition = pos;
        NextMenuParent.transform.SetParent(Camera.main!.transform, false);
        ControllerManager.Instance.OpenOverlayMenu(NextMenuParent.name, Instance.CloneButton.GetComponent<UiElement>(),
            null, Instance.UiElements);
        FastDestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, NextMenuParent, null);
        return true;
    }

    public void CloseMenu()
    {
        Info("Close NextOptionMenu");
        NextMenuParent.SetActive(false);
        ControllerManager.Instance.CloseOverlayMenu(NextMenuParent.name);
    }

    private static GameObject CreateSmallButton(string name, Transform Parent, Vector3 position, string Text,
        Action action = null)
    {
        return CreateButton(Text, name, Parent,
            new Vector2(2.5f, 0.65f),
            action,
            position,
            new Vector3(0f, -2.22f, -1),
            default, TitleTextSize: 3.5f);
    }

    private static GameObject CreateLargeButton(string name, Transform Parent, Vector3 position, string TitleText,
        string SubText, Action action = null)
    {
        return CreateButton(TitleText, name, Parent,
            new Vector2(2.5f, 1.3f),
            action,
            position,
            new Vector3(0, -1.9f, -1),
            new Vector3(-8.9f, -2.8f, -1),
            true, SubText, 4.2f, 1.5f);
    }

    private (GameObject, Transform) CreateScrollMenu
    (
        string Name,
        Transform Parent,
        Vector3 vector3,
        Vector3 ScrollBar_TrackVector3,
        Vector3 ScrollBar_HandleVector3,
        Vector2 ScrollBar_TrackSize,
        Vector2 ScrollBar_HandleSize,
        FloatRange BarRange,
        FloatRange contentRange,
        bool YorX = true,
        Sprite trackSprite = null,
        Sprite HandleSprite = null
    )
    {
        var ScrollMenu = new GameObject(Name);
        ScrollMenu.transform.SetParent(Parent);

        var MenuScroll = new GameObject(Name + "_Scroll");
        MenuScroll.transform.SetParent(ScrollMenu.transform);
        MenuScroll.transform.localPosition = vector3;

        var Scroll = GameObject.Find("ChatUi").transform.Find("QuickChatMenu").transform
            .Find("Container").transform.Find("Pages").transform.Find("PhrasesPage").transform.Find("Scroller");

        var tem_Handle = Scroll.transform.Find("ScrollBar_Handle").gameObject;
        var tem_Track = Scroll.transform.Find("ScrollBar_Track").gameObject;

        tem_Handle.GetComponent<SpriteRenderer>().sprite.CaChe("Handle_Sprite");
        tem_Track.GetComponent<SpriteRenderer>().sprite.CaChe("Track_Sprite");

        var ScrollBar_Handle = Object.Instantiate(tem_Handle, MenuScroll.transform, true);
        ScrollBar_Handle.name = "ScrollBar_Handle";
        var ScrollBar_Track = Object.Instantiate(tem_Track, MenuScroll.transform, true);
        ScrollBar_Track.name = "ScrollBar_Track";

        var tra = ScrollBar_Track.GetComponent<SpriteRenderer>();

        var bar = ScrollBar_Handle.GetComponent<Scrollbar>();
        var barSp = ScrollBar_Handle.GetComponent<SpriteRenderer>();
        bar.graphic = barSp;
        bar.trackGraphic = tra;

        bar.OnMouseOut = new UnityEvent();
        bar.OnMouseOver = new UnityEvent();

        if (ScrollBar_HandleVector3 != default) ScrollBar_Handle.transform.localPosition = ScrollBar_HandleVector3;
        if (ScrollBar_HandleSize != default) barSp.size = ScrollBar_HandleSize;
        if (HandleSprite) barSp.sprite = HandleSprite;

        if (ScrollBar_TrackVector3 != default) ScrollBar_Track.transform.localPosition = ScrollBar_TrackVector3;
        if (ScrollBar_TrackSize != default) tra.size = ScrollBar_TrackSize;
        if (trackSprite) tra.sprite = trackSprite;

        var list = new GameObject("List");
        list.transform.SetParent(ScrollMenu.transform);

        var box = ScrollMenu.AddComponent<BoxCollider2D>();
        box.size = new Vector2(1, 1);
        var scroller = ScrollMenu.AddComponent<Scroller>();
        scroller.allowY = YorX;
        scroller.showY = YorX;
        scroller.allowX = !YorX;
        scroller.showX = !YorX;
        scroller.OnMouseOut = new UnityEvent();
        scroller.OnMouseOver = new UnityEvent();
        scroller.Inner = list.transform;

        if (YorX)
            scroller.ScrollbarY = bar;
        else
            scroller.ScrollbarX = bar;

        bar.parent = scroller;

        scroller.ScrollbarYBounds = BarRange;
        scroller.ContentYBounds = contentRange;

        Instance.UiElements.Add(scroller);
        Instance.UiElements.Add(bar);

        return (ScrollMenu, list.transform);
    }

    private static GameObject CreateButton
    (
        string Title,
        string name,
        Transform Parent,
        Vector2 BackSpriteSize,
        Action action,
        Vector3 LocalPosition,
        Vector3 TitlePosition,
        Vector3 SubTextPosition,
        bool enableSubText = false,
        string Subtext = "",
        float TitleTextSize = 5,
        float SubTextSize = 3,
        Sprite ButtonBackSprite = null
    )
    {
        var button = new GameObject(name)
        {
            transform =
            {
                localPosition = LocalPosition
            }
        };
        button.transform.SetParent(Parent);

        var backGround = new GameObject("BackGround");
        backGround.transform.SetParent(button.transform);
        backGround.transform.localPosition = new Vector3(0, 0, -1);

        var backGroundSprite = backGround.AddComponent<SpriteRenderer>();
        backGroundSprite.sprite = ButtonBackSprite ? ButtonBackSprite : ObjetUtils.Find<Sprite>("buttonClick");
        backGroundSprite.drawMode = SpriteDrawMode.Sliced;
        backGroundSprite.size = BackSpriteSize;

        var onclick = backGround.CreatePassiveButton(action);
        Instance.UiElements.Add(onclick);

        var titleTextGameObject = new GameObject("TitleText");
        titleTextGameObject.transform.SetParent(button.transform);
        titleTextGameObject.transform.localPosition = TitlePosition;

        var textMeshPro = titleTextGameObject.AddComponent<TextMeshPro>();
        textMeshPro.text = Title;
        textMeshPro.fontSize = TitleTextSize;
        textMeshPro.horizontalAlignment = HorizontalAlignmentOptions.Center;

        if (!enableSubText) return button;
        var SubTextGameObject = new GameObject("SubText");
        SubTextGameObject.transform.SetParent(button.transform);
        SubTextGameObject.transform.localPosition = SubTextPosition;

        var SubTextMeshPro = SubTextGameObject.AddComponent<TextMeshPro>();
        SubTextMeshPro.text = Subtext;
        SubTextMeshPro.fontSize = SubTextSize;
        SubTextMeshPro.horizontalAlignment = HorizontalAlignmentOptions.Right;

        return button;
    }

    private enum MenuIndex
    {
        GeneralSetting = 0,
        RoleSetting = 1
    }
}