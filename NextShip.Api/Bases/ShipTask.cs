namespace NextShip.Api.Bases;

public class ShipTask
{
    public readonly TaskPriority TaskPriority;
    private readonly TaskState TaskState;
    public bool Loop;
    public Func<bool> LoopEndConditions;
    public Action Task;
    public float Time;
    public bool Update;
    public Func<bool> UpdateConditions;

    public ShipTask(float time, Action task, TaskPriority taskPriority = TaskPriority.Low)
    {
        Task = task;
        Time = time;
        TaskState = new TaskState();
        Task += () => TaskState.Completed();
        TaskPriority = taskPriority;
    }

    public TaskStateEnum GetState => TaskState.Get();

    public void StartLoop(Func<bool> conditions = null)
    {
        Loop = true;
        LoopEndConditions = conditions;
        StartUpdate();
    }

    public void StopLoop()
    {
        Loop = false;
        LoopEndConditions = null;
        RemoveUpdate();
    }

    public void StartUpdate(Func<bool> conditions = null)
    {
        Update = true;
        UpdateConditions = conditions;
        Task -= () => TaskState.Completed();
    }

    public void RemoveUpdate()
    {
        Update = false;
        UpdateConditions = null;
        Task += () => TaskState.Completed();
    }
}