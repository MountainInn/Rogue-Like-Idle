using System;

public class Level
{
    int Value;
    public event Action<int> onLevelUp;

    public Level(int value)
    {
        Value = value;
    }

    static public implicit operator int(Level level) => level.Value;

    public void Up()
    {
        Value++;
        onLevelUp?.Invoke(Value);
    }
}
