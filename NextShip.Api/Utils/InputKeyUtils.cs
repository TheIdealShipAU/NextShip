using UnityEngine;

namespace NextShip.Api.Utils;

public static class InputKeyUtils
{
    public static bool GetKeysDown(params KeyCode[] keys)
    {
        if (!keys.Any(Input.GetKeyDown) || !keys.All(Input.GetKey)) return false;
        Warn($"KeyDown:{keys.First(Input.GetKeyDown)} in [{string.Join(",", keys)}]");
        return true;
    }

    public static bool GetKeyDown(KeyCode key)
    {
        var has = Input.GetKeyDown(key);
        Info($"GetKeyDown:{key} : {has} form {MethodUtils.GetClassName()}");
        return has;
    }
}