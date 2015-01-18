using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ScoreBonus : BonusBase
{
	[SerializeField]
	TextMesh txt;

	public int Score { get; private set;}
	
	public void Init(int score)
	{
		Score = score;
		txt.text = score.ToString ();
	}

	public override void Consume ()
	{
	}

	public override string GetText()
	{
		return "+" + Score.ToString();
	}
}
