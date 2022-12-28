using System;
using System.Collections.Generic;
using MountainInn;
using Newtonsoft.Json;

[JsonObjectAttribute]
public partial class Unit
{
    [JsonPropertyAttribute] public Level level;
    [JsonPropertyAttribute] public double power;

    [JsonPropertyAttribute]
    public double
        baseDefense, baseDamage,
        defense, damage;

    [JsonPropertyAttribute] public Team enemyTeam;
    [JsonPropertyAttribute] public Unit target;

    [JsonPropertyAttribute] private List<Skill> activeSkills;
    public event Action<Unit> onUnitDied;

    public Unit(double baseDefense, double baseDamage, List<Skill> activeSkills = null)
    {
        this.baseDefense = baseDefense;
        this.baseDamage = baseDamage;
        this.activeSkills = activeSkills;
    }

    public void Attack(float delta)
    {
        target.power -= damage * delta;
    }
    public void Defend(float delta)
    {
        power += defense * delta;
    }
    public void UseSkills(float delta)
    {
        activeSkills.ForEach(skill => skill.Tick(delta));
    }

    public void SelectTarget()
    {
        target = enemyTeam.units.GetRandom();
    }

    public void CheckYourCondition()
    {
        if (power <= 0)
        {
            power = 0;
            onUnitDied?.Invoke(this);
        }
    }


    abstract public partial class Skill {}
}


