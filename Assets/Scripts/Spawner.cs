using System;
using System.Collections.Generic;
using Zenject;

public class Spawner
{
    private DungeonFloor dungeonFloor;

    public event Action<List<Unit>> onMobsSpawned;
    public event Action<Unit> onOneNewMobSpawned;

    [Inject]
    public void Construct(DungeonFloor dungeonFloor, Battle battle)
    {
        this.dungeonFloor = dungeonFloor;
        dungeonFloor.onFloorNumberUp += SpawnNewMobs;

        battle.onReadyToStart += () => SpawnNewMobs(dungeonFloor.floorNumber);
    }

    public void SpawnNewMobs(uint floorNumber)
    {
        List<Unit> mobs = new List<Unit>();

        mobs.Add(SpawnMob(floorNumber));

        onMobsSpawned?.Invoke(mobs);
    }

    public Unit SpawnMob(uint floorNumber)
    {
        Unit mob = new Unit(5,5,null);

        onOneNewMobSpawned?.Invoke(mob);

        return mob;
    }
}
