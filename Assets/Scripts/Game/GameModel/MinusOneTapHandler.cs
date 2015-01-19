using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MinusOneTapHandler : TapHandler
{
    public override bool Handle(Circle circle, GameContext context)
    {
        bool remove = true;

        if (circle.Value > context.LevelDef.FromNum)
        {
            circle.Value--;
            remove = false;
        }
        else
        {
            circle.TapBehav = Circle.TapBehaviour.None;
        }

        return remove;
    }
}
