using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialWindow : Window 
{
    [SerializeField]
    private RectTransform PnlPattern;

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
		WindowManager.Instance.CloseWindow (Name);	
	}
}
