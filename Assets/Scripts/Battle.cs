using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Zenject;
using System.Collections.Generic;

[JsonObjectAttribute]
public class Battle : MonoBehaviour
{
    [JsonPropertyAttribute] private double totalPower;
    [JsonPropertyAttribute] private float progress;

    private bool isBattleOngoing;

    public event Action<double> onBattleProgressUpdated;
    public event Action onPlayerWon;
    public event Action onPlayerLost;
    public event Action onReadyToStart;

    private Hero hero;
    private Team mobTeam, heroTeam;

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        StartBattle();
    }

    [Inject]
    public void Construct(Hero hero, TeamCreator teamCreator, DungeonFloor dungeonFloor)
    {

        teamCreator.onMobTeamCreated += CacheMobTeam;
        teamCreator.onHeroTeamCreated += CacheHeroTeam;
        teamCreator.onTeamsReady += StartBattle;

        onPlayerWon += () => hero.expiriense.Gain(dungeonFloor.floorNumber);

        this.hero = hero;
    }

    public void CacheHeroTeam(Team heroTeam) => this.heroTeam = heroTeam;
    public void CacheMobTeam(Team mobTeam) => this.mobTeam = mobTeam;

    private void Start()
    {
        onReadyToStart?.Invoke();
    }


    private void Update()
    {
        if (!isBattleOngoing)
            return;

        float delta = Time.deltaTime;

        heroTeam.SimulateUnits(delta);
        mobTeam.SimulateUnits(delta);

        UpdateBattleProgress();
    }

    private void StartBattle()
    {
        isBattleOngoing = true;
    }

    private void StopBattle()
    {
        isBattleOngoing = false;
    }


    private void UpdateBattleProgress()
    {
        totalPower = heroTeam.totalPower + mobTeam.totalPower;
        progress = (float)(heroTeam.totalPower / totalPower);

        onBattleProgressUpdated?.Invoke(progress);

        if (progress == 1.0)
        {
            StopBattle();
            onPlayerWon?.Invoke();
        }
        else if (progress == 0.0)
        {
            StopBattle();
            onPlayerLost?.Invoke();
        }
    }
}
