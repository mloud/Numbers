using UnityEngine;
using System.Collections;

public class BonusEffect : MonoBehaviour 
{
	[SerializeField]
	TextMesh txt;

	private Animator Animator { get; set; }

	private void Awake()
	{
		Animator = GetComponent<Animator> ();
	}

	public void Show(string text)
	{
		txt.text = text;

		Animator.SetTrigger ("start");

	}
}
