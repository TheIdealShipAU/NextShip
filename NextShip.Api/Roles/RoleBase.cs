namespace NextShip.Api.Roles;

public abstract class RoleBase(PlayerControl player) : IDisposable
{
    public PlayerControl Player { get; private set; } = player;

    public Func<bool> WinCheck { get; } = () => false;
    public bool CanKill { get; } = false;
    public bool CanVent { get; } = false;
    public bool HasTask { get; } = true;

    public bool Active { get; protected set; } = true;

    public void Dispose()
    {
        OnDestroy();
        Player = null;
    }

    public virtual void RpcSyncWriter()
    {
    }

    public virtual void RpcSyncReader()
    {
    }

    public virtual void OnStart()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnTeamShow()
    {
    }

    public virtual void OnRoleShow()
    {
    }

    public virtual void OnMeetingStart()
    {
    }

    public virtual void OnExile()
    {
    }

    public virtual void OnExileEnd()
    {
    }

    protected virtual void OnDestroy()
    {
    }
}