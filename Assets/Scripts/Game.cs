using UnityEngine;
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
		GameOver
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

        Init (levelToRun);
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

		var win = WindowManager.Instance.OpenWindow (WindowDef.StartLevel, new StartLevelWindow.Param() { LevelDef = level});
		win.GetComponentInChildren<Button> ().onClick.AddListener (() => { RunLevel();  WindowManager.Instance.CloseWindow(win.Name);});
	
	
		//if (!App.Instance.Player.TutorialDone)
		{
            WindowManager.Instance.OpenWindow (WindowDef.Tutorial, new TutorialWindow.Param() {Level = LevelDef});
			win.GetComponentInChildren<Button> ().onClick.AddListener (() => { App.Instance.Player.TutorialDone = true; });
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


                // add specialities
                List<LevelDb.SpecialityDef> specs = DbUtils.SpecialitiesFromString(LevelDef.SpecialitiesForNumbers[index]);
                specs.ForEach(spec => circle.AddSpeciality(SpecialityFactory.Create(spec.Name, spec.Param, circle, Model.Context)));

				circle.Run();

				circle.OnClick += OnCircleClick;

				xPos += slotSize;

				++index;
			}

			yPos -= slotSize;
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

	private void OnCircleClick(CircleController circle)
	{
		if (CurrState == State.Running)
		{
            // Number patterns
            var patternResult = Model.ProcessPatterns(circle);

            if (patternResult.FitSequence)
            {
                Model.Numbers.Add(circle.Model.Value);

                if (Model.Numbers.Count > 1)
                {
                    ComboEffect.Show(Model.Numbers.Count);
                    Sound.Instance.PlayEffect("combo" + Model.Numbers.Count);
                }
            }
            else
            {
                ProcessNumbers();
                Model.Numbers.Add(circle.Model.Value);
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
