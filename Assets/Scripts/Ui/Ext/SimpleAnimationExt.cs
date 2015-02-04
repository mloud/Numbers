using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class SimpleAnimationExt : MonoBehaviour 
{
    [SerializeField]
    AnimationClip clip;

    [SerializeField]
    string sfx;


    private void Awake()
    {
        if (animation == null)
        {
            gameObject.AddComponent<Animation>();
        }

  
        animation.clip = clip;
        animation.AddClip(clip, "flyin");
        animation.playAutomatically = false;


    }

    public void RunIn(float time)
    {
        StartCoroutine(RunInCoroutine(time));   
    }


    private IEnumerator RunInCoroutine(float time)
    {
        if (time > 0)
            yield return new WaitForSeconds(time);

        if (!string.IsNullOrEmpty(sfx))
            App.Instance.Sound.PlayEffect(sfx);

        animation.Play("flyin");
    }

}
