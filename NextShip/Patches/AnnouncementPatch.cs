using System.Collections.Generic;
using System.Linq;
using AmongUs.Data.Player;
using Assets.InnerNet;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace NextShip.Patches;

[HarmonyPatch]
public class AnnouncementPatch
{
    //https://github.com/Dolly1016/Nebula/blob/master/Nebula/Module/ModUpdater.cs
    public static readonly List<Announcement> ModUpdateAnnouncements = [];

    [HarmonyPatch(typeof(PlayerAnnouncementData), nameof(PlayerAnnouncementData.SetAnnouncements))]
    [HarmonyPrefix]
    public static void SetModAnnouncements(PlayerAnnouncementData __instance,
        [HarmonyArgument(0)] ref Il2CppReferenceArray<Announcement> aRange)
    {
        if (ModUpdateAnnouncements == null || ModUpdateAnnouncements.Count == 0) return;
        List<Announcement> list = [..aRange.ToList()];
        list.AddRange(ModUpdateAnnouncements);
        list.Sort(AnCompare);
        aRange = list.ToArray();
    }

    public static void AddAnnouncement(Announcement an)
    {
        while (ModUpdateAnnouncements.Count > 5) ModUpdateAnnouncements.RemoveAt(0);
        ModUpdateAnnouncements.Add(an);
    }

    private static int AnCompare(Announcement an1, Announcement an2)
    {
        var time1 = an1.Date.Split('-');
        var time2 = an2.Date.Split('-');
        var Sort = 0;
        for (var i = 0; i < 3; i++)
        {
            var t1 = int.Parse(time1[i]);
            var t2 = int.Parse(time2[i]);

            if (t1 == t2) continue;

            if (t1 > t2)
                Sort = 1;

            if (t1 < t2)
                Sort = -1;
        }

        return Sort;
    }
}

public class ModAnnouncement
{
    public uint LanguageId;
    public string ShortTitle;
    public string SubTitle;
    public string text;
    public string Time;
    public string Title;

    private ModAnnouncement
    (
        string text,
        string Title,
        string Time,
        string SubTitle = "",
        string ShortTitle = "",
        uint LanguageId = 13
    )
    {
        this.text = text;
        this.Title = Title;
        this.LanguageId = LanguageId;
        this.SubTitle = SubTitle;
        this.ShortTitle = ShortTitle;
        this.Time = Time.Replace('.', '-');
    }

    public Announcement ToAn(ModAnnouncement modAn)
    {
        var an = new Announcement
        {
            Id = "mod",
            Language = modAn.LanguageId,
            Number = AnnouncementPatch.ModUpdateAnnouncements != null
                ? AnnouncementPatch.ModUpdateAnnouncements[AnnouncementPatch.ModUpdateAnnouncements.Count].Number + 1
                : 1000,
            Text = modAn.text,
            SubTitle = modAn.SubTitle,
            ShortTitle = modAn.ShortTitle,
            Title = modAn.Title,
            Date = modAn.Time
        };
        return an;
    }
}