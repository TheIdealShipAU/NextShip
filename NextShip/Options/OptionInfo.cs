using System.Collections.Generic;
using NextShip.Api.Bases;
using NextShip.Api.Interfaces;

namespace NextShip.Options;

public class OptionInfo(
    string Name,
    string stringId,
    int Id,
    OptionBase optionBase,
    int Hierarchy = 0,
    IOptionInfo Parent = null,
    HashSet<IOptionInfo> Children = null)
    : IOptionInfo
{
    public string stringId { get; } = stringId;

    public bool enable { get; set; }
    public string optionName { get; } = Name;
    public int optionId { get; } = Id;
    public int hierarchy { get; } = Hierarchy;
    public IOptionInfo parent { get; set; } = Parent;
    public HashSet<IOptionInfo> children { get; set; } = Children;
    public OptionBase option { get; } = optionBase;

    public void setParent(OptionInfo optionInfo)
    {
        parent = optionInfo;
    }

    public void AddChildren(OptionInfo optionInfo)
    {
        children.Add(optionInfo);
    }

    public void RemoveChildren(OptionInfo optionInfo)
    {
        children.Remove(optionInfo);
    }
}