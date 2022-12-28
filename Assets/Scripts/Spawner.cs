using System;
using System.Collections.Generic;
using Zenject;

public class Spawner
{
    private DungeonFloor dungeonFloor;

    public event Action<List<Unit>> onMobsSpawned;

    [Inject]
    public void Construct(DungeonFloor dungeonFloor)
    {
        this.dungeonFloor = dungeonFloor;
        dungeonFloor.onFloorNumberUp += SpawnNewMobs;
    }

    private void SpawnNewMobs(uint floorNumber)
    {
        List<Unit> mobs = new List<Unit>();

        mobs.Add(new Unit( baseDefense: 100,  baseDamage:100));

        onMobsSpawned?.Invoke(mobs);
    }
}
