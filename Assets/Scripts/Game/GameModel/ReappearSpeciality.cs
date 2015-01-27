using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ReappearSpeciality : Speciality
{
    private float Time { get; set; }

    private float Timer { get; set; }

    public ReappearSpeciality(CircleController circle, GameContext context) : base(circle, context)
    {}

    public override void Init(string par)
    {
        HasIcon = true;

        Time = 0;
        Timer = -1;

        float c;
        if (float.TryParse(par, out c))
        {
            Time = c;
        }
    }

    public override bool Handle()
    {
        bool remove = false;

        Circle.SetValue(Context.Controller.GetNextFlipNumber());

        Timer = Time;

        if (Time > 0)
        {
            Circle.Enable(false, 0.2f);
            Circle.ShowProgressBar(true);
        }

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
                Circle.Enable(true, 0.2f);
                Circle.ShowProgressBar(false);
            }
        }
    }

}
