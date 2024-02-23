using System.Globalization;
using NextShip.Api.Bases;
using NextShip.Api.Interfaces;
using NextShip.Options.OptionValue;

namespace NextShip.Options.Options;

public class FloatOptionBase(
    bool enableTranslation,
    OptionBehaviour optionBehaviour,
    IOptionInfo optionInfo,
    string title,
    FloatOptionValueBase floatOptionValueBase) : OptionBase(enableTranslation, optionBehaviour, optionInfo, title)
{
    public override void Increase()
    {
        floatOptionValueBase.increase();
    }

    public override void Decrease()
    {
        floatOptionValueBase.decrease();
    }

    public override int GetInt()
    {
        return (int)floatOptionValueBase.GetValue();
    }

    public override float GetFloat()
    {
        return floatOptionValueBase.GetValue();
    }

    public override string GetValueString()
    {
        return floatOptionValueBase.GetValue().ToString(CultureInfo.CurrentCulture);
    }

    public override OptionBase GetBase()
    {
        return this;
    }
}