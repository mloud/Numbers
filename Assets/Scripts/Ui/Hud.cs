using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Hud : MonoBehaviour 
{
	[SerializeField]
	TextMesh text;

	[SerializeField]
	TextMesh score;

	[SerializeField]
	TextMesh scoreRequired;

	[SerializeField]
	Transform barFilluTimer;

	[SerializeField]
	Transform barFillLevelTimer;



	public static Hud Instance { get; private set; }

	void Awake()
	{
		Instance = this;
	}

	public void Init(int actualScore, int requiredScore)
	{
		scoreRequired.text = requiredScore.ToString ();
		score.text = actualScore.ToString ();
	}

	public void SetuTimerProgress(float value)
	{
		var scale = barFilluTimer.transform.localScale;

		scale.x = value;

		barFilluTimer.transform.localScale = scale;
	}

	public void SetLevelTimerProgress(float value)
	{
		var scale = barFillLevelTimer.transform.localScale;
		
		scale.x = value;
		
		barFillLevelTimer.transform.localScale = scale;
	}

	public void SetNumbers(List<int> num)
	{
		string s = "";
		for (int i = 0; i < num.Count; ++i)
		{
			s += num[i].ToString() + " "; 
		}

		text.text = s;
	}

	public void AddScore(int actualScore, int score, Action scoreAdded)
	{
		StartCoroutine(AddScoreEffectCoroutine(actualScore, score, 0.2f, scoreAdded));
	}


	private IEnumerator AddScoreEffectCoroutine(int from, int addon, float time, Action scoreAdded)
	{
		float startTime = Time.time;

		float t = 0;

		while (true)
		{
			t = Mathf.Clamp((Time.time - startTime) / time, 0, 1);

			this.score.text = ((int)(from + t * addon)).ToString();

			if (t == 1)
			{
				this.score.text = (from + addon).ToString();
				break;
			}
			else
				yield return 0;
		}

		//scoreAdded ();
	}
}
