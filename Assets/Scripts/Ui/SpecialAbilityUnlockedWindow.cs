using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpecialAbilityUnlockedWindow : Window 
{
    [SerializeField]
    private Text abilityName;

    [SerializeField]
    private Text description;


    [SerializeField]
    private Transform containerTransform;

    [SerializeField]
    private Button cancelButton;

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
        description.text = Parameters.Ability.Description;

        cancelButton.onClick.AddListener(OnCancelClick);

        var abilityItem = SpecialAbilityFactory.CreateUiItem(Parameters.Ability.Name);
        abilityItem.Ability = Parameters.Ability;
        abilityItem.transform.SetParent(containerTransform);
        abilityItem.transform.localScale = Vector3.one;
        abilityItem.SetToImageOnlyMode();

        abilityItem.LevelText.gameObject.SetActive(false);

        
    }


    public void OnCancelClick()
    {
        App.Instance.WindowManager.CloseWindow(Name);

        if (Parameters.OnClose != null)
           Parameters.OnClose();
    }
}
