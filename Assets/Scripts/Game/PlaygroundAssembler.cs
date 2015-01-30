using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlaygroundAssembler
{
    private const float MTime = 0.05f;


    public abstract class AssemblerBase
    {
        public abstract IEnumerator AssembleCoroutine(List<Slot> slots);
    }

    public class AssemblerFromLeft : AssemblerBase
    {

        public override IEnumerator AssembleCoroutine(List<Slot> slots)
        {
            var x1 = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
            var x2 = Camera.main.ViewportToWorldPoint(new Vector2(1,0)).x;
            var screenWorldWidth = x2 - x1;

            var dstPositions = new List<Vector3>(slots.Count);
            var startPositions = new List<Vector3>(slots.Count);
            
            slots.ForEach(delegate(Slot slot)
            {
                dstPositions.Add(slot.transform.position);

                var pos = slot.transform.position;
                pos.x -= screenWorldWidth;

                slot.transform.position = pos;

                startPositions.Add(pos);

                slot.gameObject.SetActive(true);
            });


            float t = 0;
            float startTime = Time.time;
            float duration = 0.5f;
            while (true)
            {
                t = Mathf.Clamp01((Time.time - startTime) / duration);
                int index = 0;
                slots.ForEach(delegate(Slot slot)
                {
                    var pos = startPositions[index] + (dstPositions[index] - startPositions[index]) * t;
                    slot.transform.position = pos;
                    index++;
                });

                if (t == 1)
                {
                    break;
                }
                else
                {
                    yield return 0;
                }
            }


        }
    }



    public static IEnumerator AssembleRandomCoroutine(MonoBehaviour owner, List<Slot> slots)
    {
        var assemblers = new List<AssemblerBase>()
        {
            new AssemblerFromLeft()
        };

        yield return owner.StartCoroutine(assemblers[UnityEngine.Random.Range(0, assemblers.Count - 1)].AssembleCoroutine(slots));
    }

    
}