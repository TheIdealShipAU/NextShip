namespace NextShip.Api.Interfaces;

public interface IKeyBindManager
{
    public void AddBind(NKeyBind bind);

    public void RemoveBind(NKeyBind bind);
}