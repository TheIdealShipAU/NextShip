using NextShip.Api.Attributes;
using UnityEngine;

namespace NextShip.UI.UIManager;

[Il2CppRegister]
public class NextUIManager : MonoBehaviour
{
    private static NextUIManager CurrentManager;

    public NextUIManager()
    {
        if (CurrentManager)
            this.Destroy();

        this.DontDestroyAndUnload();
        CurrentManager ??= this;
    }

    public static void Get()
    {
        CurrentManager ??= Main.Instance.AddComponent<NextUIManager>();
    }

    public void SetDescription()
    {
    }
}