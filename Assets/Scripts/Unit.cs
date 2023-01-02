using System;
using System.Collections.Generic;
using MountainInn;
using Newtonsoft.Json;

[JsonObjectAttribute]
public partial class Unit
{
    [JsonPropertyAttribute] public Level level;
    [JsonPropertyAttribute] public double power;
    [JsonPropertyAttribute] public Stat defense, attack;

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
        if (enemyTeam.units.Count >= 1)
            target = enemyTeam.units[0];
        else
            target = null;
    }

    public void CheckYourCondition()
    {
        if (power <= 0)
        {
            power = 0;
            onUnitDied?.Invoke(this);
        }
    }
}
