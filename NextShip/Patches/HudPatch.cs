using HarmonyLib;
using NextShip.NextEvents;

namespace NextShip.Patches;
/*
[HarmonyPatch]
public static class LoveChatPatch
{
    public static ChatController LoverChat;

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    [HarmonyPostfix]
    public static void LoverChat_Postfix(HudManager __instance)
    {
        if (!__instance.Chat.isActiveAndEnabled)
                __instance.Chat.SetVisible(true);
        if (MeetingHud.Instance)
            LoverChat.transform.position = __instance.Chat.transform.position + new Vector3(-0.5f, 0, 0);
        else
            LoverChat.transform.position = __instance.Chat.transform.position;
    }

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    [HarmonyPrefix]
    public static bool LoverSenndChat_Prefix(ChatController __instance)
    {
        if (__instance.name != "LoveChat") return true;
        var text = __instance.freeChatField.textArea.text;
        RPCProcedure.LoverSendChat(PlayerControl.LocalPlayer, text, true);
        __instance.freeChatField.textArea.Clear();
        return false;
    }

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
    [HarmonyPrefix]
    public static bool AddChat_Prefix(ChatController __instance, [HarmonyArgument(0)] PlayerControl sourcePlayer,
        [HarmonyArgument(1)] string chatText)
    {
        if (!CheckForModification(__instance, sourcePlayer, chatText)) return true;
        CreateAddChat(__instance, sourcePlayer, chatText);
        return false;
    }

    private static void CreateChat(HudManager __instance, ChatController chat, string chatName, bool Visible,
        Vector3 chatPos)
    {
        if (chat != null)
        {
            if (chat.transform.position != __instance.Chat.transform.position + chatPos)
                chat.transform.position = __instance.Chat.transform.position + chatPos;
            if (chat.gameObject.active != Visible) chat.SetVisible(Visible);
            return;
        }

        chat = Object.Instantiate(__instance.Chat);
        chat.name = chatName;
        chat.transform.SetParent(__instance.gameObject.transform);
        chat.SetVisible(Visible);
        chat.transform.position = __instance.Chat.transform.position + chatPos;
    }

    private static void CreateAddChat(ChatController __instance, PlayerControl sourcePlayer, string chatText)
    {
        var data = PlayerControl.LocalPlayer.Data;
        var data2 = sourcePlayer.Data;
        var chatBubble = __instance.chatBubblePool.Get<ChatBubble>();
        var transform = chatBubble.transform;
        transform.SetParent(__instance.scroller.Inner);
        transform.localScale = Vector3.one;
        var flag = sourcePlayer == PlayerControl.LocalPlayer;
        if (flag)
            chatBubble.SetRight();
        else
            chatBubble.SetLeft();
        var didVote = MeetingHud.Instance && MeetingHud.Instance.DidVote(sourcePlayer.PlayerId);
        chatBubble.SetCosmetics(data2);
        __instance.SetChatBubbleName(chatBubble, data2, data2.IsDead, didVote, PlayerNameColor.Get(data2));
        if (DataManager.Settings.Multiplayer.CensorChat) chatText = BlockedWords.CensorWords(chatText);
        chatBubble.SetText(chatText);
        chatBubble.AlignChildren();
        __instance.AlignAllBubbles();
        if (!flag)
            SoundManager.Instance.PlaySound(__instance.messageSound, false).pitch = 0.5f + sourcePlayer.PlayerId / 15f;
    }

    private static bool CheckForModification(ChatController __instance, PlayerControl sourcePlayer, string chatText)
    {
        if (__instance.name == "LoverChat") return true;

        return false;
    }
}*/

[Harmony]
public static class OnHudManagerUpdatePatch
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update)), HarmonyPostfix]
    public static void OnHudManagerUpdate(HudManager __instance)
    {
        _eventManager.CallEvent(new HudManagerUpdateEvent(__instance));
    }
}

