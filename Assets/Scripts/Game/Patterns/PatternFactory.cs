using System.Collections;
using System.Collections.Generic;
using System;


public static class PatternFactory
{
    public static NumberPattern Create(string name)
    {
        NumberPattern pattern = null;
        if (name == PatternDef.PlusOne)
        {
            pattern = new PlusOneNumberPattern();
        }
        else if (name == PatternDef.SameNumbers)
        {
            pattern = new EqualNumberPattern();
        }

        if (pattern != null)
        {
            //pattern.Init(param);
        }

        return pattern;
    }
}
