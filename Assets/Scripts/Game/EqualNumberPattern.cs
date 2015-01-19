using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EqualNumberPattern : NumberPattern
{
	public override bool IsPattern(List<int> nums)
	{
		int lastNum = -1;
		bool sameValue = true;
		
		for (int i = 0; i < nums.Count; ++i)
		{
			bool isSameValue = lastNum == -1 || (nums[i] == lastNum) || nums[i] == 0;
			
			if (!isSameValue)
			{
				sameValue = false;
				break;
			}
			
			if (lastNum != nums[i] && nums[i] != 0)
			{
				lastNum = nums[i];
			}
		}
		return sameValue;
	}

	public override int ComputeScore(List<int> nums)
	{
		//int[] scoreTable = {0, 0, 5, 6, 9, 12, 17, 25, 35, 42, 68, 90, 115, 145, 170, 215};	
		//return scoreTable[nums.Count];

         return nums.Count * nums.Find(x => x != 0);
	}
}
