#nullable enable
namespace NextShip.Api.UI;

public class NextUIManager
{
    private static NextUIManager? _instance;


    private readonly HashSet<NextUI> UIs = new();

    private readonly Queue<NextUI> UIsQueue = new();

    public static NextUIManager Instance => _instance ??= new NextUIManager();

    public int AllCount => UIs.Count;

    public void Add(NextUI nextUI)
    {
        UIs.Add(nextUI);
    }

    public void Start(NextUI ui)
    {
        UIsQueue.Enqueue(ui);
    }

    public void Update()
    {
        var ui = UIsQueue.Dequeue();

        ui.Update();

        if (ui.State != TaskStateEnum.Completed)
            UIsQueue.Enqueue(ui);
    }
}