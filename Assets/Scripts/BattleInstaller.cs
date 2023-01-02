using UnityEngine;
using Zenject;

public class BattleInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<Hero>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container
            .Bind<Battle>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container
            .Bind<DungeonFloor>()
            .FromNew()
            .AsSingle();
        Container
            .Bind<DungeonFloorView>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container
            .Bind<Spawner>()
            .FromNew()
            .AsSingle();
    }
}
