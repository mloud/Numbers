using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



public static class SpecialAbilityFactory
{
    private const string pathVisual = "Prefabs/SpecialAbilities/Visual/";
    private const string pathUi = "Prefabs/SpecialAbilities/Ui/";


    public static Ui.SpecialAbilityItem CreateUiItem(string name)
    {
        var prefab = Resources.Load<GameObject>(pathUi + name);
        
        Core.Dbg.Assert(prefab != null, "SpecialAbilityFactyCreateUiItem() " + name + " no prefab at: " + pathUi + name + " found");

        var ability = (GameObject.Instantiate(prefab) as GameObject).GetComponent<Ui.SpecialAbilityItem>();

        Core.Dbg.Assert(ability != null, "SpecialAbilityFactyCreateUiItem() no SpecialAbilityItem script found on" + name);

        return ability;
    }

    public static SpecialAbilityVisual CreateVisual(string name)
    {
        var prefab = Resources.Load<GameObject>(pathVisual + name);

        Core.Dbg.Assert(prefab != null, "SpecialAbilityFacty.CreateVisual() " + name + " no prefab at: " + pathVisual + name + " found");

        var visual = (GameObject.Instantiate(prefab) as GameObject).GetComponent<SpecialAbilityVisual>();

        Core.Dbg.Assert(visual != null, "SpecialAbilityFactory.CreateVisual() no SpecialAbilityVisual script found on" + name);

        return visual;
    }

    
    public static SpecialAbility Create(SpecialAbilityDb.SpecialAbility def, SpecialAbilityVisual visual)
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
        else if (def.Name == SpecialAbilityDef.Refill)
        {
            ability = new RefillSpecialAbility(def, visual);
        }

        UnityEngine.Debug.Log(ability == null ? "Cannot create SpecialAbility :" + def.Name : "");


        return ability;
    }
}
