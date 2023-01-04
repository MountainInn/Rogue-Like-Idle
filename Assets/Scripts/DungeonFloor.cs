using System;
using Newtonsoft.Json;
using Zenject;

[JsonObjectAttribute]
public class DungeonFloor
{
    [JsonPropertyAttribute] public uint floorNumber;

    private DungeonFloorView view;
    private Battle battle;


    public event Action<uint> onFloorNumberUp;

    [Inject]
    public DungeonFloor(DungeonFloorView view, Battle battle)
    {
        this.view = view;
        view.onFloorsSwitchAnimationHalf += UpFloorNumber;

        floorNumber = 1;
    }

    public void UpFloorNumber()
    {
        floorNumber++;
        onFloorNumberUp?.Invoke(floorNumber);
    }

}
