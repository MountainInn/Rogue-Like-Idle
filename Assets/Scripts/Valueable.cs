using System;
using Newtonsoft.Json;

[JsonObject]
public class Valueable
{
    public event Action<double, double> onChange;

    [JsonProperty]
    protected double _value;

    public TalentPoints(){}
   
    public double Value
    {
        get => _value;
        set
        {
            _value = value;

            onChange?.Invoke(_value, value);
        }
    }

    public bool IsEnough(double price)
    {
        return Value >= price;
    }

    static public implicit operator double (TalentPoints points) => points._value;
    static public TalentPoints operator + (TalentPoints talentPoints, double value) => talentPoints._value += value;
    static public TalentPoints operator - (TalentPoints talentPoints, double value) => talentPoints._value -= value;
}
