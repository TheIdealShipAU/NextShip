using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace NextShip.UI.Components;

public class UpdateTask : MonoBehaviour
{
    public static List<ShipTask> Tasks;
    public bool startd;

    public UpdateTask()
    {
        Start();
    }

    public void Start()
    {
        Tasks = new List<ShipTask>();
        startd = true;
    }

    public void FixedUpdate()
    {
        if (!startd) return;
        if (Tasks == null) return;

        Tasks.Do(UpdateTaskTime);
        Tasks.Where(n => n.Priority == ShipTask.priority.High).Do(StartTask);
    }

    public void LateUpdate()
    {
        if (!startd) return;
        if (Tasks == null) return;

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

public class ShipTask
{
    public enum priority
    {
        High = 0,
        Medium = 1,
        Low = 2
    }

    public readonly priority Priority;
    private readonly TaskState TaskState;
    public bool Loop;
    public Func<bool> LoopEndConditions;
    public Action Task;
    public float Time;
    public bool Update;
    public Func<bool> UpdateConditions;

    public ShipTask(float time, Action task, priority priority = priority.Low)
    {
        Task = task;
        Time = time;
        TaskState = new TaskState();
        Task += () => TaskState.Completed();
        Priority = priority;
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

    public void register()
    {
        UpdateTask.Tasks.Add(this);
    }
}

public class LastShipTask : ShipTask
{
    public LastShipTask(float time, Action task) : base(time, task, priority.High)
    {
    }
}