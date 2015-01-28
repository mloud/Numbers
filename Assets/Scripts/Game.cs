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


    private List<Slot> Slots { get; set;  }

	private enum State
	{
		Stopped = 0,
		Running,
		GameOver,
        Paused
	}

	private State CurrState;

    void Awake()
    {
        if (GameObject.FindObjectOfType<App>() == null)
        {
            var appGo = Instantiate(Resources.Load("Prefabs/__App__"));
        }
    }

	void Start () 
	{

		//App.Instance.GoogleAnalytics.LogScreen ("Game");

        Slots = new List<Slot>();

		// Bonus generator
		BonusGenerator = new BonusGenerator ();
		BonusGenerator.OnBonusReleased += OnBonusReleased;


		Playground = GameObject.FindObjectOfType<Playground> ();
		ComboEffect = GameObject.FindObjectOfType<ComboEffect> ();
		OverScoreEffect = GameObject.FindObjectOfType<OverScoreEffect> ();
		BonusEffect = GameObject.FindObjectOfType<BonusEffect> ();

		CurrState = State.Stopped;

        var levelToRun = App.Instance.PersistentData.Pop<LevelDb.LevelDef>();
        if (levelToRun == null)
            levelToRun = App.Instance.Db.LevelDb.Levels.Find(x => x.Order == App.Instance.Db.LevelDb.DefaultLevel);


        GameUi.Instance.btnPause.onClick.AddListener(() => PauseGame());


        Init (levelToRun);
	}

    private void PauseGame()
    {
        CurrState = State.Paused;

        UnityEngine.Time.timeScale = 0;

        var win = App.Instance.WindowManager.OpenWindow(WindowDef.Pause, null) as PauseWindow;

        win.BtnContinue.onClick.AddListener(() => { 
            RunTimer();
            App.Instance.WindowManager.CloseWindow(win.Name);
        });

        win.BtnMenu.onClick.AddListener(() => {
            RunTimer(); 
            App.Instance.GoToLevelSelection();
            App.Instance.WindowManager.CloseWindow(win.Name);
        });
        
        win.BtnRestart.onClick.AddListener(() => { 
            RunTimer(); Restart(); 
            App.Instance.WindowManager.CloseWindow(win.Name); 
        });

        GameUi.Instance.btnPause.gameObject.SetActive(false);
    }

    private void RunTimer()
    {
        CurrState = State.Running;
        UnityEngine.Time.timeScale = 1.0f;
        GameUi.Instance.btnPause.gameObject.SetActive(true);
    }

	private void RunLevel()
	{
		CurrState = State.Running;	

		App.Instance.Sound.PlayMusic ("ingame");


        PlaygroundFlipper.FlipRandom(LevelDef, Model.Circles);


        GameUi.Instance.btnPause.gameObject.SetActive(true);
   }

	private void Restart()
	{
		Init (LevelDef);
	}

	public void Init(LevelDb.LevelDef level)
	{
        if (Model != null)
        {
            Model.Numbers.Clear ();
            Model.Circles.ForEach (x => Destroy (x.gameObject));
            Model.Circles.Clear ();
        }


        Slots.ForEach(x => Destroy(x.gameObject));
        Slots.Clear();

        // Tap manager
        Model = new GameModel(new GameContext() { LevelDef = level, Controller = this });

        Score = 0;

		LevelDef = level;

		InitPlayground ();

		MicroTimer = LevelDef.MicroTime;
		LevelTimer = LevelDef.TotalTime;
		FlipTimer = LevelDef.FlipTime;

		Hud.Instance.Init (Score, LevelDef.Score, LevelDef.TotalTime);

        var winStartLevel = App.Instance.WindowManager.OpenWindow(WindowDef.StartLevel, new StartLevelWindow.Param() { LevelDef = level });
        winStartLevel.GetComponentInChildren<Button>().onClick.AddListener(() =>
        { 
            RunLevel();
            App.Instance.WindowManager.CloseWindow(winStartLevel.Name);
        });

        GameUi.Instance.btnPause.gameObject.SetActive(false);

		//if (!App.Instance.Player.TutorialDone)
		{
            var win = App.Instance.WindowManager.OpenWindow(WindowDef.Tutorial, new TutorialWindow.Param() { Level = LevelDef });
			win.GetComponentInChildren<Button> ().onClick.AddListener (() => {
                App.Instance.WindowManager.CloseWindow(win.Name);
                App.Instance.Player.TutorialDone = true;
            });
		}
	}


	void UpdateRunningState()
	{
		MicroTimer -= Time.deltaTime;
		LevelTimer -= Time.deltaTime;
		FlipTimer -= Time.deltaTime;

        if (LevelTimer <= 0 || Model.Circles.Count == 0 || (Model.Circles.Count == 1 && Model.Numbers.Count == 0))
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
		
            var circlesWithoutSpec = Model.Circles.FindAll(x=>x.Model.Specialities.Find(y=>y.GetType() == typeof(ChangeValueSpeciality)) == null);

            var rndCircle = circlesWithoutSpec[Random.Range(0, circlesWithoutSpec.Count)];
    
            rndCircle.AddSpeciality(SpecialityFactory.Create("f", "2", rndCircle, Model.Context));
		}

		Hud.Instance.SetuTimerProgress (MicroTimer / LevelDef.MicroTime);
		Hud.Instance.SetLevelTimerProgress (LevelTimer / LevelDef.TotalTime, LevelDef.TotalTime);

		BonusGenerator.Update ();
	}

    public int GetNextFlipNumber()
    {
        int number = LevelDef.FlipNumbers[FlipNumberIndex];
        FlipNumberIndex = (FlipNumberIndex + 1) >= LevelDef.FlipNumbers.Count ? 0 : FlipNumberIndex + 1;

        return number;
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
	
		GameObject circlePrefab = Resources.Load<GameObject> ("Prefabs/Circle");
        GameObject slotPrefab = Resources.Load<GameObject>("Prefabs/Slot");

        float slotSize = slotPrefab.GetComponent<BoxCollider2D>().size.x;

		List<int> numbers = new List<int> (LevelDef.Numbers);

		numbers = Utils.Randomizer.RandomizeList<int> (numbers);


		float yPos = Playground.Position.y + LevelDef.Rows * slotSize * 0.5f - slotSize * 0.5f;

		int index = 0;



		for (int y = 0; y < LevelDef.Rows; ++y)
		{
			float xPos = Playground.Position.x - LevelDef.Cols * slotSize * 0.5f + slotSize * 0.5f;

			for (int x = 0; x < LevelDef.Cols; ++x)
			{
                var slot = (Instantiate(slotPrefab) as GameObject).GetComponent<Slot>();
                var circle = (Instantiate(circlePrefab) as GameObject).GetComponent<CircleController>();

                slot.SetColor(App.Instance.ColorManager.GetColor((y * LevelDef.Rows + x) % 2 == 0 ? ColorDef.ColorType.SlotLight : ColorDef.ColorType.SlotDark));
                Destroy(slot.GetComponent<Collider2D>());
                circle.Model.Position = new Vector3(xPos, yPos, 0);
                slot.transform.position = new Vector3(xPos, yPos, 0);
               
				Model.Circles.Add(circle);
                Slots.Add(slot);

				circle.SetValue(numbers[index]);
                circle.SetColor(Random.Range(2, 2 + LevelDef.Colors));

                // add specialities
                List<LevelDb.SpecialityDef> specs = DbUtils.SpecialitiesFromString(LevelDef.SpecialitiesForNumbers[index]);
                specs.ForEach(spec => circle.AddSpeciality(SpecialityFactory.Create(spec.Name, spec.Param, circle, Model.Context)));

				circle.Run();

				circle.OnClick += OnCircleClick;

				xPos += slotSize;

				++index;

                circle.Flip(true, 0, 0);

			}

			yPos -= slotSize;
		}
	}

	private void ProcessNumbers()
	{
        NumberResult res = null;

        if (Model.Numbers.Count == 1)
        {
            res = new NumberResult() { TotalScore = 0, ColorBonusMultiplier = 0.0f };
        }
        else
        {
            res = Model.ComputeScore(Model.Numbers);
        }

		Model.Numbers.Clear ();

		Hud.Instance.ClearNumbers ();
		
		ApplyNumbersResult (res);
	}


	private void ApplyNumbersResult(NumberResult res)
	{
		if (res.TotalScore > 0)
		{
			//if (score > 0)
			//	Sound.Instance.Play ("score");

			Hud.Instance.AddScore (Score, res.TotalScore, ScoreAdded);
			Score += res.TotalScore;

			if (Score >= LevelDef.Score)
			{
				int overScore = Score - LevelDef.Score;

				if (overScore > 0)
				{
					OverScoreEffect.Show(overScore);
				}
			}

            //if (res.ColorBonusMultiplier > 0.0f)
            //{
            //    BonusEffect.Show("Color bonus " + res.ColorBonusMultiplier);
            //}
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
                App.Instance.WindowManager.OpenWindow(WindowDef.GameFinished, new GameFinishedWindow.Param() 
				{
					OnMenuClick = OnMenu
				});
			}


            App.Instance.WindowManager.OpenWindow(WindowDef.GameOver, new GameOverWindow.Param() 
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
            App.Instance.WindowManager.OpenWindow(WindowDef.LevelFailed, new LevelFailedWindow.Param() 
			                                   { 	
				Score = this.Score,
				RequiredScore = LevelDef.Score,
				LevelName = "Level " + (LevelDef.Order + 1),
				OnRestartClick = OnRestart,
				OnMenuClick = OnMenu
			});
		}

		CurrState = State.GameOver;


		App.Instance.Sound.StopMusic ();
	}


	private void ApplyBonus(BonusBase bonus)
	{
		ScoreBonus scoreBonus = bonus as ScoreBonus;
	
		if ( scoreBonus != null )
		{
            ApplyNumbersResult(new NumberResult() { TotalScore = scoreBonus.Score, ColorBonusMultiplier = 0.0f});
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
        App.Instance.Sound.StopEffects();
	}



	private void OnBonusClick(Bubble bubble)
	{
		if (CurrState == State.Running)
		{
			ApplyBonus(bubble.Bonus);
		}
	}

    public Number CircleToNumber(CircleController circle)
    {
        return new Number() { Value = circle.Model.Value, Color = circle.Model.Color };
    }
        

	private void OnCircleClick(CircleController circle)
	{
		if (CurrState == State.Running)
		{
            // Number patterns
            var patternResult = Model.ProcessPatterns(circle);

            if (patternResult.FitSequence)
            {
                var numbers = CircleToNumber(circle);
                Model.Numbers.Add(numbers);

                if (Model.Numbers.Count > 1)
                {
                    string specialText = patternResult.Pattern.IsSameColor(Model.Numbers) ? "in color" : null;
                    ComboEffect.Show(Model.Numbers.Count, specialText);
                    App.Instance.Sound.PlayEffect("combo" + Model.Numbers.Count);
                }
            }
            else
            {
                ProcessNumbers();
                Model.Numbers.Add(CircleToNumber(circle));
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
