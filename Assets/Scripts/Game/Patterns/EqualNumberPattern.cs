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
        if (nums.Count < 2)
            return 0;

         return nums.Count * nums.Find(x => x != 0);
	}
}
