using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialWindow : Window 
{
	private Param Parameters { get; set; }

	public class Param
	{
		public Action OnPlayClick;
	}

	protected override void OnInit(object param)
	{
		Parameters = param as Param;
	}

	public void OnPlayClick()
	{
		WindowManager.Instance.CloseWindow (Name);	
	}
}
