using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpecialAbilityDb : MonoBehaviour
{
    [System.Serializable]
    public class SpecialAbility
    {
        public float RechargeTime;
        public float Duration;
        public string Name;
        public int AvailableForLevel;
    }

    public List<SpecialAbility> SpecialAbilities;
}