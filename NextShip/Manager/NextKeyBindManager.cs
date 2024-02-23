using System.Collections.Generic;
using System.Linq;
using NextShip.Api.Bases;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

public class NextKeyBindManager : IKeyBindManager
{
    private readonly List<NKeyBind> _keyBinds = [];

    private int CurrentPriority;

    private int CurrentMode;

    private int MaxPriority;

    private int MaxMode;

    public NextKeyBindManager(NextManager _nextManager)
    {
        _nextManager.OnUpdate += OnUpdate;
    }
    
    public void AddBind(NKeyBind bind)
    {
        _keyBinds.Add(bind);
        if (bind.Priority > MaxPriority)
        {
            MaxPriority = bind.Priority;
        }

        if (bind.Mode > MaxMode)
        {
            MaxMode = bind.Mode;
        }
    }

    public void RemoveBind(NKeyBind bind)
    {
        _keyBinds.Remove(bind);
get:
        if (_keyBinds.All(n => n.Priority != MaxPriority) && MaxPriority > 0)
        {
            MaxPriority--;
            goto get;
        }
        
        if (_keyBinds.All(n => n.Mode != MaxMode) && MaxMode > 1)
        {
            MaxMode--;
            goto get;
        }
    }

    private void OnUpdate()
    {
        Start:
        var binds = getNKeyBinds();
        foreach (var varBind in binds)
        {
            if (!InputKeyUtils.GetKeysDown(varBind.keys)) continue;
            StartBind(varBind);
            break;
        }

        if (CanGet())
        {
            goto Start;
        }
    }

    private List<NKeyBind> getNKeyBinds()
    {
        return _keyBinds.FindAll(n => n.Mode == CurrentMode).GetSortList((x, y) => sort(x.Priority, x.Priority)).FindAll(n => n.Priority == CurrentPriority);

        int sort(int x, int y)
        {
            return x.CompareTo(y);
        }
    }

    private bool CanGet()
    {
        if (CurrentMode < MaxMode)
        {
            CurrentMode++;
            return true;
        }

        if (CurrentPriority < MaxPriority)
        {
            CurrentPriority++;
            return true;
        }

        return false;
    }

    private void StartBind(NKeyBind bind)
    {
        bind._Action.Invoke();
    }
}