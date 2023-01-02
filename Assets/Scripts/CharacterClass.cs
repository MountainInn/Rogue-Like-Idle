using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CharacterClass
{
    Descendant<Unit.Skill> skills;

    public CharacterClass()
    {

        // TalentTree talentTree = new TalentTree()
        // {
        //     new SimpleStrike(50),
        //     ( new Healing(50), typeof(SimpleStrike), 20 ),
        // };
    }
}

public class TalentTree : IEnumerable
{
    List<Unit.Skill> skills;
    public event Action<Unit.Skill> onTalentUnlocked;

    public TalentTree()
    {
       
    }

    public IEnumerator GetEnumerator()
    {
        return skills.GetEnumerator();
    }

    public void Add(Unit.Skill rootSkill)
    {
        skills.Add(rootSkill);
    }

    public void Add(Unit.Skill descendant, Type ancestorType, int requiredLevel)
    {
        Add(descendant);

        var ancestor = skills.Single(skill => skill.GetType() == ancestorType);

        if (ancestor == null)
            throw new System.Exception($"Ancestor of type {ancestorType} not found in TalentTree.");

        ancestor.onLevelUp += CheckRequirement;

        void CheckRequirement(int level)
        {
            if (level >= requiredLevel)
            {
                onTalentUnlocked?.Invoke(descendant);
                ancestor.onLevelUp -= CheckRequirement;
            }
        }
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
