using System;
using System.Collections.Generic;


public class CharacterClass
{
    Descendant<Unit.Skill> skills;

    public CharacterClass()
    {

    }
}

public class Descendant<T>
{
    public T Value {get; private set;}
    public Descendant<T> ancestor {get; private set;}
    public List<Descendant<T>> descendants {get; private set;}

    public event Action<Descendant<T>> onUnlocked;

    bool unlocked;

    public Descendant(T Value)
    {
        this.Value = Value;
        unlocked = true;
    }

    public Descendant(T Value, Descendant<T> ancestor)
    {
        this.Value = Value;
        ancestor.descendants.Add(this);
    }

    public Descendant(T Value, Type ancestorType)
    {
        this.Value = Value;
    }

    public void Descend()
    {
        unlocked = true;
        onUnlocked?.Invoke(this);
    }
}
