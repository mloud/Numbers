using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EqualNumberPattern : NumberPattern
{
	public override bool IsPattern(List<Number> nums)
	{
		int lastNum = -1;
		bool sameValue = true;
		
		for (int i = 0; i < nums.Count; ++i)
		{
			bool isSameValue = lastNum == -1 || (nums[i].Value == lastNum) || nums[i].Value == 0;
			
			if (!isSameValue)
			{
				sameValue = false;
				break;
			}
			
			if (lastNum != nums[i].Value && nums[i].Value != 0)
			{
				lastNum = nums[i].Value;
			}
		}
		return sameValue;
	}

	public override NumberResult ComputeScore(List<Number> nums)
    {
        var res = new NumberResult();

        if (nums.Count < 2)
            return res;

        res.ColorBonusMultiplier = IsSameColor(nums) ? 1.5f : 1;
        res.TotalScore = (int)(nums.Count * nums.Find(x => x.Value != 0).Value * res.ColorBonusMultiplier);

        return res;
    }
}
