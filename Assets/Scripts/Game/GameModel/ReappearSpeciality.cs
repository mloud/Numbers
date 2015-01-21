using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ReappearSpeciality : Speciality
{
    private float Time { get; set; }


    public override void Init(string par)
    {
        Time = 0;

        float c;
        if (float.TryParse(par, out c))
        {
            Time = c;
        }
    }

    public override bool Handle(CircleVisual circle, GameContext context)
    {
        bool remove = false;

        circle.Value = context.Controller.GetNextFlipNumber();

        return remove;
    }
}
