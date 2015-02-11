using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class NumberPattern 
{
	public abstract bool IsPattern(List<Number> nums);

	public abstract NumberResult ComputeScore(List<Number> nums);

    protected GameContext Context { get; private set; }

    public NumberPattern(GameContext context)
    {
        Context = context;
    }

    public bool IsSameColor(List<Number> numbers)
    {
        if (numbers.Count < 2)
            return false;

        int firstColor = numbers[0].Color;

        for (int i = 1; i < numbers.Count; ++i)
        {
            if (numbers[i].Color != firstColor)
                return false;
        }

        return true;
    }
    
    public bool CanAdd(CircleController circle, GameContext context)
    {
        if (context.Model.Numbers.Count == 0)
            return true;
    
        // special number
        if (circle.Model.Value == 0)
        { 
            return true;
        }
    
        // copy with new number
        var nums = new List<Number>(context.Model.Numbers);
        nums.Add(context.Controller.CircleToNumber(circle));
    
        return IsPattern(nums);
    }

}
