using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public IEnumerator RunCoroutine(IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);
    }

    public IEnumerator RunCoroutine(IEnumerator coroutine, float time)
    {
        yield return new WaitForSeconds(time);

        yield return StartCoroutine(coroutine);
    }

}
