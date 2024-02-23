using HarmonyLib;

namespace NextShip.Api.Interfaces;

public interface IPatchManager
{
    public Harmony Create(string id);

    public void Register(Harmony harmony);
}