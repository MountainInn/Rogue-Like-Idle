using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Newtonsoft.Json;
using System;

[JsonObjectAttribute]
public class Team
{
    [JsonPropertyAttribute]
    public List<Unit> units {get; private set;}

    public double totalPower => units.Sum(unit => unit.power);

    public event Action<double> onPowerChanged;

    public Team(){}

    public void SetUnits(params Unit[] units)
    {
        SetUnits(units.ToList());
    }
    public void SetUnits(List<Unit> units)
    {
        this.units = units;

        units.ForEach((u) =>
        {
            u.onUnitDied += RemoveUnit;
        });
    }

    private void RemoveUnit(Unit unit)
    {
        units.Remove(unit);

        unit = null;
    }

    public void PrepareForBattle(Team enemyTeam)
    {
        foreach ( var unit in units )
        {
            unit.enemyTeam = enemyTeam;

            unit.InitializePower();
        }
    }

    public void SimulateUnits(float delta)
    {
        units.ForEach((u) => u.Fight(delta));

        onPowerChanged?.Invoke(totalPower);
    }
}
