using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

[JsonObjectAttribute]
public class Team : IEnumerable<Unit>
{
    [JsonPropertyAttribute]
    public List<Unit> units {get; private set;}

    public double totalPower => units.Sum(unit => unit.power);

    public Team(List<Unit> units)
    {
        this.units = new List<Unit>();
        foreach (var item in units)
        {
            Add(item);
        }
    }
    public Team(params Unit[] units)
    {
        this.units = new List<Unit>();
        foreach (var item in units)
        {
            Add(item);
        }
    }

    public void PrepareForBattle(Team enemyTeam)
    {
        foreach ( var unit in units )
        {
            unit.enemyTeam = enemyTeam;

            unit.SelectTarget();
            unit.PrepareToFight();
        }
    }

    public void SimulateUnits(float delta)
    {
        foreach (var unit in units)
        {
            if (!unit.isAlive) continue;

            unit.Attack(delta);
            unit.Defend(delta);
            unit.UseSkills(delta);
            unit.CheckYourCondition();
        };
    }

    public void Set(List<Unit> units)
    {
        this.units = new List<Unit>();
        foreach (var item in units)
        {
            Add(item);
        }
    }

    public void Add(Unit unit)
    {
        units.Add(unit);
    }

    public IEnumerator<Unit> GetEnumerator()
    {
        return ((IEnumerable<Unit>)units).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)units).GetEnumerator();
    }
}

