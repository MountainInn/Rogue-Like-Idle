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
    private Team mobTeam;

    [Inject]
    public void Construct(MobView mobViewPrefab, Spawner spawner, TeamCreator teamCreator)
    {
        this.mobViewPrefab = mobViewPrefab;

        spawner.onOneNewMobSpawned += (mob)=>
        {
            AddMobView(mob);
            mob.onUnitDied += RemoveMobView;
        };

        teamCreator.onMobTeamCreated += (mobTeam) =>
        {
            this.mobTeam = mobTeam;

            mobTeam.onPowerChanged += UpdateTotalPowerText;
        };
    }

    private void Awake()
    {
        mobViews = new List<MobView>();
    }

    private void UpdateTotalPowerText(double power)
    {
        totalPowerText.text = $"Total\nPower\n{power.BeautifulFormat()}";
    }

    public void AddMobView(Unit mob)
    {
        Debug.Log("Added");
        MobView view = Instantiate(mobViewPrefab, mobVerticalGroup);

        view.SetMob(mob);

        mobViews.Add(view);
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
