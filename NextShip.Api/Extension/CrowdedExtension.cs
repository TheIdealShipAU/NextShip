namespace NextShip.Api.Extension;

public static class CrowdedExtension
{
    private static bool Started;
    
    public static void UseCrowded()
    {
        if (Started) return;
        
        Started = true;
    }
}