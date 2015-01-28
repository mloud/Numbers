﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlusOneNumberPattern : NumberPattern
{
  
	public override bool IsPattern(List<Number> nums)
	{
		bool sequence = true;
		for (int i = 1; i < nums.Count; ++i)
		{
			bool seqInPair =  ((nums[i-1].Value + 1) == (nums[i].Value)) || (nums[i].Value == 0 || nums[i - 1].Value == 0 );
			
			if (!seqInPair)
			{
				sequence = false;
				break;
			}
		}
		return sequence;
	}

	public override NumberResult ComputeScore(List<Number> nums)
	{
        var res = new NumberResult();

        if (nums.Count < 2)
            return res;

        int index = nums.FindLastIndex(x=>x.Value != 0);

        int addon = 0;
        for (int i = index + 1; i < nums.Count; ++i)
            addon++;

        res.ColorBonusMultiplier = IsSameColor(nums) ? 1.5f : 1;
        res.TotalScore = (int)((nums[index].Value + addon) * nums.Count * 2 * res.ColorBonusMultiplier);

        return res;
    }

}
