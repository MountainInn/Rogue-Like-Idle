using System.Collections.Generic;
using System.Linq;
using MountainInn;

public class MobDataBase
{
    List<Unit> mainPool = new List<Unit>();
    SortedList<uint, List<Unit>> levelPools = new SortedList<uint, List<Unit>>();

    uint lastActivatedFloor = 0;

    public MobDataBase()
    {
        levelPools = new SortedList<uint, List<Unit>>()
        {
            {1,
             new List<Unit>(){
                 Mob.Adept,
             }},
            {10,
             new List<Unit>(){
                 Mob.GiantCrab,
             }}
        };
    }

    public Unit GetRandomMob()
    {
        return mainPool.GetRandom();
    }

    public void UpdateMainPool(uint floorNumber)
    {
        while (floorNumber > lastActivatedFloor)
        {
            var levelPool =
                levelPools.FirstOrDefault(
                    pair =>
                    pair.Key > lastActivatedFloor &&
                    pair.Key <= floorNumber);

            if (levelPool.Value is null)
                return;

            lastActivatedFloor = levelPool.Key;

            mainPool = mainPool.Concat(levelPool.Value).ToList();
        }
    }
}
