using System;
using UnityEngine;
using Zenject;
using TMPro;

public class DungeonFloorView :MonoBehaviour
{
    [SerializeField]
    MeshRenderer
        currentFloor,
        gradient;

    private Animator animator;
    private int switchFloorsTrigger;

    public event Action onFloorsSwitchAnimationHalf;
    public event Action onFloorsSwitchAnimationEnd;

    [Inject]
    public void Construct(Battle battle)
    {
        battle.onPlayerWon += SwitchFloors;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        switchFloorsTrigger = Animator.StringToHash("SwitchFloors");
    }

    public void SwitchFloors()
    {
        animator.SetTrigger(switchFloorsTrigger);
    }

    private void OnFloorsSwitchedHalf()
    {
        onFloorsSwitchAnimationHalf?.Invoke();
    }

    private void OnFloorsSwitched()
    {
        onFloorsSwitchAnimationEnd?.Invoke();
    }
}
