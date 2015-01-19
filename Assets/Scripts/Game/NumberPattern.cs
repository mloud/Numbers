using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class NumberPattern 
{
	public abstract bool IsPattern(List<int> nums);

	public abstract int ComputeScore(List<int> nums);

    public bool CanAdd(Circle circle, GameContext context)
    {
        if (context.Model.Numbers.Count == 0)
            return true;
    
        // special number
        if (circle.Attribute == Circle.SpecialAttribute.Joker)
        {
            circle.Value = 0;
            return true;
        }
    
        // copy with new number
        List<int> nums = new List<int>(context.Model.Numbers);
        nums.Add(circle.Value);
    
        return IsPattern(nums);
    }

}
