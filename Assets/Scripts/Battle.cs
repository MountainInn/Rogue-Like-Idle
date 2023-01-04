using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Zenject;
using System.Collections.Generic;

[JsonObjectAttribute]
public class Battle : MonoBehaviour
{
    [JsonPropertyAttribute] private Team playerTeam, mobTeam;

    [JsonPropertyAttribute] private double totalPower;
    [JsonPropertyAttribute] private float progress;

    private bool isBattleOngoing;

    public event Action<Team> onMobTeamInitialized;
    public event Action<double> onBattleProgressUpdated;
    public event Action onPlayerWon;
    public event Action onPlayerLost;
    public event Action onReadyToStart;


    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        StartBattle();
    }

    [Inject]
    public void Construct(Spawner spawner, Hero hero, DungeonFloor dungeonFloor)
    {
        spawner.onMobsSpawned += InitMobTeam;
        spawner.onMobsSpawned += (_) => InitHeroTeam();
        spawner.onMobsSpawned += (_) => PrepareBothTeamsForBattle();
        spawner.onMobsSpawned += (_) => StartBattle();

        onPlayerWon += () => hero.expiriense.Gain(dungeonFloor.floorNumber);


        playerTeam = new Team(hero.unit);
    }

    private void Start()
    {
        onReadyToStart?.Invoke();
    }

    private void InitMobTeam(List<Unit> mobs)
    {
        mobTeam = new Team(mobs);
        onMobTeamInitialized?.Invoke(mobTeam);
    }
    private void InitHeroTeam()
    {
        playerTeam = new Team(hero.unit);
    }

    private void Update()
    {
        if (!isBattleOngoing)
            return;

        float delta = Time.deltaTime;

        playerTeam.SimulateUnits(delta);
        mobTeam.SimulateUnits(delta);

        UpdateBattleProgress();
    }

    private void PrepareBothTeamsForBattle()
    {
        playerTeam.PrepareForBattle(mobTeam);
        mobTeam.PrepareForBattle(playerTeam);
    }

    private void StartBattle()
    {
        isBattleOngoing = true;
    }


    private void UpdateBattleProgress()
    {
        totalPower = playerTeam.totalPower + mobTeam.totalPower;
        progress = (float)(playerTeam.totalPower / totalPower);

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

    private void StopBattle()
    {
        isBattleOngoing = false;
    }
}
