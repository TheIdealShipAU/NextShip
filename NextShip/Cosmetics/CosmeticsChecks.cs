using System.Collections;
using System.IO;
using NextShip.Api.Attributes;

namespace NextShip.Cosmetics;

[Load]
public static class CosmeticsChecks
{
    private const string TORFolderName = "TheOtherHats";

    private const string ExtremeHatFolderName = "ExtremeHat";
    public static string[] TORHatStrings;

    public static IEnumerator CheckCosmetics()
    {
        yield return null;
    }

    public static bool TORCheck()
    {
        if (!Directory.Exists(GetPatch(TORFolderName))) return false;
        TORHatStrings = Directory.GetFiles(GetPatch(TORFolderName), ".png");
        return true;
    }

    public static bool ExtremeCheck()
    {
        if (!Directory.Exists(GetPatch(ExtremeHatFolderName))) return false;
        var EXHats = Directory.GetDirectories(GetPatch(ExtremeHatFolderName));
        return true;
    }

    private static string GetPatch(string name)
    {
        return "./" + name;
    }
}