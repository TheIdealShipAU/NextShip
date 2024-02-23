using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

public class NextPatchManager : IPatchManager
{
    private readonly List<Harmony> _HarmonyS = [];

    private readonly HashSet<MethodInfo> _Patches = [];
    public Harmony RootHarmony { get; private set; }

    public Harmony Create(string id)
    {
        if (_HarmonyS.Exists(n => n.Id == id))
            return _HarmonyS.First(n => n.Id == id);

        var Harmony = new Harmony(id);
        _HarmonyS.Add(Harmony);
        return Harmony;
    }

    public void Register(Harmony harmony)
    {
        if (_HarmonyS.Exists(n => n.Id == harmony.Id))
        {
            var old = _HarmonyS.Where(n => n.Id == harmony.Id);
            old.Do(n =>
            {
                var methods = n.GetPatchedMethods();
                foreach (var method in methods) n.Unpatch(method, HarmonyPatchType.All);

                _HarmonyS.Remove(n);
            });
        }

        _HarmonyS.Add(harmony);
    }

    public void SetRoot(Harmony harmony)
    {
        RootHarmony = harmony;
    }

    public void Patch(MethodBase @base, HarmonyMethod method = null, pathType type = pathType.None)
    {
        var prefix = type == pathType.Prefix ? method : null;
        var postfix = type == pathType.Postfix ? method : null;
        var transpiler = type == pathType.Transpiler ? method : null;
        var finalizer = type == pathType.Finalizer ? method : null;
        var ilmanipulator = type == pathType.Ilmanipulator ? method : null;
        var info = RootHarmony.Patch(@base, prefix, postfix, transpiler, finalizer, ilmanipulator);
        _Patches.Add(info);
    }
}

public enum pathType
{
    None,
    Prefix,
    Postfix,
    Transpiler,
    Finalizer,
    Ilmanipulator
}