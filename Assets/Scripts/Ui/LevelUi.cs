using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;


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


    private List<LevelComponent> Levels { get; set; }

    private float AnimDuration { get; set; }
    private int AnimIdex { get; set; }
    private float AnimTimer { get; set;  }

	void Start ()
	{
		resetButton.onClick.AddListener( () => { OnReset(); });
		backButton.onClick.AddListener( () => { OnBack(); });

		App.Instance.Sound.PlayMusic("menu");


		Init ();
	}

	public void Init()
	{
		//App.Instance.GoogleAnalytics.LogScreen ("LevelSelection");
	
		GameObject levelPrefab = Resources.Load ("Prefabs/Ui/Level") as GameObject;

        AnimDuration = 0.5f;
        AnimTimer = Time.time + AnimDuration;
        AnimIdex = 0;

        if (Levels != null)
            Levels.ForEach(x => Destroy(x.gameObject));
        

        Levels = new List<LevelComponent>();
	
		for (int i = 0; i < App.Instance.Db.LevelDb.Levels.Count; ++i)
		{
			var levelDef =  App.Instance.Db.LevelDb.Levels[i];

			var level = (Instantiate(levelPrefab) as GameObject).GetComponent<LevelComponent>();
	
			level.transform.SetParent(List);

			level.transform.localScale = Vector3.one;

			level.Set(levelDef);
#if !UNLOCK_LEVELS
			if (Db.DbUtils.IsLevelUnlocked(levelDef))
#endif
			level.LevelButton.onClick.AddListener ( () => OnLevelClick(levelDef));

			level.LeaderboardButton.onClick.AddListener( () => OnLeaderboardClick(levelDef));

            Levels.Add(level);
        }
	}

	void Update () 
	{
        if (Time.time > AnimTimer)
        {
            AnimTimer = Time.time + AnimDuration;

            Levels[AnimIdex].GetComponent<Animation>().Play();

            AnimIdex = (AnimIdex + 1) < Levels.Count ? AnimIdex + 1 : 0; 
        }

    }

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
