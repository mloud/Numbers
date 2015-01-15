﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	private List<Circle> Circles { get; set; }
	private List<int> Numbers { get; set; }
	private Playground Playground { get; set; }

	private LevelDb.LevelDef LevelDef { get; set; }

	private int Score { get; set; }

	private float MicroTimer { get; set; }
	private float LevelTimer { get; set; }
	private float FlipTimer { get; set; }

	private ComboEffect ComboEffect;

	private enum State
	{
		Stopped = 0,
		Running,
		GameOver
	}

	private State CurrState;

	void Start () 
	{
		App.Instance.GoogleAnalytics.LogScreen ("Game");

		Circles = new List<Circle> ();
		Numbers = new List<int>();
		Playground = GameObject.FindObjectOfType<Playground> ();
		ComboEffect = GameObject.FindObjectOfType<ComboEffect> ();

		CurrState = State.Stopped;

		Init (App.Instance.PersistentData.Pop<LevelDb.LevelDef>());
	}

	private void RunLevel()
	{
		CurrState = State.Running;	

		Sound.Instance.PlayMusic ("ingame");
	}

	private void Restart()
	{
		Init (LevelDef);
	}

	public void Init(LevelDb.LevelDef level)
	{
		Score = 0;

		Numbers.Clear ();
		Circles.ForEach (x => Destroy (x.gameObject));
		Circles.Clear ();


		LevelDef = level;

		InitPlayground ();
		
		MicroTimer = LevelDef.MicroTime;
		LevelTimer = LevelDef.TotalTime;
		FlipTimer = LevelDef.FlipTime;

		var win = WindowManager.Instance.OpenWindow (WindowDef.StartLevel, new StartLevelWindow.Param() { LevelDef = level});
		win.GetComponentInChildren<Button> ().onClick.AddListener (() => { RunLevel();  WindowManager.Instance.CloseWindow(win.Name);});
	
	
		if (!App.Instance.Player.TutorialDone)
		{
			WindowManager.Instance.OpenWindow (WindowDef.Tutorial, null);
			win.GetComponentInChildren<Button> ().onClick.AddListener (() => { App.Instance.Player.TutorialDone = true; });
		}
	}


	void UpdateRunningState()
	{
		MicroTimer -= Time.deltaTime;
		LevelTimer -= Time.deltaTime;
		FlipTimer -= Time.deltaTime;

		if (LevelTimer <= 0 || Circles.Count == 0)
		{
			ProcessNumbers();
			GameOver();
		}
		else if (MicroTimer < 0)
		{
			MicroTimer = 0;
			ProcessNumbers();
		}
	
		if (FlipTimer < 0 && Circles.Count > 1)
		{
			FlipTimer = LevelDef.FlipTime;
			var rndCircle = Circles[Random.Range(0, Circles.Count)];

			float rnd = Random.Range(0,100);
			Circle.SpecialAttribute attr = Circle.SpecialAttribute.None;
			if (rnd > 80)
			{
				attr = Circle.SpecialAttribute.Golden;
			}


			rndCircle.StartCoroutine(rndCircle.ChangeValueTo(Random.Range(0, 15), attr));
		}

		Hud.Instance.SetuTimerProgress (MicroTimer / LevelDef.MicroTime);
		Hud.Instance.SetLevelTimerProgress (LevelTimer / LevelDef.TotalTime);
	}

	void Update ()
	{
		switch (CurrState)
		{
			case State.Running:
				UpdateRunningState();
			break;
		

			case State.GameOver:

			break;

			case State.Stopped:

			break;
		}
	}

	void InitPlayground()
	{
	
		GameObject circlePrefab = Resources.Load ("Prefabs/Circle") as GameObject;

		float circleSize = circlePrefab.GetComponent<Circle> ().Radius * 2 * 1.1f;

		List<int> numbers = new List<int> (LevelDef.Numbers);

		numbers = Utils.Randomizer.RandomizeList<int> (numbers);


		float yPos = Playground.Position.y + LevelDef.Rows * circleSize * 0.5f - circleSize * 0.5f;

		int index = 0;

		for (int y = 0; y < LevelDef.Rows; ++y)
		{
			float xPos = Playground.Position.x - LevelDef.Cols * circleSize * 0.5f + circleSize * 0.5f;

			for (int x = 0; x < LevelDef.Cols; ++x)
			{
				var circle = (Instantiate(circlePrefab) as GameObject).GetComponent<Circle>();

				circle.transform.position = new Vector3(xPos, yPos, 0);

				Circles.Add(circle);

				//circle.Value = Random.Range(LevelDef.FromNum, LevelDef.ToNum);
				circle.Value = numbers[index];

				circle.Run();

				circle.OnClick += OnCircleClick;

				xPos += circleSize;

				++index;
			}

			yPos -= circleSize;
		}

	
	}

	private void AddNumber(Circle circle)
	{
		if (CanAddNumber(circle))
		{
			Numbers.Add(circle.Value);
			if (Numbers.Count > 1)
			{
				ComboEffect.Show(Numbers.Count);
				Sound.Instance.PlayEffect("combo" + Numbers.Count);
			}
		}
		else
		{
			ProcessNumbers();
			Numbers.Add(circle.Value);
		}

		MicroTimer = LevelDef.MicroTime;
	}

	private void ProcessNumbers()
	{
		int score = 0;

		if (Numbers.Count == 1)
			score = 0;
		else
		{
			//score = (int)Mathf.Pow(2, Numbers.Count - 2) * 10;
			score = ComputeScore(Numbers);
		}

		Numbers.Clear ();

		Hud.Instance.SetNumbers (Numbers);
		
		AddScore (score);
	}

	private bool IsSequence(List<int> nums)
	{
		bool sequence = true;
		for (int i = 1; i < nums.Count; ++i)
		{
			bool seqInPair =  ((nums[i-1] + 1) == (nums[i])) || (nums[i] == 0 || nums[i - 1] == 0 );
			
			if (!seqInPair)
			{
				sequence = false;
				break;
			}
		}
		
		return sequence;
	}

	private bool IsSameValue(List<int> nums)
	{
		int lastNum = -1;
		bool sameValue = true;

		for (int i = 0; i < nums.Count; ++i)
		{
			bool isSameValue = lastNum == -1 || (nums[i] == lastNum) || nums[i] == 0;
			
			if (!isSameValue)
			{
				sameValue = false;
				break;
			}
			
			if (lastNum != nums[i] && nums[i] != 0)
			{
				lastNum = nums[i];
			}
		}
		return sameValue;
	}

	private int ComputeScore(List<int> nums)
	{

		if (IsSequence(nums))
		{
			int[] scoreTable = {0, 0, 10, 13, 17, 25, 35, 50, 70, 95, 130, 175, 230, 290, 355, 430};	
			return scoreTable[nums.Count];
		}
		if (IsSameValue(nums))
		{
			int[] scoreTable = {0, 0, 5, 6, 9, 12, 17, 25, 35, 42, 68, 90, 115, 145, 170, 215};	
			return scoreTable[nums.Count];
		}

		return 0;
	}

	private bool CanAddNumber(Circle circle)
	{
		if (Numbers.Count == 0)
			return true;

		// special number
		if (circle.Attribute == Circle.SpecialAttribute.Golden)
		{
			circle.Value = 0;
			return true;
		}

		// copy with new number
		List<int> nums = new List<int> (Numbers);
		nums.Add (circle.Value);

		// sequence
		if (IsSequence(nums))
			return true;

		// same values
		if (IsSameValue(nums))
			return true;


		return false;
	}

	private void OnCircleClick(Circle circle)
	{
		if (CurrState == State.Running)
		{
			AddNumber (circle);

			Circles.Remove (circle);

			Destroy (circle.gameObject, 0.1f);

			Hud.Instance.SetNumbers (Numbers);
		}
	}



	private void AddScore(int score)
	{
		if (score > 0)
		{
				//if (score > 0)
			//	Sound.Instance.Play ("score");

			Hud.Instance.AddScore (Score, score, ScoreAdded);
			Score += score;
		}
	}

	private void GameOver()
	{
		App.Instance.GoogleAnalytics.LogEvent("LevelFinished", (LevelDef.Order.ToString() + 1).ToString(), LevelDef.Name, Score);
		App.Instance.Player.LevelFinished (LevelDef, Score);

		CurrState = State.GameOver;

		bool gameFinished = DbUtils.IsGameFinished ();


		if (gameFinished)
		{
			WindowManager.Instance.OpenWindow (WindowDef.GameFinished, new GameFinishedWindow.Param() 
			{
				OnMenuClick = OnMenu
			});
		}


		WindowManager.Instance.OpenWindow (WindowDef.GameOver, new GameOverWindow.Param() 
	    { 	
			Score = this.Score,
		  	OnRestartClick = OnRestart,
			OnNextClick = OnNextLevel,
			OnMenuClick = OnMenu,
			IsNextLevel = !gameFinished
		});


		Sound.Instance.StopMusic ();
	}

	private void OnRestart()
	{
		Restart ();
	}

	private void OnMenu()
	{
		App.Instance.GoToLevelSelection ();
	}

	private void OnNextLevel()
	{
		Init (DbUtils.GetNextLevel (LevelDef));
	}

	private void ScoreAdded()
	{
		Sound.Instance.StopEffects ();
	}
}
