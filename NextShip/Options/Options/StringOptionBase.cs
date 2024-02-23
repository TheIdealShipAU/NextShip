using NextShip.Api.Bases;
using NextShip.Api.Interfaces;
using NextShip.Options.OptionValue;

namespace NextShip.Options.Options;

public class StringOptionBase(
    bool enableTranslation,
    OptionBehaviour optionBehaviour,
    IOptionInfo optionInfo,
    string title) : OptionBase(enableTranslation, optionBehaviour, optionInfo, title)
{
    public IntOptionValueBase IntOptionValueBase;
    public string[] Selection;

    public bool StringTranslation;
    public string StringValue;

    public override int GetInt()
    {
        return IntOptionValueBase.GetValue();
    }

    public override float GetFloat()
    {
        return IntOptionValueBase.GetValue();
    }


    public override string GetValueString()
    {
        return Selection[IntOptionValueBase.GetValue()];
    }

    public override void Increase()
    {
        IntOptionValueBase.increase();
    }

    public override void Decrease()
    {
        IntOptionValueBase.decrease();
    }

    public override StringOptionBase GetBase()
    {
        return this;
    }
}