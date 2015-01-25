using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ChangeValueSpeciality : Speciality
{
    private float Time { get; set; }

    private float Timer { get; set; }

    public ChangeValueSpeciality(CircleController circle, GameContext context) : base(circle, context)
    {}

    public override void Init(string par)
    {
        Time = 0;
        Timer = -1;

        float c;
        if (float.TryParse(par, out c))
        {
            Time = c;
        }

        Circle.ShowProgressBar(true);
     
        Timer = Time;
    }

    public override bool Handle()
    {
        bool remove = true;

        Circle.RemoveSpeciality(this);

        return remove;
    }

    public override void UpdateMe()
    {
        if (Timer != -1)
        {
            Timer -= UnityEngine.Time.deltaTime;

            Circle.SetProgress(Timer / Time);
         
            if (Timer <= 0)
            {
                Timer = -1;
                Circle.ShowProgressBar(false);

                Circle.ChangeValueTo(Context.Controller.GetNextFlipNumber());

                Circle.RemoveSpeciality(this);
            }
        }
    }

}
