using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PrelevelWindow : Window 
{
    [SerializeField]
    Text LevelNameText;

    [SerializeField]
    Text GoalValueText;


	private Param Parameters { get; set; }

	public class Param
	{
		public Action OnPlayClick;
        public LevelDb.LevelDef Level;
	}

	protected override void OnInit(object param)
	{
		Parameters = param as Param;

        LevelNameText.text = Parameters.Level.Name;
        GoalValueText.text = Parameters.Level.Score.ToString();
    }

	public void OnPlayClick()
	{
     	
	}
}
