using UnityEngine;

namespace NextShip.Api.UI;

public class NextUI : IDisposable
{
    public int IntId;

    public TaskStateEnum State;

    public NextUI()
    {
        State = TaskStateEnum.None;
        IntId = NextUIManager.Instance.AllCount;
        NextUIManager.Instance.Add(this);
    }

    public string UIName { get; set; } = "NoName";

    public void Dispose()
    {
        State = TaskStateEnum.Completed;
    }

    public static NextUI StartUI(GameObject gameObject)
    {
        return new NextUI();
    }

    public void Update()
    {
    }
}