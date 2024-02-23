namespace NextShip.Api.Interfaces;

public interface IRoleCreator : IDisposable
{
    public T Create<T>(IRole role) where T : RoleBase;

    public T GetRole<T>(PlayerControl player) where T : class;

    public void Clear();

    public IRole GetAssign();
}