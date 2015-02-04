using UnityEngine;
using System.Collections;

public abstract class SpecialAbilityVisual : MonoBehaviour 
{
    protected SpecialAbility SpecialAbility { get; private set; }

    public void AbilityStarted(SpecialAbility ability)
    {
        SpecialAbility = ability;

        ability.AbilityStarted += OnSpecialAbilityStarted;
        ability.AbilityFinished += OnSpecialAbilityFinished;
    }

    protected abstract void OnUpdate();

	void Update ()
    {
        OnUpdate();
    }


    protected virtual void OnSpecialAbilityStarted(SpecialAbility ability)
    {
        
    }

    protected virtual void OnSpecialAbilityFinished(SpecialAbility ability)
    {
        SpecialAbility = null;
    }

}
