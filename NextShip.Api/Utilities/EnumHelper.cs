#nullable enable
namespace NextShip.Api.Utilities;

// https://github.com/tukasa0001/TownOfHost/blob/main/Helpers/EnumHelper.cs#
public static class EnumHelper
{
    /// <summary>
    ///     enumのすべての値を取得します
    /// </summary>
    /// <typeparam name="T">取得したいenumの型</typeparam>
    /// <returns>Tのすべての値</returns>
    public static T[]? GetAllValues<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)) as T[];
    }

    /// <summary>
    ///     enumのすべての名前を取得します
    /// </summary>
    /// <typeparam name="T">取得したいenumの型</typeparam>
    /// <returns>Tのすべての値の名前</returns>
    public static string[] GetAllNames<T>() where T : Enum
    {
        return Enum.GetNames(typeof(T));
    }
}