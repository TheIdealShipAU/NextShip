using NextShip.Api.Bases;
using NextShip.Api.Interfaces;
using NextShip.Options.OptionValue;

namespace NextShip.Options.Options;

public class IntOptionBase(
    bool enableTranslation,
    OptionBehaviour optionBehaviour,
    IOptionInfo optionInfo,
    string title,
    IntOptionValueBase intOptionValueBase) : OptionBase(enableTranslation, optionBehaviour, optionInfo, title)
{
    public override void Increase()
    {
        intOptionValueBase.GetValue();
    }

    public override void Decrease()
    {
        intOptionValueBase.GetValue();
    }

    public override int GetInt()
    {
        return intOptionValueBase.GetValue();
    }

    public override float GetFloat()
    {
        return intOptionValueBase.GetValue();
    }

    public override string GetValueString()
    {
        return intOptionValueBase.GetValue().ToString();
    }

    public override OptionBase GetBase()
    {
        return this;
    }
}