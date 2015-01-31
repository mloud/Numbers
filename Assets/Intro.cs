using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour 
{
	[SerializeField]
	float maxDuration = 8.0f;

	[SerializeField]
	float minDuration = 3.0f;

	private float Timer { get; set; }

	private bool loginFinished = false;

	void Start () 
	{
		Timer = Time.time;

		App.Instance.SocialService.Login((bool succ) =>
	 	{
			loginFinished = true;
		});
	}
	
	void Update () 
	{
		if ( (Time.time - Timer) > minDuration &&  ( loginFinished || (Time.time - Timer) > maxDuration))
		{
			App.Instance.LoadScene(SceneDef.MenuScene);
		}
	}
}
