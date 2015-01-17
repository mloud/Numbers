using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class NumberPattern 
{
	public abstract bool IsPattern(List<int> nums);

	public abstract int ComputeScore(List<int> nums);
}
