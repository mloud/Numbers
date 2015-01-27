using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public abstract class Speciality
{
    protected CircleController Circle { get; set; }
    protected GameContext Context { get; set; }

    public bool HasIcon { get; protected set; }

    public Speciality(CircleController circle, GameContext context)
    {
        Circle = circle;
        Context = context;
    }

    public abstract bool Handle();
    public virtual void UpdateMe() {}
    public virtual void Init(string par) {}
}
