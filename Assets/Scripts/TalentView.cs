using UnityEngine;
using UnityEditor;
using TMPro;

public class TalentView : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Text gateLevelText;

    private User.Talent talent;
    private Valueable talentPoints;

    [Inject]
    public void Construct(Vault vault)
    {
        this.talentPoints = vault.talentPoints;
    }

    private void Awake()
    {
        button.onClick.AddListener(()=>BuyTalent());
    }

    public void SetTalent(Unit.Talent talent)
    {
        this.talent = talent;
        talent.onPriceUpdated += (_price) => UpdateInteractable(talentPoints, _price);

        talentPoints.onChange += (_points, _) => UpdateInteractable(_points, talent.price);

    }

    private void UpdateInteractable(double talentPoints, double talentPrice)
    {
        button.interactable = (talentPoints >= talentPrice);
    }

    private void BuyTalent()
    {
        talent.LevelUp();
    }
}
