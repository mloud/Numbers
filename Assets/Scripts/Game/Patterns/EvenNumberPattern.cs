using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EvenNumberPattern : NumberPattern
{
    public override bool IsPattern(List<int> nums)
	{
		bool sequence = true;

        if (nums.Count < 2 || nums [0] % 2 != 0)
            return false;

		for (int i = 1; i < nums.Count; ++i)
		{
			bool seqInPair =  ((nums[i-1] + 2) == (nums[i])) || (nums[i] == 0 || nums[i - 1] == 0 );
			
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
        if (nums.Count < 2)
            return 0;

        int index = nums.FindLastIndex(x=>x != 0);

        int addon = 0;
        for (int i = index + 1; i < nums.Count; ++i)
            addon++;
       
        return  (nums [index] + addon) * nums.Count * 2;
    }

}
