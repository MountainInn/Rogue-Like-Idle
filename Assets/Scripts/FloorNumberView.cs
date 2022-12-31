using UnityEngine;
using Zenject;
using TMPro;

public class FloorNumberView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI floorNumberText;

    [Inject]
    public void Construct(DungeonFloor dungeonFloor)
    {
        dungeonFloor.onFloorNumberUp += DisplayFloorNumber;
    }

    private void DisplayFloorNumber(uint number)
    {
        floorNumberText.text = $"Floor {number}".PadRight(10);
    }
}
