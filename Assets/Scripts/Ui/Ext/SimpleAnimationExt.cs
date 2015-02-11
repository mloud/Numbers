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
    List<AnimationClip> clips;

    [SerializeField]
    string sfx;

    [SerializeField]
    bool runAutomatically = false;

    [SerializeField]
    float playAfterDelay;

    [SerializeField]
    bool hideOnStart;

    private void Awake()
    {
        if (animation == null)
        {
            gameObject.AddComponent<Animation>();
        }


        if (hideOnStart)
            gameObject.SetActive(false);
        
        if (runAutomatically)   
        {
            RunIn(playAfterDelay);
        }

      
    }

    public void RunIn(float time)
    {
       App.Instance.CoroutineManager.StartCoroutine(RunInCoroutine(time));   
    }


    private IEnumerator RunInCoroutine(float time)
    {
   

        var animsToPlay = new List<AnimationClip>();
        if (clip != null)
            animsToPlay.Add(clip);
        animsToPlay.AddRange(clips);

        
        if (time > 0)
            yield return new WaitForSeconds(time);

        gameObject.SetActive(true);

        if (!string.IsNullOrEmpty(sfx))
            App.Instance.Sound.PlayEffect(sfx);

        while (true)
        {
            animation.AddClip(animsToPlay[0], "clip");
            animation.Play("clip");
            animsToPlay.RemoveAt(0);

            if (animsToPlay.Count == 0)
                break;

            while (animation.isPlaying)
                yield return 0;
        }
    }

}
