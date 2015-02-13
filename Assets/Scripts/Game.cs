using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

	private Playground Playground { get; set; }
	private GameSoundSystem SoundSystem { get; set; }


	private LevelDb.LevelDef LevelDef { get; set; }

	private int Score { get; set; }

    public bool TimersPaused { get; set; }
	public float MicroTimer { get; set; }
	public float LevelTimer { get; set; }
	private float FlipTimer { get; set; }
    private float RefillTimer { get; set; }
	private int FlipNumberIndex { get; set; }

    private ComboEffect ComboEffect;
	private OverScoreEffect OverScoreEffect;
	private BonusEffect BonusEffect;

    private SpecialAbilityManager SpecialAbilityManager { get; set; }
    private BonusGenerator BonusGenerator { get; set; }
    private GameModel Model { get; set; }


  
    private List<SpecialSlot> SpecialSlots { get; set; }


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

		SoundSystem = (Instantiate(Resources.Load<GameObject>("Prefabs/__GameSoundSystem__")) as GameObject).GetComponent<GameSoundSystem>();
    
		SoundSystem.transform.SetParent(transform);
	}

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

        var levelToRun = App.Instance.PersistentData.Pop<LevelDb.LevelDef>();
        if (levelToRun == null)
            levelToRun = App.Instance.Db.LevelDb.Levels.Find(x => x.Order == App.Instance.Db.LevelDb.DefaultLevel);


        GameUi.Instance.btnPause.onClick.AddListener(() => PauseGame(false, true));
        GameUi.Instance.btnHelp.onClick.AddListener(() =>  PauseGame(true, false));
       
        Init (levelToRun);
	}

    private void PauseGame(bool showHelpWindow, bool showPauseWindow)
    {
        CurrState = State.Paused;

        App.Instance.Sound.PauseMusic();

        UnityEngine.Time.timeScale = 0;

        if (showPauseWindow)
        {

            var win = App.Instance.WindowManager.OpenWindow(WindowDef.Pause, null) as PauseWindow;

            win.BtnContinue.onClick.AddListener(() =>
            {
                ResumeGame();
                App.Instance.WindowManager.CloseWindow(win.Name);
            });

            win.BtnMenu.onClick.AddListener(() =>
            {
                RunTimer();
                App.Instance.LoadScene(SceneDef.LevelSelection);
                App.Instance.WindowManager.CloseWindow(win.Name);
            });

            win.BtnRestart.onClick.AddListener(() =>
            {
                RunTimer(); 
                Restart();
                App.Instance.WindowManager.CloseWindow(win.Name);
            });
        }


        Hud.Instance.Stop();


        if (showHelpWindow)
        {
            var helpWin = App.Instance.WindowManager.OpenWindow(WindowDef.Patterns, new PatternWindow.Param() { Level = this.LevelDef, ShowCancelButton = true, UseAnimation = false });
            (helpWin as PatternWindow).BtnCancel.onClick.AddListener(() =>
            {
                App.Instance.WindowManager.CloseWindow(helpWin.Name);
                ResumeGame();
            });
        }


        GameUi.Instance.HideButtons();
     }

    private void ResumeGame()
    {
        RunTimer();
        App.Instance.Sound.ResumeMusic();
        GameUi.Instance.ShowButtons();
    }

    private void RunTimer()
    {
        CurrState = State.Running;
        UnityEngine.Time.timeScale = 1.0f;

        Hud.Instance.Play();
    }

	private IEnumerator RunLevelCoroutine(List<SpecialAbilityDb.SpecialAbility> specialAbilities)
	{
        
        specialAbilities.ForEach(x => Model.SpecialAbilities.Add(x.Name));
        App.Instance.PlayerStatus.AbilitiesStatus.UseAbilities(Model.SpecialAbilities);
       

        InitPlayground();

        // asseble slots
	    yield return StartCoroutine(PlaygroundAssembler.AssembleRandomCoroutine(this, Model.Slots, true));

        // flip pogs
        yield return StartCoroutine(PlaygroundFlipper.FlipRandomCoroutine(this, LevelDef, Model.Circles, false));

        // assemble special abilities
        yield return StartCoroutine(SpecialAbilityAssembler.AssembleCoroutine(SpecialSlots, 0.5f, true));

        CurrState = State.Running;

        App.Instance.Sound.PlayMusic("ingame");


        GameUi.Instance.ShowButtons();

        Hud.Instance.Play();
   }

	private void Restart()
	{
		Init (LevelDef);
	}

	public float GetProgress()
	{
		return 1 - Mathf.Clamp01(LevelTimer / LevelDef.TotalTime);
	}


    private void OpenInitialWindow()
    {
        {
            var win = (App.Instance.WindowManager.OpenWindow(WindowDef.Prelevel, new PrelevelWindow.Param() { Level = LevelDef }) as PrelevelWindow);


            win.BtnCancel.onClick.AddListener(() =>
            {
                App.Instance.LoadScene(SceneDef.LevelSelection);
            });

            win.BtnPlay.onClick.AddListener(() =>
            {
                App.Instance.WindowManager.CloseWindow(win.Name);
            

                App.Instance.WindowManager.CloseWindow(WindowDef.Patterns);

                // how start 
                var winStartLevel = App.Instance.WindowManager.OpenWindow(WindowDef.StartLevel, new StartLevelWindow.Param() { LevelDef = this.LevelDef });
                winStartLevel.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    App.Instance.WindowManager.CloseWindow(winStartLevel.Name);

                    StartCoroutine(RunLevelCoroutine(win.SelectedAbilities));

                });


            });

            var patternWin = App.Instance.WindowManager.OpenWindow(WindowDef.Patterns, new PatternWindow.Param() { Level = LevelDef, UseAnimation = true });

        }
    }

	public void Init(LevelDb.LevelDef level)
	{

		App.Instance.Sound.StopMusic();

        CurrState = State.Stopped;

        // Clear previous model
        if (Model != null)
        {
            Model.Numbers.Clear ();
            Model.Circles.ForEach (x => Destroy (x.gameObject));
            Model.Circles.Clear ();

            Model.Slots.ForEach(x => Destroy(x.gameObject));
            Model.Slots.Clear();
        }

        
		Model = new GameModel(new GameContext() { LevelDef = level, Controller = this });

		SoundSystem.Init(new GameContext() { LevelDef = level, Controller = this });

         // SpecialAbilityManager
        SpecialAbilityManager = new SpecialAbilityManager(new GameContext() { LevelDef = level, Controller = this, Model = this.Model });


        Score = 0;

		LevelDef = level;

		//InitPlayground ();

		MicroTimer = LevelDef.MicroTime;
		LevelTimer = LevelDef.TotalTime;
        FlipTimer = LevelDef.FlipTime == 0 ? float.MaxValue : LevelDef.FlipTime;
        RefillTimer = LevelDef.RefillTime == 0 ? float.MaxValue : LevelDef.RefillTime;

		Hud.Instance.Init (Score, LevelDef.Score, LevelDef.TotalTime);

        

        Invoke("OpenInitialWindow", 0.2f);

	}


	void UpdateRunningState()
	{

        if (!TimersPaused)
        {
            MicroTimer -= Time.deltaTime;
            LevelTimer -= Time.deltaTime;
            FlipTimer -= Time.deltaTime;
            RefillTimer -= Time.deltaTime;
        }

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
        else if (RefillTimer < 0)
        {
            RefillTimer = LevelDef.RefillTime;

            var emptySlots = Model.Slots.FindAll(x => x.CanPlaceCircle);

            if (emptySlots.Count > 0)
            {
                var rndSlot = emptySlots[Random.Range(0,emptySlots.Count - 1)];

                CreateCircleOnSlot(rndSlot);
            }
        }

		Hud.Instance.SetuTimerProgress (MicroTimer / LevelDef.MicroTime);
		Hud.Instance.SetLevelTimerProgress (LevelTimer / LevelDef.TotalTime, LevelDef.TotalTime);

		BonusGenerator.Update ();
        SpecialAbilityManager.Update();
    }

    public void CreateCircleOnSlot(Slot slot)
    {
        Core.Dbg.Assert(slot.CanPlaceCircle, "Game.CreateCircleOnSlot() - slot must be free!");

        var circle = CreateCircle(Resources.Load<GameObject>("Prefabs/Circle"), slot);
        
        circle.Flip(true, 0, 0);
        circle.Flip(false, 0.3f, 0);

        circle.SetValue(GetNextFlipNumber());
        circle.SetColor(GetNextColor());


        Model.Circles.Add(circle);
    }

    public int GetNextFlipNumber()
    {
        int number = LevelDef.FlipNumbers[FlipNumberIndex];
        FlipNumberIndex = (FlipNumberIndex + 1) >= LevelDef.FlipNumbers.Count ? 0 : FlipNumberIndex + 1;

        return number;
    }

    public int GetNextColor()
    {
        return Random.Range(2, 2 + LevelDef.Colors);
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

        //GameUi.Instance.btnPause.gameObject.SetActive(CurrState == State.Running);
        //GameUi.Instance.btnHelp.gameObject.SetActive(CurrState == State.Running);


	}

    CircleController CreateCircle(GameObject prefab, Slot slot)
    {
        var circle = (Instantiate(prefab) as GameObject).GetComponent<CircleController>();
        
        slot.Circle = circle;
        circle.OnRemove += slot.OnCircleRemove;
        circle.Model.Position = new Vector3(slot.transform.position.x, slot.transform.position.y, 0);
        circle.OnClick += OnCircleClick;
        return circle;
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
                

				slot.SetColor(App.Instance.ColorManager.GetColor((y * LevelDef.Cols + x) % 2 == 0 ? ColorDef.ColorType.SlotLight : ColorDef.ColorType.SlotDark));
                
				slot.transform.position = new Vector3(xPos, yPos, 0);
                Destroy(slot.GetComponent<Collider2D>());
                slot.gameObject.SetActive(false);

                var circle = CreateCircle(circlePrefab, slot);
                
                Model.Circles.Add(circle);
                Model.Slots.Add(slot);

				circle.SetValue(numbers[index]);
                circle.SetColor(GetNextColor());

                // add specialities
                List<LevelDb.SpecialityDef> specs = Data.DbUtils.SpecialitiesFromString(LevelDef.SpecialitiesForNumbers[index]);
                specs.ForEach(spec => circle.AddSpeciality(SpecialityFactory.Create(spec.Name, spec.Param, circle, Model.Context)));

				circle.Run();

				xPos += slotSize;

				++index;

                circle.Flip(true, 0, 0);
			}

			yPos -= slotSize;
		}

        InitSpecialAbilities();

	}

    private void InitSpecialAbilities2()
    {
        // Init special abilities;
        if (SpecialSlots != null)
            SpecialSlots.ForEach(x => Destroy(x.gameObject));
        SpecialSlots = new List<SpecialSlot>();

        GameObject specialSlot = Resources.Load<GameObject>("Prefabs/SpecialAbilities/Slot");

        var freeSlots = new List<Slot>(Model.Slots);


        foreach (var sa in Model.SpecialAbilities)
        {

            var rndSlot = freeSlots[Random.Range(0, freeSlots.Count - 1)];

            if (rndSlot.Circle != null)
            {
                Model.Circles.Remove(rndSlot.Circle);
                Destroy(rndSlot.Circle.gameObject);
            }
            
            freeSlots.Remove(rndSlot);

            
            var slot = (Instantiate(specialSlot) as GameObject).GetComponent<SpecialSlot>();
            slot.OnClick += OnSpecialSlotClick;
            slot.transform.SetParent(rndSlot.transform);
            slot.SpecialAbilityDef = App.Instance.Db.SpecialAbilityDb.SpecialAbilities.Find(x => x.Name == sa);
            slot.transform.localPosition = Vector3.zero;

            var visual = SpecialAbilityFactory.CreateVisual(sa);
            slot.SetVisual(visual);
            slot.StartRecharging();

            slot.gameObject.SetActive(false);

            SpecialSlots.Add(slot);
        }
    }



    private void InitSpecialAbilities()
    {
        // Init special abilities;
        if (SpecialSlots != null)
            SpecialSlots.ForEach(x => Destroy(x.gameObject));
        SpecialSlots = new List<SpecialSlot>();

        GameObject specialSlot = Resources.Load<GameObject>("Prefabs/SpecialAbilities/Slot");
        var specialSlotSize = specialSlot.GetComponent<BoxCollider2D>().size;
        float gap = 0.1f;

        var container = GameObject.Find("SpecialAbilities");
        float slotX = -((Model.SpecialAbilities.Count * (gap + specialSlotSize.x)) * 0.5f) + specialSlotSize.x * 0.5f;
        foreach (var sa in Model.SpecialAbilities)
        {
            var slot = (Instantiate(specialSlot) as GameObject).GetComponent<SpecialSlot>();
            slot.OnClick += OnSpecialSlotClick;
            slot.transform.SetParent(container.transform);
            slot.SpecialAbilityDef = App.Instance.Db.SpecialAbilityDb.SpecialAbilities.Find(x => x.Name == sa);
            var pos = new Vector3(slotX, 0, 0);
            slot.transform.localPosition = pos;

            var visual = SpecialAbilityFactory.CreateVisual(sa);
            slot.SetVisual(visual);
            slot.StartRecharging();

            slotX += (gap + specialSlotSize.x);

            slot.gameObject.SetActive(false);

            SpecialSlots.Add(slot);
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
        StartCoroutine(GameOverCoroutine());
    }

	private IEnumerator GameOverCoroutine()
	{
		//App.Instance.GoogleAnalytics.LogEvent("LevelFinished", (LevelDef.Order.ToString() + 1).ToString(), LevelDef.Name, Score);

        App.Instance.Sound.StopMusic();
        SoundSystem.Leave();

        CurrState = State.GameOver;

        // flip away cicles
        yield return StartCoroutine(PlaygroundFlipper.FlipRandomCoroutine(this, LevelDef, Model.Circles, true));

        // disassemble playground
        yield return StartCoroutine(PlaygroundAssembler.AssembleRandomCoroutine(this, Model.Slots, false));

        // disassemble special abilities
        /*yield return*/ StartCoroutine(SpecialAbilityAssembler.AssembleCoroutine(SpecialSlots, 0.5f, false));

        GameUi.Instance.HideButtons();

		var levelStatus = Data.DbUtils.GetLevelStatus (LevelDef);
	
		bool levelFinished = Score >= LevelDef.Score;

        // trigger level over event
        App.Instance.Events.Trigger(new Evt.LevelFinished() { LeveDef = this.LevelDef, Score = this.Score, Win = levelFinished });

		if (levelFinished)
		{
			App.Instance.PlayerStatus.LevelsStatus.Finish (LevelDef.Name, Score);
            var unlockedAbility = App.Instance.Db.SpecialAbilityDb.SpecialAbilities.Find(x => x.AvailableForLevel == App.Instance.PlayerStatus.LevelsStatus.LastReachedLevel);
            
            if (unlockedAbility != null)
                App.Instance.PlayerStatus.AbilitiesStatus.Unlock(unlockedAbility.Name);
         

			App.Instance.Services.GetService<Srv.SocialService>().ReportScore(Score, LevelDef.LeaderboardId, null);

			bool gameFinished = Data.DbUtils.IsGameFinished ();

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
				BestScore = levelStatus == null ? Score : levelStatus.BestScore,
				LevelName = "Level " + (LevelDef.Order + 1),
				OnRestartClick = OnRestart,
				OnNextClick = OnNextLevel,
			    OnOkClick = OnMenu,
                IsNextLevel = !Data.DbUtils.IsLastLevel(LevelDef),
				LevelDef = this.LevelDef,


                OnAnimsFinished = () =>
                {
                    if (unlockedAbility != null)
                    {
                        App.Instance.WindowManager.OpenWindow(WindowDef.SpecialAbilityUnlocked, new SpecialAbilityUnlockedWindow.Param()
                        {
                            Ability = unlockedAbility
                        });
                    }
                }

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

    private void OnSpecialSlotClick(SpecialSlot slot)
    {
        if (slot.CanBeUsed())
        {
            // create ability model
            var ability = SpecialAbilityFactory.Create(slot.SpecialAbilityDef, slot.SpecialAbilityVisual);
            slot.SpecialAbilityVisual.AbilityStarted(ability);
            slot.Toggled = false;
            ability.AbilityFinished += slot.OnAbilityFinished;
            
            SpecialAbilityManager.Run(ability);
        }
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
		App.Instance.LoadScene(SceneDef.LevelSelection);
	}

	private void OnNextLevel()
	{
		Init (Data.DbUtils.GetNextLevel (LevelDef));
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


    private void ApplyPattern(CircleController circle)
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
            Model.Circles.Remove(circle);

            Destroy(circle.gameObject);
        }
    }


	public void OnCircleClick(CircleController circle)
	{
		if (CurrState == State.Running)
		{
            ApplyPattern(circle);

            MicroTimer = LevelDef.MicroTime;
		}
	}


  
}
