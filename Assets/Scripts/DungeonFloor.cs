using System.Collections;
using UnityEngine;

public class DungeonFloor : MonoBehaviour
{
    Team
        playerTeam,
        mobTeam;

    double
        powerCapacity;

    public float fillAmount => playerTeam.totalPower / (playerTeam.totalPower + mobTeam.totalPower);

    private void Update()
    {

    }

    private void Battle()
    {

    }
}
