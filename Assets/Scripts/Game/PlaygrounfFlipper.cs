using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlaygroundFlipper
{
    private const float MTime = 0.05f;


    public abstract class FlipperBase
    {
        public abstract IEnumerator FlipCoroutine(LevelDb.LevelDef level,  List<CircleController> circles);
    }

    public class FlipperAtOnce : FlipperBase
    {
        public override IEnumerator FlipCoroutine(LevelDb.LevelDef level, List<CircleController> circles)
        {
            float duration = 0.3f;
            circles.ForEach(x => x.Flip(false, duration, 0));
            yield return new WaitForSeconds(duration);
        }
    }

    public class FlipperFromTop : FlipperBase
    {
        public override IEnumerator FlipCoroutine(LevelDb.LevelDef level, List<CircleController> circles)
        {
            float duration = 0.3f;

            for (int i = 0; i < circles.Count; ++i)
            {
                circles[i].Flip(false, duration, i * MTime);
            }
            yield return new WaitForSeconds(circles.Count * MTime);
        }
    }


    public class FlipperSnakeX : FlipperBase
    {
        public override IEnumerator FlipCoroutine(LevelDb.LevelDef level, List<CircleController> circles)
        {
            float duration = 0.3f;

            for (int i = 0; i < circles.Count; ++i)
            {
                int y = i / level.Cols;
                int x = i - (y * level.Cols);

                float delay = y % 2 == 1 ? (y * level.Cols + level.Cols - x) * MTime : i * MTime;

                circles[i].Flip(false, duration, delay);
            }
            yield return new WaitForSeconds(circles.Count * MTime);
        }
    }


    public static IEnumerator FlipRandomCoroutine(MonoBehaviour owner,  LevelDb.LevelDef level, List<CircleController> circles)
    {
        var flippers = new List<FlipperBase>()
        {
            new FlipperAtOnce(),
            new FlipperFromTop(),
            new FlipperSnakeX()
        };

        yield return owner.StartCoroutine(flippers[UnityEngine.Random.Range(0, flippers.Count - 1)].FlipCoroutine(level, circles));
    }

    
}