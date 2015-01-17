using UnityEngine;
using System.Collections;

public class App : MonoBehaviour
{
	public static App Instance { get; private set; }

	public GoogleAnalyticsV3 GoogleAnalytics;

	public Db Db { get; private set; }

	public Player Player { get; private set; }

	public PersistentData PersistentData { get; private set; }

	void Awake()
	{
		Instance = this;

		DontDestroyOnLoad (gameObject);

		//DontDestroyOnLoad (GoogleAnalytics.gameObject);

		//GoogleAnalytics.StartSession();
	
		Init ();
	}

	public void StartLevel(LevelDb.LevelDef level)
	{
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

	void Start ()
	{

		GameObject.FindObjectOfType<LevelUi> ().Init ();
	}

	void Init()
	{
		Db = (Instantiate (Resources.Load ("Prefabs/Db/Db") as GameObject) as GameObject).GetComponent<Db> ();
		Db.transform.SetParent (transform);

		var persDataGo = new GameObject ("PersistentData");
		persDataGo.transform.SetParent (transform);
		PersistentData = persDataGo.AddComponent<PersistentData> ();

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
