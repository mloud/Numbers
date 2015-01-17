using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlusOneNumberPattern : NumberPattern
{
	public override bool IsPattern(List<int> nums)
	{
		bool sequence = true;
		for (int i = 1; i < nums.Count; ++i)
		{
			bool seqInPair =  ((nums[i-1] + 1) == (nums[i])) || (nums[i] == 0 || nums[i - 1] == 0 );
			
			if (!seqInPair)
			{
				sequence = false;
				break;
			}
		}
		return sequence;
	}

	public override int ComputeScore(List<int> nums)
	{
		int[] scoreTable = {0, 0, 10, 13, 17, 25, 35, 50, 70, 95, 130, 175, 230, 290, 355, 430};	
		return scoreTable[nums.Count];
	}
}
