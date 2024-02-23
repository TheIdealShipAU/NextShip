using UnityEngine;

namespace NextShip.Api.Bases;

public class NKeyBind
{
    /// <summary>
    /// 0 last
    /// </summary>
    public int Priority;

    /// <summary>
    /// 1 key or 2 keys or 3
    /// </summary>
    public int Mode;

    public KeyCode[] keys;

    public int KeyCount;

    public Action _Action;
}