namespace NextShip.Api.Plugins;

public interface INextPlugin
{
    public void OnLoad()
    {
    }

    public bool OnEnable()
    {
        return true;
    }

    public bool OnDisable()
    {
        return true;
    }

    public void OnUnload()
    {
    }
}