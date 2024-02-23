namespace NextShip.Api.Enums;

// from https://github.com/Impostor/Impostor/blob/master/src/Impostor.Server/Net/Inner/GameDataTag.cs
public enum DataFlags : byte
{
    Data = 1,
    Rpc = 2,
    Spawn = 4,
    Despawn = 5,
    SceneChange = 6,
    Ready = 7,
    ChangeSettings = 8,

    FastRPC = 50,

    ConsoleDeclareClientPlatform = 205,
    PS4RoomRequest = 206
}