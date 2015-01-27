using UnityEngine;
using System.Collections;


public class App : MonoBehaviour
{
	public static App Instance { get; private set; }

	public GoogleAnalyticsV3 GoogleAnalytics;

	public Db Db { get; private set; }

	public Player Player { get; private set; }

    public ColorManager ColorManager{ get; private set; }

	public PersistentData PersistentData { get; private set; }



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
        WindowManager.Instance.OpenWindow(WindowDef.Loading, null);

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
        //ColorManager
        ColorManager = (Instantiate(Resources.Load<GameObject>("Prefabs/__ColorManager__")) as GameObject).GetComponent<ColorManager>();

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
		Player = PlayerGo.AddComponent<Player> ();
		Player.Load ();
	}


	void OnDestroy()
	{
		//GoogleAnalytics.StopSession ();
	}
}
