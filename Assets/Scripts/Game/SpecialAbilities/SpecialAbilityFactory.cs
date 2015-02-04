using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



public static class SpecialAbilityFactory
{
    private const string path = "Prefabs/SpecialAbilities/";
    private const string TimeFreeze = path + "TimeFreeze";
    private const string Shuffle = path + "Shuffle";
    private const string AnyNumber = path + "AnyNumber";



    public static SpecialAbilityVisual CreateVisual(string name)
    {
        SpecialAbilityVisual visual = null;

        if (name == SpecialAbilityDef.TimeFreeze)
        {
            visual = (GameObject.Instantiate(Resources.Load<GameObject>(TimeFreeze)) as GameObject).GetComponent<SpecialAbilityVisual>();
        }
        else if (name == SpecialAbilityDef.Shuffle)
        {
            visual = (GameObject.Instantiate(Resources.Load<GameObject>(Shuffle)) as GameObject).GetComponent<SpecialAbilityVisual>();
        }
        else if (name == SpecialAbilityDef.AnyNumber)
        {
            visual = (GameObject.Instantiate(Resources.Load<GameObject>(AnyNumber)) as GameObject).GetComponent<SpecialAbilityVisual>();
        }

        return visual;
    }

    
    public static SpecialAbility Create(LevelDb.LevelDef.SpecialAbility def, SpecialAbilityVisual visual)
    {
        SpecialAbility ability = null;


        if (def.Name == SpecialAbilityDef.TimeFreeze)
        {
            ability = new TimeFreezeSpecialAbility(def, visual);
        }
        else if (def.Name == SpecialAbilityDef.Shuffle)
        {
            ability = new ShuffleSpecialAbility(def, visual);
        }
        else if (def.Name == SpecialAbilityDef.AnyNumber)
        {
            ability = new AnyNumberSpecialAbility(def, visual);
        }
        

        UnityEngine.Debug.Log(ability == null ? "Cannot create SpecialAbility :" + def.Name : "");


        return ability;
    }
}
