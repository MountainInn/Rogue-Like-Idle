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

    public TalentTree()
    {

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
