using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

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
        public bool UseAnimation;
    }


    private IEnumerator PlaceCirclesCoroutine(float time)
    {
        var linker = (Resources.Load("Prefabs/Ui/Tutorial/PatternLinks") as GameObject).GetComponent<PatternToTutorial>();

        var circlePanelPrefab = Resources.Load<GameObject>("Prefabs/Ui/Tutorial/PnlNumbers");
        var circleMenuPrefab = Resources.Load<GameObject>("Prefabs/Ui/Tutorial/CircleMenu");
        var orPrefab = Resources.Load<GameObject>("Prefabs/Ui/Tutorial/OrPrefab");


        var panels = new List<RectTransform>(PnlPattern.GetComponentsInChildren<RectTransform>());
        panels.RemoveAt(0);
        int pnlCount = Parameters.Level.Patterns.Count + Parameters.Level.Patterns.Count - 1;
        
        for (int i = 0; i <panels.Count; ++i)
        {
            if (i >= pnlCount)
            {
                Destroy(panels[i].gameObject);
                panels[i] = null; 
            }
        }
        panels.RemoveAll(x => x == null);


        int pnlIndex = 0;
        foreach (var patternName in Parameters.Level.Patterns)
        {
           

            // fill panel
            var patternLink = linker.Links.Find(x => x.PatternName == patternName);

            foreach (var num in patternLink.Example)
            {
                var circleMenuGo = (GameObject.Instantiate(circleMenuPrefab) as GameObject);
                if (!Parameters.UseAnimation)
                    Destroy(circleMenuGo.GetComponent<Animation>());
                circleMenuGo.transform.SetParent(panels[pnlIndex]);
                circleMenuGo.transform.localScale = Vector3.one;

                circleMenuGo.GetComponentInChildren<Text>().text = num.ToString();
            }

            yield return new WaitForSeconds(time);

            pnlIndex++;

            // OR
            if (pnlIndex < pnlCount)
            {
                var orGo = (GameObject.Instantiate(orPrefab) as GameObject);
                
                if (!Parameters.UseAnimation)
                    Destroy(orGo.GetComponent<Animation>());

                orGo.transform.SetParent(panels[pnlIndex]);
                orGo.transform.localScale = Vector3.one;
            }
            yield return new WaitForSeconds(time);

            pnlIndex++;

        }
    }

    private void PlaceCircles(Window win)
    {
        StartCoroutine(PlaceCirclesCoroutine(Parameters.UseAnimation ? 1.0f : 0));
    }

	protected override void OnInit(object param)
	{
		Parameters = param as Param;

        BtnCancel.gameObject.SetActive(Parameters.ShowCancelButton);


        if (Parameters.UseAnimation)
            base.OpenFinished += PlaceCircles; // TODO
        else
            PlaceCircles(this);
    }

	public void OnPlayClick()
	{
     	
	}
}
