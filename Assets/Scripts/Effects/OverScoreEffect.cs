using UnityEngine;
using System.Collections;

public class OverScoreEffect : MonoBehaviour 
{
	[SerializeField]
	TextMesh txtScores;

	private Animator Animator { get; set; }

	private void Awake()
	{
		Animator = GetComponent<Animator> ();
	}

	public void Show(int number)
	{
		txtScores.text = "+" + number;
		Animator.SetTrigger ("start");
	}
}
