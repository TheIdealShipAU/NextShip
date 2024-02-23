using InnerNet;
using NextShip.Api.Interfaces;

namespace NextShip.Api.Bases;

public record NextInfo
{
    public float BodySpeed;

    public ClientData ClientData;

    public int clientId;

    public int ColorId;

    public string FriendCode;

    public float GhostSpeed;

    public bool IsHost;

    public bool IsLocal;

    public PlayerControl PlayerControl;

    public PlayerPhysics PlayerPhysics;

    public byte PlayerId;

    public GameData.PlayerInfo PlayerInfo;

    public uint PlayerLevel;

    public string PlayerName;

    public string PUID;

    public IRole Role;

    public RoleBase RoleBase;
}