using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MinusOneSpeciality : Speciality
{
    private int Counter { get; set; }

    public MinusOneSpeciality(CircleController circle, GameContext context) : base(circle, context)
    {}

    public override void Init(string par)
    {
        Counter = 1;

        int c;
        if (int.TryParse(par, out c))
        {
            Counter = c;
        }
    }

    public override bool Handle()
    {
        bool remove = true;

        if (Circle.Model.Value > Context.LevelDef.FromNum)
        {
            Circle.ChangeValueTo(Circle.Model.Value - 1);

            remove = false;

            Counter--;

            if (Counter == 0)
            {
                //disable
                Circle.RemoveSpeciality(this);
            }
        }
        else
        {
           // circle.TapBehav = CircleVisual.TapBehaviour.None;
            Circle.RemoveSpeciality(this);
        }

        return remove;
    }
}
