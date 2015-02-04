using UnityEngine;
using System.Collections;

public class TimeFreezeVisual : SpecialAbilityVisual 
{
    [SerializeField]
    private Transform progressTransform;

  
    private void Awake()
    {
        progressTransform.gameObject.SetActive(false);
    }

    protected override void OnSpecialAbilityStarted(SpecialAbility ability)
    {
        base.OnSpecialAbilityStarted(ability);

        progressTransform.gameObject.SetActive(true);
    }

    protected override void OnSpecialAbilityFinished(SpecialAbility ability)
    {
        base.OnSpecialAbilityFinished(ability);

        progressTransform.gameObject.SetActive(false);
    }

	protected override void OnUpdate ()
    {

        if (SpecialAbility != null)
        {
            progressTransform.localScale = new Vector3(1, 1 - (SpecialAbility as TimeFreezeSpecialAbility).GetProgress(), 1);
        }
	}
}
