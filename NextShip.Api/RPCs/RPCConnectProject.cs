namespace NextShip.Api.RPCs;

public abstract class RPCConnectProject
{
    public string ID;

    protected RPCConnectProject()
    {
        RPCProjectManager.Instance.AllProjects.Add(this);
    }

    public abstract void OnStart();

    public virtual void OnEnd()
    {
    }
}