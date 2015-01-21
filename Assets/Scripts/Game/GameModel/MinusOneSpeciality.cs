using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MinusOneSpeciality : Speciality
{
    private int Counter { get; set; }


    public override void Init(string par)
    {
        Counter = 1;

        int c;
        if (int.TryParse(par, out c))
        {
            Counter = c;
        }
    }

    public override bool Handle(CircleVisual circle, GameContext context)
    {
        bool remove = true;

        if (circle.Value > context.LevelDef.FromNum)
        {
            circle.Value--;
            remove = false;

            Counter--;

            if (Counter == 0)
            {
                //disable
                circle.Removepeciality(this);
            }
        }
        else
        {
           // circle.TapBehav = CircleVisual.TapBehaviour.None;
            circle.Removepeciality(this);
        }

        return remove;
    }
}
