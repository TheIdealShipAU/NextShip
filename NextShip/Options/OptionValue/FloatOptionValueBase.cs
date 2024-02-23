using NextShip.Api.Bases;

namespace NextShip.Options.OptionValue;

public class FloatOptionValueBase(float defaultValue, float min, float step, float max)
    : OptionValueBase<float>(defaultValue, min, step, max)
{
    public override void decrease()
    {
        if (Value - Step < Min) return;
        Value -= Step;
    }

    public override float GetValue()
    {
        return Value;
    }

    public override void increase()
    {
        if (Value + Step > Max) return;
        Value += Step;
    }
}