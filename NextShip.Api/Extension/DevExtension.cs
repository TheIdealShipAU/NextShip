namespace NextShip.Api.Extension;

public static class DevExtension
{
    private static bool Start;

    public static void UseDevExtension()
    {
        if (Start)
            return;

        try
        {
            Start = true;
        }
        catch (Exception e)
        {
            Exception(e);
        }
    }

    public static void DisableDev()
    {
        try
        {
        }
        catch (Exception e)
        {
            Exception(e);
        }

        Start = false;
    }
}