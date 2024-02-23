using HarmonyLib;

namespace NextShip.Api.Bases;

public class UpdateTasker
{
    public readonly List<ShipTask> Tasks = new();

    public void FixedUpdate()
    {
        if (Tasks == null) return;

        Tasks.Do(UpdateTaskTime);
        Tasks.Where(n => n.TaskPriority == TaskPriority.High).Do(StartTask);
    }

    public void LateUpdate()
    {
        Tasks.Do(StartTask);
        Tasks.Do(CheckTask);
    }

    private void StartTask(ShipTask task)
    {
        if (task.Time > 0) return;

        task.Task.Invoke();
    }

    private void CheckTask(ShipTask task)
    {
        if (task.LoopEndConditions != null && task.LoopEndConditions.Invoke()) task.StopLoop();

        if (task.UpdateConditions != null && !task.UpdateConditions.Invoke()) task.RemoveUpdate();

        if (task.GetState == TaskStateEnum.Completed) Tasks.Remove(task);
    }

    private void UpdateTaskTime(ShipTask Task)
    {
        if (Task.Time > 1)
            Task.Time -= 1;
        else
            Task.Time = 0;
    }
}