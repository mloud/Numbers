using UnityEngine;
using System.Collections;
using System;

public class SpecialSlot : MonoBehaviour 
{
    [SerializeField]
    private TextMesh counterTextMesh;

    [SerializeField]
    private Transform diableOverlayTransform;

    // ID of ability
    public SpecialAbilityDb.SpecialAbility SpecialAbilityDef { get; set; }

    public Action<SpecialSlot> OnClick;

    // SpeciaAbility visual
    public  SpecialAbilityVisual SpecialAbilityVisual { get; private set; }


    public float Timer
    {
        get { return timer ; }

        set 
        {
            timer = value;

            float progress = Mathf.Clamp01(timer / SpecialAbilityDef.RechargeTime);

            diableOverlayTransform.localScale = new Vector3(1, progress, 1);
        }
    }

    public bool Toggled
    { 
        get { return toggled; }

        set 
        { 
            toggled = value;
            bool isIdle = Animator.GetCurrentAnimatorStateInfo(0).IsName("idle");
            // todo  
            if (value && isIdle)
                Animator.SetTrigger( "Select");
            else if (!value && !isIdle)
                Animator.SetTrigger("Unselect");
        }
    }
    private Animator Animator { get; set; }

    private bool toggled;

    private float timer;

    void Awake()
    {
        Animator = GetComponent<Animator>();
    }

	void Update ()
    {
        float timer = Timer;
        Timer = Mathf.Max(0, (Timer - Time.deltaTime));

        if (Timer != timer && Timer == 0)
            Toggled = true;
    }


    public bool CanBeUsed()
    {
        return Timer == 0;
    }

    public void StartRecharging()
    {
        Timer = SpecialAbilityDef.RechargeTime;
        Toggled = false;
    }
    

    public void SetVisual(SpecialAbilityVisual specialAbilityVisual)
    {
        SpecialAbilityVisual = specialAbilityVisual;
        specialAbilityVisual.transform.SetParent(transform);
        specialAbilityVisual.transform.localPosition = Vector3.zero;
    }

    public void OnAbilityFinished(SpecialAbility ability)
    {
        StartRecharging();
    }

    void OnMouseUp()
    {
        if (OnClick != null)
            OnClick(this);
    }
}
