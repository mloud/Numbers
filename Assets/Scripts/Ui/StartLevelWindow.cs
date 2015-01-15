using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StartLevelWindow : Window 
{
	[SerializeField]
	private Text title;

	private Param Parameters { get; set; }

	public class Param
	{
		public LevelDb.LevelDef LevelDef;
	}

	protected override void OnInit(object param)
	{
		Parameters = param as Param;

		title.text = "Level " + (Parameters.LevelDef.Order + 1).ToString ();
	}
}
