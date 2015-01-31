using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LevelUi : MonoBehaviour
{
	[SerializeField]
	private RectTransform List; 

	[SerializeField]
	private Text Titles; 

	[SerializeField]
	private Button resetButton;

	[SerializeField]
	private Button backButton;



	void Start ()
	{
		resetButton.onClick.AddListener( () => { OnReset(); });
		backButton.onClick.AddListener( () => { OnBack(); });

		Init ();
	}

	public void Init()
	{
		//App.Instance.GoogleAnalytics.LogScreen ("LevelSelection");
	
		GameObject levelPrefab = Resources.Load ("Prefabs/Ui/Level") as GameObject;

	
		for (int i = 0; i < App.Instance.Db.LevelDb.Levels.Count; ++i)
		{
			var levelDef =  App.Instance.Db.LevelDb.Levels[i];

			var level = (Instantiate(levelPrefab) as GameObject).GetComponent<LevelComponent>();
	
			level.transform.SetParent(List);

			level.transform.localScale = Vector3.one;

			level.Set(levelDef);
#if !UNLOCK_LEVELS
			if (DbUtils.IsLevelUnlocked(levelDef))
#endif
			level.LevelButton.onClick.AddListener ( () => OnLevelClick(levelDef));

			level.LeaderboardButton.onClick.AddListener( () => OnLeaderboardClick(levelDef));
		}
	}

	void Update () 
	{}

	public void OnReset()
	{
		App.Instance.ResetProgress ();
		App.Instance.LoadScene (SceneDef.LevelSelection);
	}

	public void OnBack()
	{
		App.Instance.LoadScene(SceneDef.MenuScene);
	}

	private void OnLevelClick(LevelDb.LevelDef level)
	{
		App.Instance.StartLevel (level);
	}

	private void OnLeaderboardClick(LevelDb.LevelDef level)
	{
		App.Instance.SocialService.ShowSpecificLeaderBoard(level.LeaderboardId);
	}

}
