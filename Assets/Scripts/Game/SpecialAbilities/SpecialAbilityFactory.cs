using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



public static class SpecialAbilityFactory
{
    private const string path = "Prefabs/SpecialAbilities/";
    private const string TimeFreeze = path + "TimeFreeze";

    public static GameObject CreateIcon(string name)
    {
        GameObject iconGo = null;

        if (name == SpecialAbilityDef.TimeFreeze)
        {
            iconGo = GameObject.Instantiate(Resources.Load<GameObject>(TimeFreeze)) as GameObject;
        }
     

        return iconGo;
    }

    
    public static SpecialAbility Create(string name)
    {
        SpecialAbility ability = null;


        if (name == SpecialAbilityDef.TimeFreeze)
        {
            ability = new TimeFreezeSpecialAbility();              
        }
      
        UnityEngine.Debug.Log(ability == null ? "Cannot create SpecialAbility :" + name : "");


        return ability;
    }
}
