using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

[JsonObjectAttribute]
public class Team
{
    [JsonPropertyAttribute]
    public List<Unit> units;

    public double totalPower => units.Sum(unit => unit.power);

    public void PrepareForBattle(Team enemyTeam)
    {
        units.ForEach(unit =>
        {
            unit.enemyTeam = enemyTeam;

            unit.SelectTarget();
        });
    }

    public void SimulateUnits(float delta)
    {
        units.ForEach(unit =>
        {
            unit.Attack(delta);
            unit.Defend(delta);
            unit.UseSkills(delta);
            unit.CheckYourCondition();
        });
    }
}

