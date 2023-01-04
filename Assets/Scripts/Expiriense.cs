using System;

public class Expiriense
{
    public event Action<double, double> onExpirienseGained;
    public event Action onMaxReached;

    Level level;
    public double Value {get; private set;}
    public double Max {get; private set;}
    Func<Level, double> maxExpirienseFunc;

    public Expiriense(Level level, Func<Level, double> maxExpirienseFunc)
    {
        this.level = level;
        this.maxExpirienseFunc = maxExpirienseFunc;
    }

    public void Gain(double value)
    {
        Value += value;

        if (Value >= Max)
        {
            Value -= Max;

            onMaxReached?.Invoke();

            level.Up();
            Max = maxExpirienseFunc(level);
        }

        onExpirienseGained?.Invoke(Value, Max);
    }
}
