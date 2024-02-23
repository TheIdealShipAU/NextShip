namespace NextShip.Api.RPCs;

public sealed class RPCProjectManager
{
    private static RPCProjectManager Current;

    public readonly List<RPCConnectProject> AllProjects = new();

    public RPCProjectManager()
    {
        Current = this;
    }

    public static RPCProjectManager Instance
    {
        get { return Current ??= new RPCProjectManager(); }
        set => Current = value;
    }
}