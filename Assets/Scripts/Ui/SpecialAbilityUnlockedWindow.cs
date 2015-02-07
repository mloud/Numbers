using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpecialAbilityUnlockedWindow : Window 
{
    [SerializeField]
    private Text abilityName;

    [SerializeField]
    private Transform containerTransform;

    private Param Parameters { get; set; }

    public class Param
    {
        public Action OnClose;
        public SpecialAbilityDb.SpecialAbility Ability;
    }

    protected override void OnInit(object param)
    {
        Parameters = param as Param;

        abilityName.text = Parameters.Ability.Name;


        var abilityItem = SpecialAbilityFactory.CreateUiItem(Parameters.Ability.Name);
        abilityItem.Ability = Parameters.Ability;
        abilityItem.transform.SetParent(containerTransform);
        abilityItem.transform.localScale = Vector3.one;
        abilityItem.Selected = false;

        abilityItem.LevelText.gameObject.SetActive(false);
    }


    public void OnMenuClick()
    {
        App.Instance.WindowManager.CloseWindow(Name);

        if (Parameters.OnClose != null)
           Parameters.OnClose();
    }
}
