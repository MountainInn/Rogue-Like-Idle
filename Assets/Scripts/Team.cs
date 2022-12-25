using System.Collections.Generic;
using System.Linq;

public class Team
{
    List<Unit> units;

    public int totalPower => units.Sum(unit => unit.power);

    public void SimulateUnits()
    {
        units.ForEach(unit => unit.Tick());
    }
}
