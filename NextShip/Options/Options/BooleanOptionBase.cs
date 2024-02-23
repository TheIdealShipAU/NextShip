using NextShip.Api.Interfaces;

namespace NextShip.Options.Options;

public class BooleanOptionBase(
    bool enableTranslation,
    OptionBehaviour optionBehaviour,
    IOptionInfo optionInfo,
    string title) : StringOptionBase(enableTranslation, optionBehaviour, optionInfo, title)
{
    private static readonly string[] BooleanValueSelection = { "false", "true" };
    public bool BooleanValue;


    public override BooleanOptionBase GetBase()
    {
        return this;
    }
}