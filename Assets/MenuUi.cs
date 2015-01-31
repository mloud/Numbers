using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour 
{
	[SerializeField]
	private Button playLevelsButton;

	[SerializeField]
	private Button survivalButton;


	void Start ()
	{
		playLevelsButton.onClick.AddListener( () => { App.Instance.LoadScene(SceneDef.LevelSelection); } );
		survivalButton.onClick.AddListener( () => { App.Instance.StartSurvival(); } );

	}
	
	void Update () {
	
	}
}
