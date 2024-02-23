using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NextShip.Languages;

public static class Language
{
    public static string GetString(string s, Dictionary<string, string> replacementDic = null)
    {
        var langId = TranslationController.InstanceExists
            ? TranslationController.Instance.currentLanguage.languageID
            : SupportedLangs.English;
        var str = File.Exists(@"Language\" + LanguagePack.languageName + ".dat")
            ? LanguagePack.GetPString(s)
            : LanguageCSV.GetCString(s, langId);
        return replacementDic == null
            ? str
            : replacementDic.Aggregate(str, (current, rd) => current.Replace(rd.Key, rd.Value));
    }

    public static void Init()
    {
        if (!File.Exists(@"Language\" + LanguagePack.languageName + ".dat"))
            LanguageCSV.LoadCSV();
        else
            LanguagePack.Load();
    }
}