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

	public Core.SocialService SocialService { get; private set; }

	public Db.Db Db { get; private set; }

	public Player.PlayerStatus Player { get; private set; }

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

	public void ResetProgress()
	{
		App.Instance.Player.Reset ();
	}

	
	void Init()
	{
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

		// Social Service
		SocialService = (Instantiate(Resources.Load<GameObject>("Prefabs/__SocialService__")) as GameObject).GetComponent<Core.SocialService>();
		SocialService.transform.SetParent(transform);
		SocialService.Activate();
	

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
		Player = PlayerGo.AddComponent<Player.PlayerStatus> ();
		Player.Load ();
	
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

    }


	void OnDestroy()
	{
		//GoogleAnalytics.StopSession ();
	}
}
