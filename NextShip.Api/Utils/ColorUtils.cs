using System.Text.RegularExpressions;
using UnityEngine;

namespace NextShip.Api.Utils;

public static partial class ColorUtils
{
    public static Color32 HTMLColorTo32(this string HTML_Color)
    {
        var regex = MyRegex();
        if (ColorUtility.TryParseHtmlString(HTML_Color, out var color) && regex.IsMatch(HTML_Color)) return color;

        return new Color32(255, 255, 255, byte.MinValue);
    }

    [GeneratedRegex("^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$")]
    private static partial Regex MyRegex();
}