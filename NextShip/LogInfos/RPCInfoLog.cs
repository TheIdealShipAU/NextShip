using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Hazel;
using InnerNet;

namespace NextShip.LogInfos;

public class RPCInfoLog
{
    [HarmonyPatch]
    internal static class RPCInfoLogPatch
    {
        private static IEnumerable<Type> InnerNetObjectTypes { get; } = typeof(InnerNetObject).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(InnerNetObject))).ToList();

        public static IEnumerable<MethodBase> TargetMethods()
        {
            return InnerNetObjectTypes.Select(x => x.GetMethod(nameof(InnerNetObject.HandleRpc), AccessTools.allDeclared)).Where(m => m != null)!;
        }

        public static void Prefix(InnerNetObject __instance, [HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            => Info($"RPCHandle {nameof(__instance)} CallId{callId} Length:{reader.Length} Pos:{reader.Position} Tag:{reader.Tag}", "RPCInfoLog");
    }
}