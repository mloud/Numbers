using UnityEngine;
using System.Collections;


public class App : MonoBehaviour
{
	public static App Instance { get; private set; }

	public GoogleAnalyticsV3 GoogleAnalytics;

	public SocialService SocialService { get; private set; }

	public Db Db { get; private set; }

	public Num.NPlayer Player { get; private set; }

    public ColorManager ColorManager{ get; private set; }

	public PersistentData PersistentData { get; private set; }

    public WindowManager WindowManager { get; private set; }

    public Sound Sound { get; private set;  }

	public Console Console { get; private set; }

#if UNITY_EDITOR
    private int StartLevelOrder = -1;
#endif 
	void Awake()
	{
        Application.targetFrameRate = 30;

		Instance = this;

		DontDestroyOnLoad (gameObject);

		//DontDestroyOnLoad (GoogleAnalytics.gameObject);

		//GoogleAnalytics.StartSession();
	
		Init ();
	}

#if UNITY_EDITOR
    public void StartLevelFromEditor(LevelDb.LevelDef level)
    {
        StartLevelOrder = level.Order;
    }
#endif 

    public void StartLevel(LevelDb.LevelDef level)
	{
        WindowManager.OpenWindow(WindowDef.Loading, null);

		PersistentData.Push (level);
		Application.LoadLevel (1);// GameScene
	}

	public void GoToLevelSelection()
	{
		Application.LoadLevel (0);
	}

	public void ResetProgress()
	{
		App.Instance.Player.Reset ();
	}

	
	void Init()
	{
		// Console
		Console = (Instantiate(Resources.Load<GameObject>("Prefabs/__Console__")) as GameObject).GetComponent<Console>();
		Console.transform.SetParent(transform);
		Console.Show(true);
		DontDestroyOnLoad(Console.gameObject);

		// Social Service
		SocialService = (Instantiate(Resources.Load<GameObject>("Prefabs/__SocialService__")) as GameObject).GetComponent<SocialService>();
		SocialService.transform.SetParent(transform);
		SocialService.Activate();
		SocialService.Login(null);


        //ColorManager
        ColorManager = (Instantiate(Resources.Load<GameObject>("Prefabs/__ColorManager__")) as GameObject).GetComponent<ColorManager>();
        ColorManager.transform.SetParent(transform);


        // DB
		Db = (Instantiate (Resources.Load ("Prefabs/Db/Db") as GameObject) as GameObject).GetComponent<Db> ();
		Db.transform.SetParent (transform);

        // Persistent data
		var persDataGo = new GameObject ("PersistentData");
		persDataGo.transform.SetParent (transform);
		PersistentData = persDataGo.AddComponent<PersistentData> ();

        // Player
		var PlayerGo = new GameObject ("Player");
		DontDestroyOnLoad (PlayerGo);
		PlayerGo.transform.SetParent (transform);
		Player = PlayerGo.AddComponent<Num.NPlayer> ();
		Player.Load ();
	
        // Window Manager
        var windowManGo = new GameObject("__WindowManager__");
        WindowManager = windowManGo.AddComponent<WindowManager>();

        // Sound
        Sound = (Instantiate(Resources.Load<GameObject>("Prefabs/__Sound__")) as GameObject).GetComponent<Sound>();
        DontDestroyOnLoad(Sound.gameObject);
        Sound.transform.SetParent(transform);

    }


	void OnDestroy()
	{
		//GoogleAnalytics.StopSession ();
	}
}
