﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

	private Playground Playground { get; set; }

	private LevelDb.LevelDef LevelDef { get; set; }

	private int Score { get; set; }

	private float MicroTimer { get; set; }
	private float LevelTimer { get; set; }
	private float FlipTimer { get; set; }
	private int FlipNumberIndex { get; set; }

    private ComboEffect ComboEffect;
	private OverScoreEffect OverScoreEffect;
	private BonusEffect BonusEffect;

	private BonusGenerator BonusGenerator { get; set; }
    private GameModel Model { get; set; }


	private enum State
	{
		Stopped = 0,
		Running,
		GameOver
	}

	private State CurrState;

	void Start () 
	{
		//App.Instance.GoogleAnalytics.LogScreen ("Game");
	
		// Bonus generator
		BonusGenerator = new BonusGenerator ();
		BonusGenerator.OnBonusReleased += OnBonusReleased;


		Playground = GameObject.FindObjectOfType<Playground> ();
		ComboEffect = GameObject.FindObjectOfType<ComboEffect> ();
		OverScoreEffect = GameObject.FindObjectOfType<OverScoreEffect> ();
		BonusEffect = GameObject.FindObjectOfType<BonusEffect> ();

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
        // Tap manager
        Model = new GameModel(new GameContext() { LevelDef = level });

        Score = 0;

		Model.Numbers.Clear ();
		Model.Circles.ForEach (x => Destroy (x.gameObject));
		Model.Circles.Clear ();


		LevelDef = level;

		InitPlayground ();

		MicroTimer = LevelDef.MicroTime;
		LevelTimer = LevelDef.TotalTime;
		FlipTimer = LevelDef.FlipTime;

		Hud.Instance.Init (Score, LevelDef.Score, LevelDef.TotalTime);

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

		if (LevelTimer <= 0 || Model.Circles.Count == 0)
		{
			ProcessNumbers();
			GameOver();
		}
		else if (MicroTimer < 0)
		{
			MicroTimer = 0;
			ProcessNumbers();
		}
	
		if (FlipTimer < 0 && Model.Circles.Count > 1)
		{
			FlipTimer = LevelDef.FlipTime;
			
            var rndCircle = Model.Circles[Random.Range(0, Model.Circles.Count)];
    
            FlipCircle(rndCircle);
		}

		Hud.Instance.SetuTimerProgress (MicroTimer / LevelDef.MicroTime);
		Hud.Instance.SetLevelTimerProgress (LevelTimer / LevelDef.TotalTime, LevelDef.TotalTime);

		BonusGenerator.Update ();
	}

    private void FlipCircle(Circle circle)
    {
        var rndCircle = Model.Circles[Random.Range(0, Model.Circles.Count - 1)];

        int number = LevelDef.FlipNumbers[FlipNumberIndex];
        
        // special attribute
        Circle.SpecialAttribute attr = number == 0 ? Circle.SpecialAttribute.Joker : Circle.SpecialAttribute.None;
        
        // tap behaviour
        float rnd01 = Random.Range(0.0f, 1.0f);
        float sum = 0;
        var tapBehav = Circle.TapBehaviour.None;
        foreach (var tap in LevelDef.TapBehaviours)
        {
            sum += tap.Probability;
            if (rnd01 < sum)
            {
                tapBehav = tap.Behaviour;
                break;
            }
        }


        rndCircle.StartCoroutine(rndCircle.ChangeValueTo(number, attr, tapBehav));
        
        FlipNumberIndex = (FlipNumberIndex + 1) >= LevelDef.FlipNumbers.Count ? 0 : FlipNumberIndex + 1;
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

				Model.Circles.Add(circle);

				//circle.Value = Random.Range(LevelDef.FromNum, LevelDef.ToNum);
				circle.Value = numbers[index];

				circle.Run(Circle.TapBehaviour.None);

				circle.OnClick += OnCircleClick;

				xPos += circleSize;

				++index;
			}

			yPos -= circleSize;
		}

	
	}

	private void ProcessNumbers()
	{
		int score = 0;

		if (Model.Numbers.Count == 1)
			score = 0;
		else
		{
			score = Model.ComputeScore(Model.Numbers);
		}

		Model.Numbers.Clear ();

		Hud.Instance.ClearNumbers ();
		
		AddScore (score);
	}

	private void AddScore(int score)
	{
		if (score > 0)
		{
			//if (score > 0)
			//	Sound.Instance.Play ("score");

			Hud.Instance.AddScore (Score, score, ScoreAdded);
			Score += score;

			if (Score >= LevelDef.Score)
			{
				int overScore = Score - LevelDef.Score;

				if (overScore > 0)
				{
					OverScoreEffect.Show(overScore);
				}
			}
		}
	}

	private void GameOver()
	{
		//App.Instance.GoogleAnalytics.LogEvent("LevelFinished", (LevelDef.Order.ToString() + 1).ToString(), LevelDef.Name, Score);

		var levelStats = DbUtils.GetLevelStatistic (LevelDef);
	
		bool levelFinished = Score >= LevelDef.Score;

		if (levelFinished)
		{
			App.Instance.Player.LevelFinished (LevelDef, Score);

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
				BestScore = levelStats == null ? 0 : levelStats.Score,
				LevelName = "Level " + (LevelDef.Order + 1),
				OnRestartClick = OnRestart,
				OnNextClick = OnNextLevel,
				OnMenuClick = OnMenu,
				IsNextLevel = !gameFinished
			});

		}
		else
		{
			WindowManager.Instance.OpenWindow (WindowDef.LevelFailed, new LevelFailedWindow.Param() 
			                                   { 	
				Score = this.Score,
				RequiredScore = LevelDef.Score,
				LevelName = "Level " + (LevelDef.Order + 1),
				OnRestartClick = OnRestart,
				OnMenuClick = OnMenu
			});
		}

		CurrState = State.GameOver;


		Sound.Instance.StopMusic ();
	}


	private void ApplyBonus(BonusBase bonus)
	{
		ScoreBonus scoreBonus = bonus as ScoreBonus;
	
		if ( scoreBonus != null )
		{
			AddScore(scoreBonus.Score);
		}

		
		// effect
		BonusEffect.Show(bonus.GetText());
		

	}

	private void OnBonusReleased(Bubble bubble)
	{
		bubble.OnClick += OnBonusClick;
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



	private void OnBonusClick(Bubble bubble)
	{
		if (CurrState == State.Running)
		{
			ApplyBonus(bubble.Bonus);
		}
	}

	private void OnCircleClick(Circle circle)
	{
		if (CurrState == State.Running)
		{
            // Number patterns
            var patternResult = Model.ProcessPatterns(circle);

            if (patternResult.FitSequence)
            {
                Model.Numbers.Add(circle.Value);

                if (Model.Numbers.Count > 1)
                {
                    ComboEffect.Show(Model.Numbers.Count);
                    Sound.Instance.PlayEffect("combo" + Model.Numbers.Count);
                }
            }
            else
            {
                ProcessNumbers();
                Model.Numbers.Add(circle.Value);
            }

            Hud.Instance.AddNumber(circle);

            // Tap handlers
            var tapResult = Model.ProcessTapHandlers(circle);            

            if (tapResult.Remove)
            {
		    	Model.Circles.Remove (circle);

                Destroy(circle.gameObject);
            }



            MicroTimer = LevelDef.MicroTime;
		}
	}

}
