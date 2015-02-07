using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


namespace Ui
{
    public class SpecialAbilityItem : MonoBehaviour
    {
        [SerializeField]
        public Text LevelText;
        
        [SerializeField]
        private Button button;


        [SerializeField]
        private Image selectedImage;
 
     
        public Action<SpecialAbilityItem> OnClickAction;

        public bool Selected 
        {
            get { return selected;  }
            set
            {
                selected = value;
                selectedImage.gameObject.SetActive(selected);
            }
        }

        public bool Disabled
        {
            get { return disabled; }
            set
            {
                disabled = value;
                button.interactable = !disabled;
            }
        }

       
        private bool selected;

        private bool disabled;

       public  SpecialAbilityDb.SpecialAbility Ability 
        {
            get { return ability;  }
            set
            {
                ability = value;

                LevelText.text = "Level " + Db.DbUtils.GetLevelByOrder(ability.AvailableForLevel).Name;
            }
        }

        SpecialAbilityDb.SpecialAbility ability;

        private void Awake()
        {

            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            Selected = !Selected;

            if (OnClickAction != null)
                OnClickAction(this);
        }
    

    }
}
