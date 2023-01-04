using UnityEngine;
using Zenject;
using TMPro;

public class FloorNumberView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI floorNumberText;
    private DungeonFloor dungeonFloor;

    [Inject]
    public void Construct(DungeonFloor dungeonFloor)
    {
        this.dungeonFloor = dungeonFloor;
        dungeonFloor.onFloorNumberUp += DisplayFloorNumber;
    }

    private void Start()
    {
        DisplayFloorNumber(dungeonFloor.floorNumber);
    }
    private void DisplayFloorNumber(uint number)
    {
        floorNumberText.text = $"Floor {number}".PadRight(10);
    }
}
