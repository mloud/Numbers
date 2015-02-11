using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class OddNumberPattern : NumberPattern
{
    public OddNumberPattern(GameContext context)
        : base(context)
    { }


	public override bool IsPattern(List<Number> nums)
	{
		bool sequence = true;

        if (nums.Count < 2 || nums [0].Value % 2 != 1)
            return false;

		for (int i = 1; i < nums.Count; ++i)
		{
			bool seqInPair =  ((nums[i-1].Value + 2) == (nums[i].Value)) || (nums[i].Value == 0 || nums[i - 1].Value == 0 );
			
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

        res.ColorBonusMultiplier = (Context.LevelDef.Colors > 0 && IsSameColor(nums)) ? 1.5f : 1;
        res.TotalScore = (int)((nums[index].Value + addon) * nums.Count * 2 * res.ColorBonusMultiplier);

        return res;
    }

}
