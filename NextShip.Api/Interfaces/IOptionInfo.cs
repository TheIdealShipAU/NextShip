namespace NextShip.Api.Interfaces;

public interface IOptionInfo
{
    public bool enable { get; set; }
    public string optionName { get; }
    public int optionId { get; }
    public int hierarchy { get; }
    public IOptionInfo parent { get; set; }
    public HashSet<IOptionInfo> children { get; set; }
    public OptionBase option { get; }

    public void setParent(IOptionInfo optionInfo)
    {
        parent = optionInfo;
    }

    public void AddChildren(IOptionInfo optionInfo)
    {
        children.Add(optionInfo);
    }

    public void RemoveChildren(IOptionInfo optionInfo)
    {
        children.Remove(optionInfo);
    }
}