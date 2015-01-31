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
	TextMesh levelTimer;

	[SerializeField]
	TextMesh score;

	[SerializeField]
	TextMesh scoreRequired;

	[SerializeField]
	TextMesh scoreSlash;


	[SerializeField]
	ProgressBar barFilluTimer;

	[SerializeField]
	ProgressBar barFillLevelTimer;

	[SerializeField]
	NumberContainer numberContainer;



	public static Hud Instance { get; private set; }

	void Awake()
	{
		Instance = this;
	}

	public void Init(int actualScore, int requiredScore, float levelDuration)
	{
		scoreRequired.text = requiredScore.ToString ();
		score.text = actualScore.ToString ();

		scoreRequired.gameObject.SetActive(requiredScore > 0);
		scoreSlash.gameObject.SetActive(requiredScore > 0);


		SetLevelTimerProgress (1, levelDuration);
		SetuTimerProgress (1);
	}

	public void SetuTimerProgress(float value)
	{
		barFilluTimer.Set (value);
	}

	public void SetLevelTimerProgress(float value, float levelDuration)
	{
		barFillLevelTimer.Set (value);


		float secondsLeft = value * levelDuration;

		TimeSpan t = TimeSpan.FromSeconds( secondsLeft );
		
		string res = string.Format ("{0:D1}:{1:D2}", 
		                              t.Minutes, 
		                              t.Seconds);
		                     
		levelTimer.text = res;
	}


	public void AddNumber(CircleController circle)
	{
		numberContainer.AddNumber (circle);
	}

	public void ClearNumbers()
	{
		numberContainer.Clear ();
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
