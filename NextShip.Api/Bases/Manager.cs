namespace NextShip.Api.Bases;

public class Manager<T> where T : new()
{
    protected static T _instance = default!;

    public static T Instance => Get();

    public static T Get()
    {
        return _instance ??= new T();
    }
}