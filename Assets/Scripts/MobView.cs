using UnityEngine;
using TMPro;

public class MobView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI mobName;

    public Unit mob {get; private set;}

    public void SetMob(Unit mob)
    {
        this.mob = mob;
        mobName.text = mob.name;
    }
}
