using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BonusGenerator
{
	private float Interval = 4;


	private float Timer { get; set; }

	public void Init()
	{
		Timer = Interval;
	}

	public void Update()
	{
		Timer -= Time.deltaTime;

		if (Timer <= 0)
		{
			Release();
			Timer = Interval;
		}
	}


	private void Release()
	{
		GameObject bonusGo = GameObject.Instantiate(Resources.Load ("Prefabs/Bubble")) as GameObject;

		Bubble bubble = bonusGo.GetComponent<Bubble> ();


		Vector3 worldPoint = Camera.main.ScreenToWorldPoint (new Vector3 (Random.Range (-Screen.width * 0.5f, Screen.width * 0.5f), -Screen.height * 0.5f, 0));
		worldPoint.z = 0;

		bubble.Run (worldPoint, Vector3.up, 0.10f);
	}

}
