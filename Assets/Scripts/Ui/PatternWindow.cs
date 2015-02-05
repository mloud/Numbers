﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PatternWindow : Window 
{

    [SerializeField]
    public Button BtnCancel;

    [SerializeField]
    private RectTransform PnlPattern;

	private Param Parameters { get; set; }

	public class Param
	{
        public LevelDb.LevelDef Level;
        public bool ShowCancelButton;
    }

	protected override void OnInit(object param)
	{
		Parameters = param as Param;

        BtnCancel.gameObject.SetActive(Parameters.ShowCancelButton);

     

        var linker = (Resources.Load("Prefabs/Ui/Tutorial/PatternLinks") as GameObject).GetComponent<PatternToTutorial>();

        var circleMenuPrefab = Resources.Load<GameObject>("Prefabs/Ui/Tutorial/CircleMenu");
        var circlePanelPrefab = Resources.Load<GameObject>("Prefabs/Ui/Tutorial/PnlNumbers");

        foreach(var patternName in Parameters.Level.Patterns)
        {
            // create panel
            var pnlPattern = (GameObject.Instantiate(circlePanelPrefab) as GameObject).GetComponent<RectTransform>();
            pnlPattern.SetParent(PnlPattern);
            pnlPattern.localScale = Vector3.one;


            // fill panel
            var patternLink = linker.Links.Find(x=>x.PatternName == patternName);

            foreach (var num in patternLink.Example)
            {
                var circleMenuGo = (GameObject.Instantiate(circleMenuPrefab) as GameObject);
                circleMenuGo.transform.SetParent(pnlPattern);
                circleMenuGo.transform.localScale = Vector3.one;

                circleMenuGo.GetComponentInChildren<Text>().text = num.ToString();
            }
        }


    }

	public void OnPlayClick()
	{
     	
	}
}