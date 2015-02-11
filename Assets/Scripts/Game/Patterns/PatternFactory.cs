using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;


public static class PatternFactory
{
    public static NumberPattern Create(string name, GameContext context)
    {
        NumberPattern pattern = null;
        if (name == PatternDef.PlusOne)
        {
            pattern = new PlusOneNumberPattern(context);
        }
        else if (name == PatternDef.SameNumbers)
        {
            pattern = new EqualNumberPattern(context);
        }
        else if (name == PatternDef.EvenNumbers)
        {
            pattern = new EvenNumberPattern(context);
        }
        else if (name == PatternDef.OddNumbers)
        {
            pattern = new OddNumberPattern(context);
        }

        UnityEngine.Debug.Log(pattern == null ? "Cannot create pattern :" + name : "");

        if (pattern != null)
        {
            //pattern.Init(param);
        }

        return pattern;
    }
}
