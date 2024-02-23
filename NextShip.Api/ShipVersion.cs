using Hazel;
using NextShip.Api.RPCs;
using UnityEngine;

namespace NextShip.Api;

public class ShipVersion
{
    public ShipVersion()
    {
    }

    public ShipVersion(int major, int minor, int info)
    {
        Major = major;
        Minor = minor;
        Info = info;
    }

    public int Major { get; protected set; }
    public int Minor { get; protected set; }
    public int Info { get; protected set; }

    public string StringText { get; protected set; }

    public void set(int major, int minor, int info)
    {
        Major = major;
        Minor = minor;
        Info = info;
    }

    public ShipVersion Parse(string str)
    {
        var strings = str.Split('.');
        return new ShipVersion(int.Parse(strings[0]), int.Parse(strings[1]), int.Parse(strings[2]));
    }

    public void Write(MessageWriter writer)
    {
        writer.Write(Major);
        writer.Write(Minor);
        writer.Write(Info);
    }

    public void Write(FastRpcWriter writer)
    {
        writer.Write(Major);
        writer.Write(Minor);
        writer.Write(Info);
    }

    public ShipVersion Read(MessageReader reader)
    {
        var major = reader.ReadInt32();
        var minor = reader.ReadInt32();
        var info = reader.ReadInt32();
        return new ShipVersion(major, minor, info);
    }
}

public class AmongUsVersion : ShipVersion
{
    public AmongUsVersion(int major, int minor, int info) : base(major, minor, info)
    {
    }

    private AmongUsVersion()
    {
    }

    public int Year { get; protected set; }
    public int Month { get; protected set; }
    public int Day { get; protected set; }
    public int Revision { get; protected set; }

    internal AmongUsVersion Form()
    {
        return new AmongUsVersion();
    }

    public AmongUsVersion AmongUsSet(int year, int month, int day, int revision = 0)
    {
        Year = year;
        Month = month;
        Day = day;
        Revision = revision;
        return this;
    }

    internal static AmongUsVersion GetFormConstants()
    {
        return new AmongUsVersion().AmongUsSet(Constants.Year, Constants.Month, Constants.Day, Constants.Revision);
    }

    internal static string GetBuildVersion()
    {
        return Application.version;
    }

    public static int GetVersion(int year, int month, int day, int rev)
    {
        return year * 25000 + month * 1800 + day * 50 + rev;
    }

    public static explicit operator string(AmongUsVersion Ver)
    {
        if (Ver.Major == 0 || Ver.Minor == 0 || Ver.Info == 0) return string.Empty;
        return $"{Ver.Major}.{Ver.Minor}.{Ver.Day}";
    }

    public static explicit operator int(AmongUsVersion Ver)
    {
        if (Ver.Year == 0 || Ver.Month == 0 || Ver.Day == 0) return 0;
        return GetVersion(Ver.Year, Ver.Month, Ver.Day, Ver.Revision);
    }
}