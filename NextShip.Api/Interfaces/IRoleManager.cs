namespace NextShip.Api.Interfaces;

public interface IRoleManager
{
    public void Register(IRole role);

    public void UnRegister(IRole role);

    public void AssignRole(PlayerControl player, IRole role);

    public void Clear();

    public void SetCreator(IRoleCreator creator);
}