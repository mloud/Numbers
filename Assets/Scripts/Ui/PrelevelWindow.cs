using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PrelevelWindow : Window 
{
    [SerializeField]
    public Button BtnPlay;

    [SerializeField]
    public Button BtnCancel;

    [SerializeField]
    private Transform specialAbilitiesTransform;

    [SerializeField]
    Text LevelNameText;

    [SerializeField]
    Text GoalValueText;


	private Param Parameters { get; set; }

    private int AbilitiesSelected { get; set; }

    public List<SpecialAbilityDb.SpecialAbility> SelectedAbilities { get; set; }

	public class Param
	{
		public Action OnPlayClick;
        public LevelDb.LevelDef Level;
	}

	protected override void OnInit(object param)
	{
		Parameters = param as Param;

        LevelNameText.text = "Level " + Parameters.Level.Name;
        GoalValueText.text = Parameters.Level.Score.ToString();

        InitSpecialAbilities();
    }


    private void InitSpecialAbilities()
    {
        SelectedAbilities = new List<SpecialAbilityDb.SpecialAbility>();

        AbilitiesSelected = 0;
        // todo
        var abilities = App.Instance.Db.SpecialAbilityDb.SpecialAbilities;


        abilities.Sort( (x,y) =>
        {
            return x.AvailableForLevel < y.AvailableForLevel ? -1 : 1;
        });
        
        
        foreach(var ab in abilities)
        {
            var abilityItem = SpecialAbilityFactory.CreateUiItem(ab.Name);
            abilityItem.Ability = ab;
            abilityItem.transform.SetParent(specialAbilitiesTransform);
            abilityItem.transform.localScale = Vector3.one;
            abilityItem.Selected = false;
            abilityItem.OnClickAction += OnAbilityInfoClick;


            var abilityStatus = App.Instance.Player.AbilitiesStatus.GetStatus(ab.Name);
        
            // locked
            if (abilityStatus == null)
            {
                abilityItem.Disabled = true;
                abilityItem.SetToLockedMode();
            }
            // unlocked
            else
            {
                
                abilityItem.SetToUnlockMode(abilityStatus.Count > 0);
                abilityItem.CountText.text = abilityStatus.Count.ToString();

              
                abilityItem.OnBuyAction += OnAbilityBuyClick;
                abilityItem.OnUseAction += OnAbilityUseClick;
            }
        }

    }


    private void OnAbilityBuyClick(Ui.SpecialAbilityItem item)
    {
        App.Instance.WindowManager.OpenWindow(WindowDef.SpecialAbilityPreLevelInfo, new SpecialAbilityPreLevelInfoWindow.Param() { Ability = item.Ability, ShowBuyButton = false });
    }

    private void OnAbilityInfoClick(Ui.SpecialAbilityItem item)
    {
        App.Instance.WindowManager.OpenWindow(WindowDef.SpecialAbilityPreLevelInfo, new SpecialAbilityPreLevelInfoWindow.Param() { Ability = item.Ability, ShowBuyButton = false });
    }

    private void OnAbilityUseClick(Ui.SpecialAbilityItem item)
    {
        if (item.Selected)
        {
            AbilitiesSelected++;

            if (AbilitiesSelected > Parameters.Level.MaxAbilities)
            {
                AbilitiesSelected--;
                item.Selected = false;

                // show info dialog
                App.Instance.WindowManager.OpenWindow(WindowDef.Info, new InfoWindow.Param() { Title = "", Text = "You can use " + Parameters.Level.MaxAbilities + " special abilities in this level" });
            }
            else
            {
                SelectedAbilities.Add(item.Ability);
            }
        }
        else
        {
            AbilitiesSelected--;
            SelectedAbilities.Remove(item.Ability);
        }
    }


	public void OnPlayClick()
	{
     	
	}


}
