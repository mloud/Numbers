using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialWindow : Window 
{
    [SerializeField]
    RectTransform PnlPattern;

	private Param Parameters { get; set; }

	public class Param
	{
		public Action OnPlayClick;
        public LevelDb.LevelDef Level;
	}

	protected override void OnInit(object param)
	{
		Parameters = param as Param;
	
        var linker = (Resources.Load("Prefabs/Ui/Tutorial/PatternLinks") as GameObject).GetComponent<PatternToTutorial>();

        foreach(var patternName in Parameters.Level.Patterns)
        {
            var pnlPattern = (GameObject.Instantiate(linker.Links.Find(x=>x.PatternName == patternName).TutorialPrefab) as GameObject).GetComponent<RectTransform>();
            pnlPattern.SetParent(PnlPattern);
            pnlPattern.localScale = Vector3.one;
        }


    }

	public void OnPlayClick()
	{
		WindowManager.Instance.CloseWindow (Name);	
	}
}
