using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour 
{
	[SerializeField]
	AlphaCutOff alphaCutOff;

	public void Set(float progress) // 0-1
	{
		alphaCutOff.SetCutOff(progress);	
	}
}
