using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TalentTree : MonoBehaviour, IEnumerable
{
    [SerializeField]
    public List<Unit.Talent> talents;

    [HideInInspector]
    public List<Unit.Skill> skills;

    public event Action<Unit.Skill> onTalentUnlocked;

    List<Node<Unit.Talent>>
        branches;

    Transform
        talentViewPrefab;
    Transform
        content;

    [Inject]
    public void Construct(TalentView talentViewPrefab)
    {
        this.talentViewPrefab = talentViewPrefab;
    }

   
    private void Awake()
    {
        content = GetComponent<ScrollView>().content;

        var simpleStrike = new Unit.SimpleStrikeTalent(5);
        var improvedSimpleStrike = new Unit.ImprovedSimpleStrikeTalent(5);
        var healing = new Unit.HealingTalent(5);
        var challengeWeakness = new Unit.ChallengeTalent_Weakness(10);

        branches = new List<Node<Unit.Talent>>(3);

        branches[0] =
            new Node<Unit.Talent>(simpleStrike)
            {
                new Node<Unit.Talent>(improvedSimpleStrike),
                new Node<Unit.Talent>(healing)
                {
                    challengeWeakness
                }
            };
        branches[0].ConstructTree();

    }

    private void DisplayTree()
    {
        branches[0].ForEach(
            (node)=>
            {
                var newTalentView = GameObject.Instantiate(talentViewPrefab, content);

                node.SetViewPosition(newTalentView, 15);

            });

    }

    public IEnumerator GetEnumerator()
    {
        return skills.GetEnumerator();
    }

    public void Add(Unit.Skill rootSkill)
    {
        skills.Add(rootSkill);
    }

    public void Add(Unit.Skill descendant, Type ancestorType, int requiredLevel)
    {
        Add(descendant);

        var ancestor = skills.Single(skill => skill.GetType() == ancestorType);

        if (ancestor == null)
            throw new System.Exception($"Ancestor of type {ancestorType} not found in TalentTree.");

        ancestor.onLevelUp += CheckRequirement;

        void CheckRequirement(int level)
        {
            if (level >= requiredLevel)
            {
                onTalentUnlocked?.Invoke(descendant);
                ancestor.onLevelUp -= CheckRequirement;
            }
        }
    }
}
