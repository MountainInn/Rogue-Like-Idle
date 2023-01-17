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
            _power = Math.Max(0, value);

            onPowerChanged?.Invoke(_power);

            if (_power == 0)
            {
                onUnitDied?.Invoke(this);
            }
        }
    }

    public bool isAlive => power > 0;
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
    }
    public Unit(double baseAttack,double baseDefense,  List<Skill> activeSkills = null)
    {
        this.activeSkills = activeSkills ?? new List<Skill>();
        this.defense = new Stat(baseDefense);
        this.attack = new Stat(baseAttack);
        InitializePower();
    }
    public Unit(string name, double baseAttack, double baseDefense, List<Skill> activeSkills = null) : this(baseDefense, baseAttack, activeSkills)
    {
        this.name = name;
    }

    public void Fight(float delta)
    {
        SelectTarget();

        if (target is null)
            return;

        Attack(delta);
        Defend(delta);
        UseSkills(delta);
    }
    private void Attack(float delta)
    {
        target.power -= attack * delta;
    }
    private void Defend(float delta)
    {
        power += defense * delta;
    }
    private void UseSkills(float delta)
    {
        activeSkills.ForEach(skill => skill.Tick(delta));
    }

    public void InitializePower()
    {
        _power = defense + attack;
    }

    public void SelectTarget()
    {
        if (enemyTeam.units.Count >= 1)
            target = enemyTeam.units[0];
        else
            target = null;
    }
}
