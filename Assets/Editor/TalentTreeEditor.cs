using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(TalentTree))]
public class TalentTreeEditor : Editor
{
    private SerializedProperty _talents;

    private List<Type> talentTypes;
    private string[] talentTypeNames;

    private int n_talentId;
    private int n_ancestorId;
    private int n_levelReq;
    bool shouldAddTalent;

    TalentTree tree;
    string[] talentNames;
    string[] ancestorNames;

    public void OnEnable()
    {
        tree = (TalentTree)target;
        UpdateTalentNames();

        _talents = serializedObject.FindProperty("talents");

        talentTypes =
            (typeof(Unit.Talent).Assembly)
            .GetTypes()
            .Where(type => type.BaseType == typeof(Unit.Talent))
            .ToList();

        talentTypeNames = talentTypes.Select(type => type.Name).ToArray();
    }

    private void UpdateTalentNames()
    {
        talentNames = tree.talents.Select(tal => tal.name).ToArray();
        ancestorNames = talentNames.Prepend("null").ToArray();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        LayoutBody();
        

        bool hasModifiedProperties = serializedObject.hasModifiedProperties;

        serializedObject.ApplyModifiedProperties();

        if (shouldAddTalent)
            AddTalent();
    }

    private void LayoutBody()
    {
        shouldAddTalent = false;

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.PrefixLabel("Talent");
        n_talentId = EditorGUILayout.Popup(n_talentId, talentTypeNames);
        EditorGUILayout.EndVertical();

        if (talentNames.Count() > 0)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PrefixLabel("Ancestor");
            n_ancestorId = EditorGUILayout.Popup(n_ancestorId, ancestorNames);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.PrefixLabel("Level Req");
            n_levelReq = EditorGUILayout.IntField(n_levelReq);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add"))
        {
            shouldAddTalent = true;
        }

        GUILayout.Space(15);


        for (int i = tree.talents.Count -1; i >= 0; i--)
        {
            var item = tree.talents[i];

            EditorGUILayout.BeginHorizontal();

            int previousTalentType = talentTypes.IndexOf(item.GetType());
            int newTalentType = EditorGUILayout.Popup(previousTalentType, talentTypeNames);

            int previousAncestor = (item.ancestor is null) ? 0 : talentTypes.IndexOf(item.ancestor.GetType());
            int newAncestor = EditorGUILayout.Popup(previousAncestor, ancestorNames);

            if (newTalentType != previousTalentType)
            {
                item = ConstructTalent(newTalentType);
            }

            if (newAncestor != previousAncestor && (newAncestor -1) > 0)
            {
                item.ancestor = tree.talents[newAncestor-1];
            }

            item.ancestorLevelRequirement = (uint)EditorGUILayout.IntField((int)item.ancestorLevelRequirement);

            if (GUILayout.Button("X"))
            {
                tree.talents.Remove(item);
            }

            EditorGUILayout.EndHorizontal();
        }

    }

    private void AddTalent()
    {
        Unit.Talent nTalent = ConstructTalent(n_talentId);

        if (0 <= n_ancestorId && n_ancestorId < tree.talents.Count())
        {
            nTalent.ConstructRequirement(
                tree.talents[n_ancestorId],
                (uint)n_levelReq);
        }
       
        tree.talents.Add(nTalent);

        UpdateTalentNames();
    }

    private Unit.Talent ConstructTalent(int talentTypeID)
    {
        return (Unit.Talent) talentTypes[talentTypeID].GetConstructor(new Type[]{}).Invoke(new Type[]{});
    }
}
