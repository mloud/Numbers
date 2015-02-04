using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class LevelFailedWindow : Window 
{
    [SerializeField]
    Text txtScoreTitle;

    [SerializeField]
    Text txtRequiredScoreTitle;


	[SerializeField]
	Text txtScoreValue;

	[SerializeField]
	Text txtRequiredScoreValue;

    [SerializeField]
    Text txtLevelName;

    [SerializeField]
    Text txtFailed;
   

	[SerializeField]
	Button btnMenu;

	[SerializeField]
	Button btnRepeat;

	private Param Parameters { get; set; }

	public class Param
	{
		public int Score;
		public int RequiredScore;
		public string LevelName;
		public Action OnMenuClick;
		public Action OnRestartClick;
	}

	protected override void OnInit(object param)
	{
		Parameters = param as Param;

		txtLevelName.text = Parameters.LevelName;
		txtScoreValue.text = Parameters.Score.ToString ();
		txtRequiredScoreValue.text = Parameters.RequiredScore.ToString ();
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


	protected override void OnOpen()
	{
		App.Instance.Sound.PlayEffect("levelFailed");

     
        txtScoreTitle.enabled = false;
        txtScoreValue.enabled = false;
        txtRequiredScoreValue.enabled = false;
        txtRequiredScoreTitle.enabled = false;
        txtLevelName.enabled = false;
        txtFailed.enabled = false;

        txtLevelName.GetComponent<SimpleAnimationExt>().RunIn(0);
        txtFailed.GetComponent<SimpleAnimationExt>().RunIn(0);
        txtScoreTitle.GetComponent<SimpleAnimationExt>().RunIn(1);
        txtScoreValue.GetComponent<SimpleAnimationExt>().RunIn(1.5f);

        txtRequiredScoreTitle.GetComponent<SimpleAnimationExt>().RunIn(2);
        txtRequiredScoreValue.GetComponent<SimpleAnimationExt>().RunIn(2.5f);

	}

}
