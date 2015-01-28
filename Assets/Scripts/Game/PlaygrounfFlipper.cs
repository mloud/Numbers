using System;
using System.Collections.Generic;



public class PlaygroundFlipper
{
    private const float MTime = 0.05f;


    public abstract class FlipperBase
    {
        public abstract void Flip(LevelDb.LevelDef level,  List<CircleController> circles);
    }

    public class FlipperAtOnce : FlipperBase
    {
        public override void Flip(LevelDb.LevelDef level, List<CircleController> circles)
        {
            circles.ForEach(x => x.Flip(false, 0.3f, 0));
        }
    }

    public class FlipperFromTop : FlipperBase
    {
        public override void Flip(LevelDb.LevelDef level, List<CircleController> circles)
        {
            for (int i = 0; i < circles.Count; ++i)
            {
                circles[i].Flip(false, 0.3f, i * MTime);
            }
        }
    }


    public class FlipperSnakeX : FlipperBase
    {
        public override void Flip(LevelDb.LevelDef level, List<CircleController> circles)
        {
            for (int i = 0; i < circles.Count; ++i)
            {
                int y = i / level.Cols;
                int x = i - (y * level.Cols);

                float delay = y % 2 == 1 ? (y * level.Cols + level.Cols - x) * MTime : i * MTime;

                circles[i].Flip(false, 0.3f, delay);
            }
        }
    }


    public static void FlipRandom(LevelDb.LevelDef level, List<CircleController> circles)
    {
        var flippers = new List<FlipperBase>()
        {
            new FlipperAtOnce(),
            new FlipperFromTop(),
            new FlipperSnakeX()
        };

        flippers[UnityEngine.Random.Range(0, flippers.Count - 1)].Flip(level, circles);
    }

    
}