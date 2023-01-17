using System;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class TeamCreator : MonoBehaviour
{
    [JsonPropertyAttribute] private Team heroTeam, mobTeam;
    public event Action<Team> onMobTeamCreated;
    public event Action<Team> onHeroTeamCreated;
    public event Action onTeamsReady;

    [Inject]
    public void Construct(Hero hero, Spawner spawner)
    {
        mobTeam = new Team();
        onMobTeamCreated?.Invoke(mobTeam);

        heroTeam = new Team();
        heroTeam.SetUnits(hero.unit);
        onHeroTeamCreated?.Invoke(heroTeam);

        spawner.onMobsSpawned += mobTeam.SetUnits;
        spawner.onMobsSpawned += (_) => PrepareBothTeamsForBattle();
    }

    private void PrepareBothTeamsForBattle()
    {
        heroTeam.PrepareForBattle(mobTeam);
        mobTeam.PrepareForBattle(heroTeam);

        onTeamsReady?.Invoke();
    }
}
