using System.Collections.Generic;
using System.Linq;
using Twitch;
using UnityEngine;

namespace NextShip.UI.Module;

public class FastScreen
{
    public static List<FastScreen> AllFastScreen = new();

    public GameObject _GameObject;
    public int Index;

    public string Name;

    public FastScreen(string name)
    {
        Name = name;
        Index = GetIndex();

        _GameObject = new GameObject(name);

        AllFastScreen.Add(this);
    }

    private static int GetIndex()
    {
        if (AllFastScreen.Count == 0) return 1;

        AllFastScreen.Sort((x, y) => x.Index.CompareTo(y.Index));
        return AllFastScreen.Last().Index++;
    }

    private static void Sort()
    {
        var index = 1;
        foreach (var t in AllFastScreen)
        {
            t.Index = index;
            index++;
        }
    }

    private static void Remove(int index)
    {
        foreach (var screen in AllFastScreen.Where(screen => screen.Index == index)) AllFastScreen.Remove(screen);
    }

    public FastScreen GenerateBackGround(bool defaultWindow = true, Sprite sprite = null, Vector2 size = default,
        Vector3 Position = default)
    {
        var BackGround =
            GameObjectUtils.CreateGameObject<SpriteRenderer>("Background", _GameObject.transform, Position);
        BackGround.Item2.sprite = TwitchManager.Instance.transform.GetChild(0).GetChild(3)
            .GetComponent<SpriteRenderer>().sprite;
        BackGround.Item2.drawMode = SpriteDrawMode.Sliced;
        BackGround.Item2.tileMode = SpriteTileMode.Continuous;
        BackGround.Item1.layer = LayerUtils.GetUILayer();

        return this;
    }

    public FastScreen GenerateWindow(bool defaultWindow = true, Sprite sprite = null, Vector2 size = default,
        Vector3 Position = default)
    {
        return this;
    }

    public static FastScreen Get(string Name)
    {
        return AllFastScreen.FirstOrDefault(n => n.Name == Name) ?? new FastScreen(Name);
    }
}