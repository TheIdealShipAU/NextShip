using HarmonyLib;

namespace NextShip.Api.Utils;

public static class TaskUtils
{
    public static void StartTask(IEnumerable<Action> actions)
    {
        actions.Do(StartTask);
    }

    public static void StartTask(Action action)
    {
        var task = new Task(action);
        task.Start();
        Info($"TaskUtils : {action.Method.Name}");
    }
}