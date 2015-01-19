using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public abstract class TapHandler
{
    public abstract bool Handle(Circle circle, GameContext context);
}
