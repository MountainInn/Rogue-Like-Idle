using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Zenject;

public class HeroPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI
        levelText,
        attackText,
        defenseText,
        powerText;

    [SerializeField]
    private Image
        expirienseBar;

    private Hero hero;

    [Inject]
    public void Construct(Hero hero)
    {
        hero.onHeroInitialized += SubHeroToView;
    }

    private void SubHeroToView(Hero hero)
    {
        this.hero = hero;
        hero.onHeroInitialized -= SubHeroToView;

        hero.expiriense.onExpirienseGained += UpdateExpirienseBar;
        hero.unit.defense.onResultChanged += UpdateDefenseText;
        hero.unit.attack.onResultChanged += UpdateAttackText;
        hero.unit.onPowerChanged += UpdatePowerText;
        hero.level.onLevelUp += UpdateLevelText;

        UpdateExpirienseBar(hero.expiriense.Value, hero.expiriense.Max);
        UpdateDefenseText(hero.unit.defense);
        UpdateAttackText(hero.unit.attack);
        UpdatePowerText(hero.unit.power);
        UpdateLevelText(hero.level);
    }

    private void UpdatePowerText(double obj)
    {
        powerText.text = $"Power: {obj.BeautifulFormat()}";
    }

    private void UpdateLevelText(int obj)
    {
        levelText.text = $"Level: {obj}";
    }

    private void UpdateAttackText(double obj)
    {
        attackText.text = $"Attack: {obj}";
    }

    private void UpdateDefenseText(double obj)
    {
        defenseText.text = $"Defense: {obj}";
    }

    private void UpdateExpirienseBar(double value, double max)
    {
        expirienseBar.fillAmount = (float)( value / max );
    }
}
