using System;
using Hazel;
using NextShip.Roles;
using static NextShip.Api.Roles.RoleEnum;

namespace NextShip.RPC;

internal enum CustomRPC
{
    // Main 主要
    ResetVariables,
    ShareOptions,
    WorkaroundSetRoles,
    SetRole,
    SetModifier,
    setDead,
    RestoreRole,
    ChangeRole,
    RestorePlayerLook,
    customrpc,
    VersionShakehand,
    HistorySynchronization,
    RpcValueHandshake,

    // Role 职业相关
    SheriffKill,
    Camouflager,
    Illusory,
    SchrodingerSCatTeamChange,
    LoverSendChat
}

public static class RPCProcedure
{
    public static void ResetVariables()
    {
    }

    public static void WorkaroundSetRoles(byte numberOfRoles, MessageReader reader)
    {
        for (var i = 0; i < numberOfRoles; i++)
        {
            var playerId = (byte)reader.ReadPackedUInt32();
            var roleId = (byte)reader.ReadPackedUInt32();
            try
            {
                setRole(roleId, playerId);
            }
            catch (Exception e)
            {
                Error("Error while deserializing roles: " + e.Message);
            }
        }
    }

    public static void setRole(byte roleId, byte playerId)
    {
        var player = PlayerUtils.GetPlayerForId(playerId);
        switch ((RoleId)roleId)
        {
            case RoleId.Sheriff:
                Sheriff.sheriff = player;
                break;

            case RoleId.Jester:
                Jester.jester = player;
                break;

            case RoleId.Camouflager:
                Roles.Camouflager.camouflager = player;
                break;

            case RoleId.Illusory:
                Roles.Illusory.illusory = player;
                break;
        }
    }

    public static void setModifier(byte modifierId, byte playerId, byte flag)
    {
        var player = PlayerUtils.GetPlayerForId(playerId);
        switch ((RoleId)modifierId)
        {
            case RoleId.Flash:
                Flash.flash = player;
                break;

            case RoleId.Lover:
                if (flag == 0)
                    Lover.lover1 = player;
                else
                    Lover.lover2 = player;
                break;
        }
    }

    public static void setDead(byte id, bool isDead)
    {
        var player = PlayerUtils.GetPlayerForId(id);
        player.Data.IsDead = isDead;
    }

    public static void SheriffKill(byte targetId)
    {
        var player = PlayerUtils.GetPlayerForId(targetId);
        Sheriff.sheriff.MurderPlayer(player, MurderResultFlags.Succeeded);
    }

    public static void RestoreRole(byte id)
    {
        switch ((RoleId)id)
        {
            case RoleId.Sheriff:
                Sheriff.sheriff = null;
                break;
            case RoleId.Flash:
                Flash.flash = null;
                break;
            case RoleId.Jester:
                Jester.jester = null;
                break;
        }
    }

    public static void ChangeRole(byte playerId, byte targetRoleId)
    {
        var player = PlayerUtils.GetPlayerForId(playerId);
        setRole(targetRoleId, playerId);
    }

    public static void RestorePlayerLook()
    {
        foreach (var player in CachedPlayer.AllPlayers) player.PlayerControl.setDefaultLook();
    }

    public static void Camouflager()
    {
        foreach (var player in CachedPlayer.AllPlayers) player.PlayerControl.setLook("", 6, "", "", "", "");
    }

    public static void Illusory()
    {
        if (CachedPlayer.LocalPlayer.PlayerControl.Data.Role.IsImpostor) return;
        for (var i = 0; i < CachedPlayer.AllPlayers.Count; i++)
        {
            var rndPlayer = CachedPlayer.AllPlayers[rnd.Next(0, CachedPlayer.AllPlayers.Count)].PlayerControl;
            CachedPlayer.AllPlayers[i].PlayerControl.setLook(rndPlayer.Data.PlayerName,
                rndPlayer.Data.DefaultOutfit.ColorId, rndPlayer.Data.DefaultOutfit.HatId,
                rndPlayer.Data.DefaultOutfit.VisorId, rndPlayer.Data.DefaultOutfit.SkinId,
                rndPlayer.Data.DefaultOutfit.PetId);
        }
    }

    /*public static void LoverSendChat(PlayerControl player, string text, bool isSend = false)
    {
        if (!isSend)
        {
            LoveChatPatch.LoverChat.AddChat(player, text);
            return;
        }

        if (isSend)
        {
            text = Regex.Replace(text, "<.*?>", string.Empty);
            var messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.LoverSendChat, SendOption.Reliable);
            messageWriter.Write(player.PlayerId);
            messageWriter.Write(text);
            AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
            LoveChatPatch.LoverChat.AddChat(PlayerControl.LocalPlayer, text);
        }
    }*/
}