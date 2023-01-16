using System;
using UnityEngine;
using Newtonsoft.Json;
using Zenject;

[JsonObject]
public class Vault : MonoBehaviour
{
    [JsonProperty]
    public Valueable gold, talentPoints;

    private DungeonFloor dungeonFloor;

    [Inject]
    public void Construct(DungeonFloor dungeonFloor, Battle battle)
    {
        this.dungeonFloor = dungeonFloor;
        dungeonFloor.onMaxFloorChanged += EarnTalentPoints;

        battle.onPlayerWon += EarnGold;
    }

    private void EarnTalentPoints(uint maxFloorReached)
    {
        talentPoints += 1 + maxFloorReached / 100;
    }
    private void EarnGold()
    {
        gold += dungeonFloor.floorNumber * 2;
    }
}
