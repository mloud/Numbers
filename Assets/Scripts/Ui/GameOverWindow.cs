using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class GameOverWindow : Window 
{
	[SerializeField]
	Text txtScoreValue;

	[SerializeField]
	Button btnNextLevel;

	private Param Parameters { get; set; }

	public class Param
	{
		public int Score;
		public Action OnNextClick;
		public Action OnRestartClick;
		public Action OnMenuClick;
		public bool IsNextLevel;

	}

	protected override void OnInit(object param)
	{
		Parameters = param as Param;

		txtScoreValue.text = Parameters.Score.ToString ();

		btnNextLevel.gameObject.SetActive (Parameters.IsNextLevel);
	}

	public void OnNextClick()
	{
		WindowManager.Instance.CloseWindow (Name);	

		Parameters.OnNextClick ();
	}

	public void OnMenuClick()
	{
		WindowManager.Instance.CloseWindow (Name);	
		
		Parameters.OnMenuClick ();
	}

	public void OnRestartClick()
	{
		WindowManager.Instance.CloseWindow (Name);	
		
		Parameters.OnRestartClick ();
	}

}
