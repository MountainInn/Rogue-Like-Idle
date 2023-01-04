using System;
using System.Collections.Generic;
using MountainInn;
using Newtonsoft.Json;

[JsonObjectAttribute]
public partial class Unit
{
    [JsonPropertyAttribute] public string name;
    [JsonPropertyAttribute] public Level level;
    [JsonPropertyAttribute] public double power
    {
        get => _power;
        set
        {
            _power = value;
            onPowerChanged?.Invoke(_power);
        }
    }

    public bool isAlive {get; private set;}
    private double _power;
    [JsonPropertyAttribute] public Stat defense, attack;

    [JsonPropertyAttribute] public Team enemyTeam;
    [JsonPropertyAttribute] public Unit target;

    [JsonPropertyAttribute] protected List<Skill> activeSkills;
    public event Action<Unit> onUnitDied;
    public event Action<double> onPowerChanged;

    public Unit()
    {
        activeSkills = new List<Skill>();
        this.isAlive = true;
    }
    public Unit(double baseAttack,double baseDefense,  List<Skill> activeSkills = null)
    {
        this.activeSkills = activeSkills ?? new List<Skill>();
        this.defense = new Stat(baseDefense);
        this.attack = new Stat(baseAttack);
        this.isAlive = true;
    }
    public Unit(string name, double baseAttack, double baseDefense, List<Skill> activeSkills = null) : this(baseDefense, baseAttack, activeSkills)
    {
        this.name = name;
    }

    public void Attack(float delta)
    {
        target.power -= attack * delta;
    }
    public void Defend(float delta)
    {
        power += defense * delta;
    }
    public void UseSkills(float delta)
    {
        activeSkills.ForEach(skill => skill.Tick(delta));
    }

    public void PrepareToFight()
    {
        power = defense + attack;
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
            isAlive = false;
            onUnitDied?.Invoke(this);
        }
    }
}
