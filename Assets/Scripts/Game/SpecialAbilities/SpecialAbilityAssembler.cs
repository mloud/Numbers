using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SpecialAbilityAssembler 
{

    public static IEnumerator AssembleCoroutine(List<SpecialSlot> slots, float duration)
    {
        List<Vector3> dstPos = new List<Vector3>(slots.Count);
        slots.ForEach(x => dstPos.Add(x.transform.position));

        List<Vector3> startPos = new List<Vector3>(slots.Count);

        float y = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - 1;
        
        slots.ForEach((SpecialSlot slot) =>
        {
            startPos.Add(new Vector3(slot.transform.position.x, y, slot.transform.position.z));

            slot.gameObject.SetActive(true);
        });


        float t = 0;
        float startTime = Time.time;
        while (true)
        {
            t = Mathf.Clamp01((Time.time - startTime) / duration);

            for (int i = 0; i < slots.Count; ++i)
            {
                slots[i].transform.position = startPos[i] + (dstPos[i] - startPos[i]) * t;
            }

            if (t >= 1)
                break;
            else
                yield return 0;
        }
    }
}
