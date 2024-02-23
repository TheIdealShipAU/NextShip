#nullable enable
using System.Collections.Generic;
using System.Linq;
using InnerNet;
using NextShip.Api.Bases;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

public class NextPlayerManager : IPlayerManager
{
    private readonly List<NextInfo> PlayerInfos = [];


    public static NextPlayerManager Instance => Main._Service.Get<NextPlayerManager>();

    public NextInfo CreateOrGetSetPlayerInfo(PlayerControl player)
    {
        var info = PlayerInfos.Exists(IsInfo) ? PlayerInfos.First(IsInfo) : new NextInfo();

        info.PlayerControl = player;
        info.PlayerId = player.PlayerId;
        info.PlayerInfo = player.Data;
        info.IsHost = AmongUsClient.Instance.HostId == info.clientId;
        info.IsLocal = CachedPlayer.LocalPlayer?.PlayerControl == player;
        return info;

        bool IsInfo(NextInfo nextInfo)
        {
            return nextInfo.PlayerControl == player || nextInfo.PlayerId == player.PlayerId;
        }
    }

    public NextInfo CreateOrGetSetPlayerInfo(ClientData data)
    {
        var info = PlayerInfos.Exists(IsInfo) ? PlayerInfos.First(IsInfo) : new NextInfo();

        info.ClientData = data;
        info.clientId = data.Id;
        info.FriendCode = data.FriendCode;
        info.PUID = data.ProductUserId;
        info.PlayerLevel = data.PlayerLevel;
        info.PlayerName = data.PlayerName;
        info.PlayerControl = data.Character;
        info.ColorId = data.ColorId;
        info.IsHost = AmongUsClient.Instance.HostId == data.Id;
        info.IsLocal = CachedPlayer.LocalPlayer?.PlayerControl == data.Character;
        return info;

        bool IsInfo(NextInfo nextInfo)
        {
            return nextInfo.clientId == data.Id || nextInfo.PlayerControl == data.Character;
        }
    }

    public void InitPlayer(PlayerControl player)
    {
    }

    public bool IsHost(PlayerControl player)
    {
        return PlayerInfos.First(n => n.PlayerControl == player).IsHost;
    }


    public bool TryGetPlayer(PlayerControl player, out NextInfo? info)
    {
        if (PlayerInfos.Exists(n => n.PlayerControl == player))
        {
            info = PlayerInfos.First(n => n.PlayerControl == player);
            return true;
        }

        info = null;
        return false;
    }


    public void Clear()
    {
        PlayerInfos.Clear();
    }
}