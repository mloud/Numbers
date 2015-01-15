using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class GameFinishedWindow : Window 
{

	private Param Parameters { get; set; }

	public class Param
	{
		public Action OnMenuClick;
	}

	protected override void OnInit(object param)
	{
		Parameters = param as Param;
	}


	public void OnMenuClick()
	{
		WindowManager.Instance.CloseWindow (Name);	
		
		Parameters.OnMenuClick ();
	}
}
