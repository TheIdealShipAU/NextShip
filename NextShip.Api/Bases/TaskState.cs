#nullable enable
namespace NextShip.Api.Bases;

public sealed class TaskState
{
    private Action? Action;
    private TaskStateEnum StateEnum = TaskStateEnum.None;

    public TaskStateEnum Get()
    {
        return StateEnum;
    }

    public void set(TaskStateEnum @enum)
    {
        StateEnum = @enum;
    }

    public void Completed()
    {
        StateEnum = TaskStateEnum.Completed;
        Action?.Invoke();
    }

    public void AddAction(Action action)
    {
        Action += action;
    }

    public void RemoverAction(Action action)
    {
        if (Action == null) return;
        Action -= action;
    }
}