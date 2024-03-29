namespace NextShip.Options;

public class FloatOptionValue : OptionValue<float>
{
    public FloatOptionValue(float defaultValue, float min, float step, float max) : base(defaultValue, min, step, max)
    {
    }

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