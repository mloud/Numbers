using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class BonusGenerator
{
	public Action<Bubble> OnBonusReleased;

	private float Interval = 4;

	private float Timer { get; set; }


	public BonusGenerator()
	{
		Init ();
	}

	private void Init()
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
		// Create bubble holder
		GameObject bubbleGo = GameObject.Instantiate(Resources.Load ("Prefabs/Bonuses/Bubble")) as GameObject;
		Bubble bubble = bubbleGo.GetComponent<Bubble> ();

		float posX = UnityEngine.Random.Range (0, Screen.width);
		float posY = -Screen.height * 0.5f;
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint (new Vector2 (posX, posY));
		worldPoint.z = -1.0f;

		// create bonus
		GameObject bonusGo = GameObject.Instantiate(Resources.Load ("Prefabs/Bonuses/ScoreBonus")) as GameObject;
		var bonus = bonusGo.GetComponent<ScoreBonus> ();
		bonus.Init (10);

		bubble.Run (worldPoint, Vector3.up, 0.05f, bonus);

		if (OnBonusReleased != null)
		{
			OnBonusReleased(bubble);
		}
	}

}
