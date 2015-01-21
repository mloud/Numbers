using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public abstract class Speciality
{
    public abstract bool Handle(CircleVisual circle, GameContext context);
    public virtual void UpdateMe() {}
    public virtual void Init(string par) {}
}
