using System;
using UnityEngine;
using Zenject;

public class BattleProgressView : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer floorRenderer;
    private Material material;
    private int progressProperty;

    [Inject]
    public void Construct(Battle battle, DungeonFloorView dungeonFloorView)
    {
        battle.onBattleProgressUpdated += UpdateBattleProgress;

        dungeonFloorView.onFloorsSwitchAnimationHalf += ResetBattleProgress;
    }

    private void ResetBattleProgress()
    {
        material.SetFloat(progressProperty, 0);
    }
    private void UpdateBattleProgress(double progress)
    {
        material.SetFloat(progressProperty, (float) progress);
    }

    private void Awake()
    {
        material = floorRenderer.material;
        progressProperty = Shader.PropertyToID("_Progress");
    }
}
