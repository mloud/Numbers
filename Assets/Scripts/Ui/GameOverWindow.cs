using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class GameOverWindow : Window 
{
	[SerializeField]
	Text txtScoreValue;

	[SerializeField]
	Text txtBestScoreValue;

	[SerializeField]
	Text txtLevelName;

    [SerializeField]
    Button btnNextLevel;

    [SerializeField]
    Button btnOk;

	[SerializeField]
	Button btnMenu;

	[SerializeField]
	Button btnRepeat;

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
	}

	protected override void OnInit(object param)
	{
		Parameters = param as Param;

		txtLevelName.text = Parameters.LevelName;
		txtScoreValue.text = Parameters.Score.ToString ();
		txtBestScoreValue.text = Parameters.BestScore.ToString ();
		btnNextLevel.gameObject.SetActive (Parameters.IsNextLevel);

        btnRepeat.gameObject.SetActive(Parameters.IsNextLevel);
        btnMenu.gameObject.SetActive(Parameters.IsNextLevel);

        btnOk.gameObject.SetActive(!Parameters.IsNextLevel);
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
