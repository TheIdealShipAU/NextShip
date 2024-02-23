using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using AmongUs.Data;

namespace NextShip.Languages;

public class LanguagePack
{
    // 语言文件夹
    public static string LANGUAGEFILE = "Language";

    public static string languageName = "SChinese";

    // 语言文件夹路径
    private static readonly string FPath = $"./{LANGUAGEFILE}";

    // 语言包路径
    public static string PPath = $"{FPath}/{languageName}.dat";

    // 模板文件路径
    private static readonly string LPath = $"{FPath}/lang.dat";

    private static LanguagePack language;
    private static readonly Dictionary<string, string> defaultLanguageSet = new();

    public Dictionary<string, string> languageSet;


    //初始化
    public static void Init()
    {
        Msg("正在Pack加载中", "Language Pack");
        GetLN();
        CTT();
    }

    // 获取Language名称
    public static void GetLN()
    {
        var lang = DataManager.Settings.language.CurrentLanguage.ToString();
        var name = lang.Replace("SupportedLangs.", "");
        languageName = name;
        Info("Language:" + name, "Language Pack");
    }

    // 序号转语言英文名
    public static string GetLname(int id)
    {
        return id switch
        {
            0 => "English",
            1 => "Latam",
            2 => "Brazilian",
            3 => "Portuguese",
            4 => "Korean",
            5 => "Russian",
            6 => "Dutch",
            7 => "Filipino",
            8 => "French",
            9 => "German",
            10 => "Italian",
            11 => "Japanese",
            12 => "Spanish",
            13 => "SChinese",
            14 => "TChinese",
            15 => "Irish",
            _ => ""
        };
    }

    // 创建文件夹
    private static void CTT()
    {
        if ((Directory.GetDirectories(FPath).Length > 0 || Directory.GetFiles(FPath).Length > 0) &&
            File.Exists(LPath) && File.Exists(PPath)) return;
        CreateTT();
        Msg("正在创建语言模板", "Language Pack");
    }

    // 创建语言模板
    private static void CreateTT()
    {
        var text = "";
        foreach (var title in LanguageCSV.translateMaps)
        {
            text += '"' + $"{title.Key}" + '"' + " : " + '"' + LanguageCSV.GetCString(title.Key, 0) + '"' + "\n";
            File.WriteAllText(LPath, text);
        }
    }


    public static string GetPString(string key)
    {
        if (language?.languageSet.ContainsKey(key) ?? false) return language.languageSet[key];
        return "*" + key;
    }

    public static bool CheckValidKey(string key)
    {
        return language?.languageSet.ContainsKey(key) ?? true;
    }

    public static void Load()
    {
        var lang = languageName;


        language = new LanguagePack();
        language.deserialize(@"Language\" + lang + ".dat");
    }

    public void deserialize(string path)
    {
        using var sr = new StreamReader(path, Encoding.GetEncoding("utf-8"));
        deserialize(sr);
    }

    public void deserialize(StreamReader reader)
    {
        try
        {
            string data = "", line;

            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length < 3) continue;
                if (data.Equals(""))
                    data = line;
                else
                    data += "," + line;
            }

            if (data.Equals("")) return;
            var option = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var deserialized = JsonSerializer.Deserialize<Dictionary<string, string>>("{ " + data + " }", option);
            foreach (var entry in deserialized) languageSet[entry.Key] = entry.Value;
        }
        catch (Exception e)
        {
            Exception(e);
        }
    }
}