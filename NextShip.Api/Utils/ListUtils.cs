namespace NextShip.Api.Utils;

public static class ListUtils
{
    public static List<T> GetSortList<T>(this List<T> list,  Comparison<T> comparison)
    {
        list.Sort(comparison);
        return list;
    }
    
    public static List<T> GetLists<T>(this List<T> list, int count)
    {
        var l = new List<T>();
        for (var i = 0; i < count; i++) l.Add(list[i]);

        return l;
    }

    public static List<T> Disorganize<T>(this List<T> list)
    {
        switch (list.Count)
        {
            case 1:
                return list;
            case 2:
                list.Reverse();
                return list;
        }

        var list2 = new List<T>();
        var random = new Random();
        while (list.Any())
        {
            var i = random.Next(0, list.Count - 1);
            list2.Add(list[i]);
            list.RemoveAt(i);
        }

        return list2;
    }

    public static bool Contains<T>(this T[] objects1, T[] objects2)
    {
        var b = true;
        foreach (var o in objects2)
            if (!objects2.Contains(o))
                b = false;

        return b;
    }
}