using NextShip.Api.Bases;

namespace NextShip.Options.OptionValue;

public class IntOptionValueBase(int defaultValue, int min, int step, int max)
    : OptionValueBase<int>(defaultValue, min, step, max)
{
    public override void decrease()
    {
        if (Value - Step < Min) return;

        Value -= Step;
    }

    public override int GetValue()
    {
        return Value;
    }

    public override void increase()
    {
        if (Value + Step > Min) return;

        Value += Step;
    }
}