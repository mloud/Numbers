using UnityEngine;
using System.Collections;


public class App : MonoBehaviour
{
	public static App Instance 
	{ 
		get 
		{
			if (instance == null && Application.isPlaying)
			{
				GameObject go = new GameObject("__App__");
				instance = go.AddComponent<App>();
			}

			return instance;
		}
	}

	public GoogleAnalyticsV3 GoogleAnalytics;


    public Srv.Services Services { get; private set; }

	public Db.Db Db { get; private set; }

    public GameStatus.PlayerStatus PlayerStatus { get; private set; }

    public ColorManager ColorManager{ get; private set; }

	public PersistentData PersistentData { get; private set; }

    public WindowManager WindowManager { get; private set; }

    public Sound Sound { get; private set;  }

	public RootConsole RootConsole { get; private set; }

	public LogConsole Console { get; private set; }

	public SocialServiceConsole SocialConsole { get; private set; }

	private static App instance;

#if UNITY_EDITOR
    private int StartLevelOrder = -1;
#endif 
	void Awake()
	{
        instance = this;

        Application.targetFrameRate = 30;

		DontDestroyOnLoad (gameObject);

		//DontDestroyOnLoad (GoogleAnalytics.gameObject);

		//GoogleAnalytics.StartSession();
	
		Init ();
	}

    void Start()
    {}

#if UNITY_EDITOR
    public void StartLevelFromEditor(LevelDb.LevelDef level)
    {
        StartLevelOrder = level.Order;
    }
#endif 

    public void StartLevel(LevelDb.LevelDef level)
	{
        //WindowManager.OpenWindow(WindowDef.Loading, null);

		PersistentData.Push (level);
		App.Instance.LoadScene(SceneDef.GameScene);
	}

	public void StartSurvival()
	{
		var level = Db.LevelDb.Levels.Find(x=>x.Name == "Survival");

		Core.Dbg.Assert(level != null, "App.StartLevel() Survival level not found");

		StartLevel(level);
		
	}

	public void LoadScene(string scene)
	{
		StartCoroutine(LoadSceneCoroutine(scene));
	}

	private IEnumerator LoadSceneCoroutine(string scene)
	{
		yield return StartCoroutine(WindowManager.ShowFadeCoroutine(false, 0.1f));
		//WindowManager.OpenWindow(WindowDef.Loading, null);
		Application.LoadLevel (scene);

		//yield return StartCoroutine(WindowManager.ShowFadeCoroutine(true, 2.0f));

	}


	
	void Init()
	{
        // Services
        Services = new Srv.Services();
		Services.RegisterService(new Srv.SaveGameService());
        Services.RegisterService(new Srv.SocialService());

		// RootConsole
		RootConsole = (Instantiate(Resources.Load<GameObject>("Prefabs/__RootConsole__")) as GameObject).GetComponent<RootConsole>();
		RootConsole.transform.SetParent(transform);
		DontDestroyOnLoad(RootConsole.gameObject);

		// SocialConsole
		SocialConsole = (Instantiate(Resources.Load<GameObject>("Prefabs/__SocialServiceConsole__")) as GameObject).GetComponent<SocialServiceConsole>();
		SocialConsole.transform.SetParent(transform);
		DontDestroyOnLoad(SocialConsole.gameObject);


		// LogConsole
		Console = (Instantiate(Resources.Load<GameObject>("Prefabs/__LogConsole__")) as GameObject).GetComponent<LogConsole>();
		Console.transform.SetParent(transform);
		Console.Show(false);
		DontDestroyOnLoad(Console.gameObject);

        //ColorManager
        ColorManager = (Instantiate(Resources.Load<GameObject>("Prefabs/__ColorManager__")) as GameObject).GetComponent<ColorManager>();
        ColorManager.transform.SetParent(transform);


        // DB
		Db = (Instantiate (Resources.Load ("Prefabs/Db/Db") as GameObject) as GameObject).GetComponent<Db.Db> ();
		Db.transform.SetParent (transform);

        // Persistent data
		var persDataGo = new GameObject ("__PersistentData__");
		persDataGo.transform.SetParent (transform);
		PersistentData = persDataGo.AddComponent<PersistentData> ();

        // Player
		var PlayerGo = new GameObject ("__PlayerStatus__");
		DontDestroyOnLoad (PlayerGo);
		PlayerGo.transform.SetParent (transform);
        PlayerStatus = PlayerGo.AddComponent<GameStatus.PlayerStatus>();
        PlayerStatus.Init();
	
        // Window Manager
        var windowManGo = new GameObject("__WindowManager__");
        WindowManager = windowManGo.AddComponent<WindowManager>();
		DontDestroyOnLoad(windowManGo);
		windowManGo.transform.SetParent(transform);

        // Sound
        Sound = (Instantiate(Resources.Load<GameObject>("Prefabs/__Sound__")) as GameObject).GetComponent<Sound>();
        Sound.gameObject.name = "__Sound__";
        DontDestroyOnLoad(Sound.gameObject);
        Sound.transform.SetParent(transform);

        StartCoroutine(StartServicesCoroutine());
        

    }

    private IEnumerator StartServicesCoroutine()
    {
        float maxLoadingTime = 4.0f;

        float startTime = Time.time;

        WindowManager.OpenWindow(WindowDef.Intro, null);

        bool loggingFinished = false;
        bool saveGameInit = false;
        
        //activate social services
		Services.GetService<Srv.SocialService>().Activate();

        // 1) login
		Services.GetService<Srv.SocialService>().Login((bool succ) =>
        {
            loggingFinished = true;
        });

        while (!loggingFinished)
            yield return 0;


        // 2) SaveGame service init

        var saveModes = Srv.SaveGameService.Mode.Local;
		if (Services.GetService<Srv.SocialService>().IsLogged())
            saveModes |= Srv.SaveGameService.Mode.Cloud;

        Services.GetService<Srv.SaveGameService>().Init(saveModes, (Srv.SaveGameService.Mode mode, bool result) => 
        {
            saveModes ^= mode;
            saveGameInit = saveModes == 0;
        });

        while (!saveGameInit)
            yield return 0;

        // Load SaveGame
        Services.GetService<Srv.SaveGameService>().Load();


        float timeLeft = Mathf.Max(0, maxLoadingTime - (Time.time - startTime));
        yield return new WaitForSeconds(timeLeft);

        WindowManager.CloseWindow(WindowDef.Intro);

        LoadScene(SceneDef.MenuScene);

    }



	void OnDestroy()
	{
		//GoogleAnalytics.StopSession ();
	}
}
