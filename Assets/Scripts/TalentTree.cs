using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TalentTree : MonoBehaviour
{
    [SerializeField]
    public List<Unit.Talent> talents;
    public event Action<Unit.Skill> onTalentUnlocked;

    Node<Unit.Talent>[] branches;
    TalentView talentViewPrefab;
    Transform content;

    [Inject]
    public void Construct(TalentView talentViewPrefab)
    {
        this.talentViewPrefab = talentViewPrefab;
    }

    private void Awake()
    {
        content = GetComponent<ScrollRect>().content;
        InitTalents();
    }

    private void InitTalents()
    {
        var simpleStrike = new Unit.SimpleStrikeTalent(5);
        var improvedSimpleStrike = new Unit.ImprovedSimpleStrikeTalent(5);
        var healing = new Unit.HealingTalent(5);
        var challengeWeakness = new Unit.ChallengeTalent_Weakness(10);

        branches = new Node<Unit.Talent>[3];
        branches[0] =
            new Node<Unit.Talent>(simpleStrike)
            {
                    new Node<Unit.Talent>(improvedSimpleStrike),
                    new Node<Unit.Talent>(healing)
                    {
                        challengeWeakness
                    }
            };
    }

    private void DisplayTree()
    {
        branches[0].ConstructTree(talentViewPrefab, content, 150);
    }

    private void Start()
    {
        ConstructTree(150);
    }


    private void ConstructTree(int squareSize)
    {
        branches[0].ForEach_BreadthFirst((node) =>
        {

            (int x, int y) = node.coordinates;

            int halfChildrenCount = Mathf.CeilToInt(node.parent.children.Count / 2f);

            view.anchoredPosition =
                squareSize * ((node.parent is null)
                              ? new Vector2(x, y)
                              : new Vector2(x - halfChildrenCount, y));


            var talent = node.Value;
            foreach (var item in node.children)
            {
                var childTalent = item.Value;

                GateLineRenderer gateLineRenderer = GameObject.Instantiate(gateLinePrefab, content);

                // gateLineRenderer.SetLine()

                talent.level.onLevelUp += (level) =>
                {
                    float fill = level / childTalent.gateLevel;
                    gateLineRenderer.SetFillValue(fill);
                };
            }
        });
    }
}
