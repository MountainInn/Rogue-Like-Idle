using System;
using System.Collections.Generic;
using Zenject;

public class Spawner
{
    private DungeonFloor dungeonFloor;

    public event Action<List<Unit>> onMobsSpawned;
    public event Action<Unit> onOneNewMobSpawned;

    private MobDataBase mobDataBase;

    [Inject]
    public void Construct(DungeonFloor dungeonFloor, Battle battle)
    {
        mobDataBase = new MobDataBase();
        mobDataBase.UpdateMainPool(dungeonFloor.floorNumber);

        this.dungeonFloor = dungeonFloor;

        battle.onReadyToStart += () => SpawnNewMobs(dungeonFloor.floorNumber);

        dungeonFloor.onFloorNumberUp += mobDataBase.UpdateMainPool;
    }

    public void SpawnNewMobs(uint floorNumber)
    {
        List<Unit> mobs = new List<Unit>();

        mobs.Add(SpawnMob(floorNumber));

        onMobsSpawned?.Invoke(mobs);
    }

    public Unit SpawnMob(uint floorNumber)
    {
        Unit mob = mobDataBase.GetRandomMob();

        AddFloorPowerMult(mob, floorNumber);

        onOneNewMobSpawned?.Invoke(mob);

        return mob;
    }

    private void AddFloorPowerMult(Unit mob, uint floorNumber)
    {
        float floorPower = FloorPower(floorNumber);

        mob.attack.MultSuper(floorPower);
        mob.defense.MultSuper(floorPower);
    }

    private float FloorPower(uint floorNumber)
    {
        float tens = floorNumber / 10 + 1;

        float powerTens = MathF.Pow(tens, 2);

        float power = powerTens + floorNumber;

        return power;
    }
}
