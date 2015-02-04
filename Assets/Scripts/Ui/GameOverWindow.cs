using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class GameOverWindow : Window 
{
    [SerializeField]
    Text txtCompleted;

    [SerializeField]
    Text txtLevelName;


    [SerializeField]
    Text txtScoreTitle;

    [SerializeField]
    ScoreCounter scoreValue;

    [SerializeField]
    Text txtScoreValue;


    [SerializeField]
    Text txtBestScoreTitle;

	[SerializeField]
	Text txtBestScoreValue;


    [SerializeField]
    Button btnNextLevel;

    [SerializeField]
    Button btnOk;

	[SerializeField]
	Button btnMenu;

	[SerializeField]
	Button btnRepeat;

	[SerializeField]
	Button btnLeaderboard;


	private Param Parameters { get; set; }

	public class Param
	{
		public int Score;
		public int BestScore;
		public string LevelName;
		public Action OnNextClick;
		public Action OnRestartClick;
		public Action OnMenuClick;
		public bool IsNextLevel;
		public LevelDb.LevelDef LevelDef;
	}

	protected override void OnInit(object param)
	{
		Parameters = param as Param;

		txtLevelName.text = Parameters.LevelName;
        scoreValue.Set(Parameters.Score);
		txtBestScoreValue.text = Parameters.BestScore.ToString ();
		btnNextLevel.gameObject.SetActive (Parameters.IsNextLevel);

        btnRepeat.gameObject.SetActive(Parameters.IsNextLevel);

        btnOk.gameObject.SetActive(!Parameters.IsNextLevel);

		btnLeaderboard.gameObject.SetActive(App.Instance.SocialService.IsLogged());
		btnLeaderboard.onClick.AddListener( () => 
		{
			App.Instance.SocialService.ShowSpecificLeaderBoard(Parameters.LevelDef.LeaderboardId);
		});



	}

	protected override void OnOpen()
	{
		App.Instance.Sound.PlayEffect("levelFinished");

        // start aniamtions
        txtLevelName.enabled = false;
        txtCompleted.enabled = false;
        txtScoreTitle.enabled = false;
	    txtScoreValue.enabled = false;
        txtBestScoreTitle.enabled = false;
        txtBestScoreValue.enabled = false;

        txtLevelName.GetComponent<SimpleAnimationExt>().RunIn(0);
        txtCompleted.GetComponent<SimpleAnimationExt>().RunIn(0);
        txtScoreTitle.GetComponent<SimpleAnimationExt>().RunIn(1);
        txtScoreValue.GetComponent<SimpleAnimationExt>().RunIn(1.5f);
        txtBestScoreTitle.GetComponent<SimpleAnimationExt>().RunIn(2);
        txtBestScoreValue.GetComponent<SimpleAnimationExt>().RunIn(2.5f);

        
	}

	public void OnNextClick()
	{
        App.Instance.WindowManager.CloseWindow(Name);	

		Parameters.OnNextClick ();
	}

	public void OnMenuClick()
	{
        App.Instance.WindowManager.CloseWindow(Name);	
		
		Parameters.OnMenuClick ();
	}

	public void OnRestartClick()
	{
        App.Instance.WindowManager.CloseWindow(Name);	
		
		Parameters.OnRestartClick ();
	}

    public void OnOkClick()
    {
        App.Instance.WindowManager.CloseWindow(Name);  
    }

}
