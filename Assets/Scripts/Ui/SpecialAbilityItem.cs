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
        public Text CountText;

        [SerializeField]
        public Button buyButton;

        [SerializeField]
        public Button useButton;

        [SerializeField]
        public Transform buttonContainer;

        private Button mainButton;

        [SerializeField]
        private Image selectedImage;


        public Action<SpecialAbilityItem> OnClickAction;
        public Action<SpecialAbilityItem> OnBuyAction;
        public Action<SpecialAbilityItem> OnUseAction;


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
                mainButton.interactable = !disabled;
            }
        }

       
        private bool selected;

        private bool disabled;


        private void Awake()
        {
            buyButton.onClick.AddListener(OnBuyClick);
            useButton.onClick.AddListener(OnUseClick);

        }

        public void SetToImageOnlyMode()
        {
            LevelText.gameObject.SetActive(false);
            CountText.gameObject.transform.parent.gameObject.SetActive(false);
            selectedImage.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
            mainButton.onClick.RemoveAllListeners();
        }

        public void SetToLockedMode()
        {
            LevelText.gameObject.SetActive(true);
            selectedImage.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
            CountText.transform.parent.gameObject.SetActive(false);
            Disabled = true;
        }

        public void SetToUnlockMode(bool canUse)
        {
            LevelText.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(!canUse);
            useButton.gameObject.SetActive(canUse);
            CountText.transform.parent.gameObject.SetActive(true);
            Disabled = false;
        }
       
        public void InjectUIVisual(GameObject uiVisual)
        {
            uiVisual.transform.SetParent(buttonContainer.transform);
            uiVisual.transform.localPosition = Vector3.zero;
            uiVisual.transform.localScale = Vector3.one;

            mainButton = uiVisual.GetComponentInChildren<Button>();

            Core.Dbg.Assert(mainButton != null, "SpecialAbilityItem.InjectVisual - no button found");


            mainButton.onClick.AddListener(OnClick);
        }
   
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

        
        private void OnClick()
        {
            if (OnClickAction != null)
                OnClickAction(this);
        }

        private void OnBuyClick()
        {
            if (OnBuyAction != null)
                OnBuyAction(this);
        }

        private void OnUseClick()
        {
            Selected = !Selected;

            if (OnUseAction != null)
                OnUseAction(this);
        }
    }
}
