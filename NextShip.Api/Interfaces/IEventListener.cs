namespace NextShip.Api.Interfaces;

public interface IEventListener
{
    public void On(string name) {}
    
    public void On(string name, object[] Instances) {}

    public void On(INextEvent @event) {}
}