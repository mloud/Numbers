using UnityEngine;
using System.Collections;
using System;

public class SpecialSlot : MonoBehaviour 
{
    [SerializeField]
    private TextMesh counterTextMesh;

    [SerializeField]
    private Transform diableOverlayTransform;

    public LevelDb.LevelDef.SpecialAbility SpecialAbility { get; set; }

    public Action<SpecialSlot> OnClick;


    public float Timer
    {
        get { return timer ; }

        set 
        {
            timer = value;

            float progress = Mathf.Clamp01(timer / SpecialAbility.RechargeTime);

            diableOverlayTransform.localScale = new Vector3(1, progress, 1);
        }
    }

    public bool Toggled
    { 
        get { return toggled; }

        set { toggled = value; Animator.SetTrigger( value ? "Select" : "Unselect"); }
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

    public void SetToMax()
    {
        Timer = SpecialAbility.RechargeTime;
        Toggled = false;
    }
    

    public void SetIcon(GameObject icon)
    {
        icon.transform.SetParent(transform);
        icon.transform.localPosition = Vector3.zero;
    }


    void OnMouseUp()
    {
        if (OnClick != null)
            OnClick(this);
    }
}
