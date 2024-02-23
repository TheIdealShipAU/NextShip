#nullable enable
using Newtonsoft.Json.Linq;

namespace NextShip.Api.Utils;

public static class JsonUtils
{
    public static string? GetString(this JToken token, string key)
    {
        return token[key]?.ToString();
    }
}