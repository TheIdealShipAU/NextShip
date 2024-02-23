namespace NextShip.Api.Enums;

// form Impostor https://github.com/Impostor/Impostor/blob/master/src/Impostor.Api/Net/Messages/MessageFlags.cs
public enum MessageFlags : byte
{
    HostGame = 0,
    JoinGame,
    StartGame,
    RemoveGame,
    RemovePlayer,
    DataAll,
    DataToPlayer,
    JoinedGame,
    EndGame,
    AlterGame = 10,
    KickPlayer,
    WaitForHost,
    Redirect,
    ReselectServer,
    GetGameListV2 = 16,
    ReportPlayer,
    QuickMatch,
    QuickMatchHost,
    SetGameSession,
    SetActivePodType,
    QueryPlatformIds,
    QueryLobbyInfo,

    NextContact = 51
}