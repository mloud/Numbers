﻿using UnityEngine;
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
	Button btnRepeat;

	[SerializeField]
	Button btnLeaderboard;


	private Param Parameters { get; set; }

	public class Param
	{
        public Action OnAnimsFinished;
		public int Score;
		public int BestScore;
		public string LevelName;
		public Action OnNextClick;
		public Action OnRestartClick;
	    public Action OnOkClick;
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

		btnLeaderboard.gameObject.SetActive(App.Instance.Services.GetService<Srv.SocialService>().IsLogged());
		btnLeaderboard.onClick.AddListener( () => 
		{
			App.Instance.Services.GetService<Srv.SocialService>().ShowSpecificLeaderBoard(Parameters.LevelDef.LeaderboardId);
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

        Invoke("AnimsFinished", 3.0f);
	}

    private void AnimsFinished()
    {
        Parameters.OnAnimsFinished();
    }

	public void OnNextClick()
	{
        App.Instance.WindowManager.CloseWindow(Name);	

		Parameters.OnNextClick ();
	}

	public void OnRestartClick()
	{
        App.Instance.WindowManager.CloseWindow(Name);	
		
		Parameters.OnRestartClick ();
	}

    public void OnOkClick()
    {
        App.Instance.WindowManager.CloseWindow(Name);

        Parameters.OnOkClick();
    }

}
