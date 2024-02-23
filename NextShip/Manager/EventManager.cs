using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

public class EventManager : IEventManager
{
    public static readonly EventManager _eventManager = Main._Service.Get<EventManager>();

    private readonly HashSet<IEventListener> EventListeners = [];

    private readonly List<INextEvent> RegisterEvents = [];

    private FastListener _listener;

    public FastListener GetFastListener() => _listener ??= new FastListener();

    public void CallEvent<T>(T @event) where T : INextEvent
    {
        foreach (var _event in RegisterEvents.FindAll(n => n.EventName == @event.EventName && n is T))
            @event.Call(_event);
    }
    
    public void RegisterEvent(INextEvent @event)
    {
        @event.OnRegister(this);
        RegisterEvents.Add(@event);
    }

    public void UnRegisterEvent(INextEvent @event)
    {
        @event.OnUnRegister(this);
        RegisterEvents.Remove(@event);
    }

    public void RegisterListener(IEventListener listener)
    {
        EventListeners.Add(listener);
    }

    public void UnRegisterListener(IEventListener listener)
    {
        EventListeners.Remove(listener);
    }

    public void CallEvent(INextEvent @event)
    {
        foreach (var _event in RegisterEvents.FindAll(n => n.EventName == @event.EventName))
            @event.Call(_event);
    }

    public void CallListener(string name)
    {
        EventListeners.Do(n => n.On(name));
    }
    
    public void CallListener(string name, object[] Instances)
    {
        EventListeners.Do(n => n.On(name, Instances));
    }

    public void CallListener(INextEvent @event)
    {
        EventListeners.Do(n => n.On(@event));
    }

    public bool TryGetEvent(string eventName, out INextEvent @event)
    {
        if (RegisterEvents.Any(n => n.EventName == eventName))
        {
            @event = RegisterEvents.FirstOrDefault(n => n.EventName == eventName);
            return true;
        }

        @event = null;
        return false;
    }

    public T GetEvent<T>() where T : INextEvent
    {
        return (T)RegisterEvents.FirstOrDefault(n => n is T);
    }

    public INextEvent GetEvent(Type type)
    {
        return RegisterEvents.FirstOrDefault(n => n.GetType() == type);
    }
}

public class FastListener
{
    private readonly List<Action<string, object[]>> AllListener = [];
    
    public void Register(Action<string, object[]> action)
    {
       AllListener.Add(action);
    }

    public void UnRegister(Action<string, object[]> action)
    {
        AllListener.Remove(action);
    }

    public void Call(string name, object[] instances = null)
    {
        AllListener.Do(n => n.Invoke(name, instances));
    }
}