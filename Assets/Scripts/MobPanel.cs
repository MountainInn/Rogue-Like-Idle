using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using TMPro;
using System;

public class MobPanel : MonoBehaviour
{
    [SerializeField]
    private Transform mobVerticalGroup;
    [SerializeField]
    private TextMeshProUGUI totalPowerText;

    private MobView mobViewPrefab;
    private List<MobView> mobViews;

    private Spawner spawner;


    [Inject]
    public void Construct(MobView mobViewPrefab, Spawner spawner, Battle battle)
    {
        this.mobViewPrefab = mobViewPrefab;

        this.spawner = spawner;
        spawner.onOneNewMobSpawned += AddMob;
        spawner.onOneNewMobSpawned += SubscribeOnRemove;

        battle.onMobTeamInitialized += SubscribeToUpdateTotalPowerText;
    }

    private void SubscribeToUpdateTotalPowerText(Team team)
    {
        team.units.ForEach((unit) => unit.onPowerChanged += Upd);

        void Upd(double _)
        {
            UpdateTotalPowerText(team);
        }
    }

    private void UpdateTotalPowerText(Team team)
    {
        totalPowerText.text = $"Total\nPower\n{team.totalPower.BeautifulFormat()}";
    }

    public void Awake()
    {
        mobViews = new List<MobView>();
    }

    public void AddMob(Unit mob)
    {
        MobView view = Instantiate(mobViewPrefab, mobVerticalGroup);

        view.SetMob(mob);

        mobViews.Add(view);
    }

    private void SubscribeOnRemove(Unit mob)
    {
        mob.onUnitDied += RemoveMobView;
    }

    public void RemoveMobView(Unit mob)
    {
        MobView mobView = mobViews.First((view)=> view.mob == mob);

        mobViews.Remove(mobView);

        Destroy(mobView.gameObject);
    }

    public void Clear()
    {
        mobViews.ForEach(view => Destroy(view.gameObject));

        mobViews.Clear();
    }
}
