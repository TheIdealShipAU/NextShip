namespace NextShip.Api.Interfaces;

public interface INextEvent
{
    public string EventName { get; set; }
    
    public int Id { get; set; }

    public void OnRegister(IEventManager eventManager);

    public void OnUnRegister(IEventManager eventManager);

    public void Call(INextEvent @event);
}