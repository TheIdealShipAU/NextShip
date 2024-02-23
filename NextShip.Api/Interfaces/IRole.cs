namespace NextShip.Api.Interfaces;

public interface IRole : IDisposable
{
    public Type RoleBaseType { get; set; }
    public Type RoleType { get; set; }

    public SimpleRoleInfo SimpleRoleInfo { get; set; }

    public List<RoleBase> RoleBase { get; set; }

    public List<PlayerControl> Players { get; set; }


    public bool EnableAssign { get; set; }

    public Func<PlayerControl, RoleBase> CreateRoleBase { get; set; }

    public bool CanCreate(IRole role, PlayerControl player);
}