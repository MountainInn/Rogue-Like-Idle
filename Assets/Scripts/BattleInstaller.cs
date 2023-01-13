using UnityEngine;
using Zenject;

public class BattleInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<Unit>()
            .FromMethod(CreateHeroUnit)
            .WhenInjectedInto<Hero>()
            .NonLazy();
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
        Container
            .Bind<MobView>()
            .FromResource("MobView")
            .AsSingle();
        Container
            .Bind<TalentView>()
            .FromResource("TalentView")
            .AsSingle();
    }

    private Unit CreateHeroUnit()
    {
        return new Unit(10,10,null);
    }
}
