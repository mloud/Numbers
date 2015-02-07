using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;


public class SpecialAbilityEditorController
{
    public Action<LevelDb.LevelDef> OnNumLimitsChange;
    public Action<LevelDb.LevelDef> OnProbabilityChange;

    private Db.Db Db { get; set; }

    public SpecialAbilityEditorController()
    {
        Db = (Resources.Load("Prefabs/Db/Db") as GameObject).GetComponent<Db.Db>();
    }

    public SpecialAbilityDb SpecialAbilityDb { get { return Db.SpecialAbilityDb;  } }

    public void AddNewAbility()
    {
        SpecialAbilityDb.SpecialAbility ability = new SpecialAbilityDb.SpecialAbility();

        ability.Name = "ability";

        SpecialAbilityDb.SpecialAbilities.Add(ability);
    }

    public void RemoveSpecialAbility( SpecialAbilityDb.SpecialAbility ability)
    {
        SpecialAbilityDb.SpecialAbilities.Remove(ability);
    }

    public void Save()
    {
        EditorUtility.SetDirty(Db.gameObject);
        EditorApplication.SaveAssets();
    }

}