using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour 
{
	[SerializeField]
	private Button playLevelsButton;

    [SerializeField]
    private Button survivalButton;

    [SerializeField]
    private Button leaderboardButton;


	void Start ()
	{
		playLevelsButton.onClick.AddListener( () => { App.Instance.LoadScene(SceneDef.LevelSelection); } );
		survivalButton.onClick.AddListener( () => { App.Instance.StartSurvival(); } );
		leaderboardButton.onClick.AddListener(() => { App.Instance.Services.GetService<Srv.SocialService>().ShowLeaderBoard(); });


		App.Instance.Sound.PlayMusic("menu");
	}
	
	void Update () 
    {
		leaderboardButton.gameObject.SetActive(App.Instance.Services.GetService<Srv.SocialService>().IsLogged());
	}
}
