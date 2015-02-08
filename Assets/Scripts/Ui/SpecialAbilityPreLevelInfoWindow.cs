using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpecialAbilityPreLevelInfoWindow : Window
{
    [SerializeField]
    private Transform PanelTransform;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button buyButton;


    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text descriptionText;


    private Param Parameters { get; set; }

    public class Param
    {
        public Action<SpecialAbilityDb.SpecialAbility> OnBuyAction;
        public bool ShowBuyButton;
        public SpecialAbilityDb.SpecialAbility Ability;
    }


    protected override void OnInit(object param)
    {
        Parameters = param as Param;

        titleText.text = Parameters.Ability.Name;
        descriptionText.text = Parameters.Ability.Description;

        buyButton.gameObject.SetActive(Parameters.ShowBuyButton);

        if (Parameters.ShowBuyButton)
        {
            buyButton.onClick.AddListener( () => Parameters.OnBuyAction(Parameters.Ability));    
        }

        cancelButton.onClick.AddListener(() => App.Instance.WindowManager.CloseWindow(Name));

        var abilityItem = SpecialAbilityFactory.CreateUiItem(Parameters.Ability.Name);
        abilityItem.gameObject.transform.SetParent(PanelTransform);
        abilityItem.gameObject.transform.localPosition = Vector3.zero;
        abilityItem.gameObject.transform.localScale = Vector3.one;
        abilityItem.SetToImageOnlyMode();

    }
}
