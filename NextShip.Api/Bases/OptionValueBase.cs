namespace NextShip.Api.Bases;

public abstract class OptionValueBase<T>(
    T defaultValue,
    T min,
    T step,
    T max)
{
    public readonly T Max = max;
    public readonly T Min = min;
    public readonly T Step = step;
    public T DefaultValue = defaultValue;
    public T Value = defaultValue;

    public abstract T GetValue();
    public abstract void increase();
    public abstract void decrease();
}