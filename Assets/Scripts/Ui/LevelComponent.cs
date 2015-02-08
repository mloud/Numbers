using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LevelComponent : MonoBehaviour
{
	[SerializeField]
	private Text txtLevel;

	[SerializeField]
	private Transform alreadyPlayerIcon;

	[SerializeField]
	private Transform lockIcon;

	[SerializeField]
	private Text txtScore;

	[SerializeField]
	private Transform rankTransform;

	[SerializeField]
	private Text txtRanking;

	[SerializeField]
	private Text txtRankingFrom;

	[SerializeField]
	private Transform leaderboardTransform;


	[SerializeField]
	private Button levelButton;

	[SerializeField]
	private Button leaderboardButton;

	public Button LevelButton { get { return levelButton; } }

	public Button LeaderboardButton { get { return leaderboardButton; } }


	private LevelDb.LevelDef LevelDef { get; set; }


	public void Set(LevelDb.LevelDef levelDef)
	{
		LevelDef = levelDef;
	
		Refresh ();
	}

	public void OnClick()
	{
		Core.Dbg.Log ("LevelComponent.OnClick() " + LevelDef.Order);
	}

	private void SetRanking(int pos, int from, bool visible)
	{
		rankTransform.gameObject.SetActive(visible);
		txtRanking.text = pos.ToString();

		txtRankingFrom.text = pos.ToString();
	}

	private void Refresh()
	{
		bool isUncloked = Db.DbUtils.IsLevelUnlocked (LevelDef);

		txtLevel.text = (LevelDef.Order + 1).ToString ();

		Player.LevelsStatus.LevelStatus levelStatus = Db.DbUtils.GetLevelStatus (LevelDef);

		alreadyPlayerIcon.gameObject.SetActive (levelStatus != null);
		txtScore.gameObject.SetActive (levelStatus != null);

		txtLevel.gameObject.SetActive (isUncloked);
		lockIcon.gameObject.SetActive (!isUncloked);

		if (levelStatus != null)
		{
			txtScore.text = levelStatus.BestScore.ToString();
		}


		bool scoreRecExist = false;
		int rank = 0;
		int from = 0;

	
		if (App.Instance.SocialService.IsLogged())
		{
			App.Instance.SocialService.LoadScores(LevelDef.LeaderboardId, (UnityEngine.SocialPlatforms.IScore[] scores) =>
			{
				var scoreRec = Array.Find(scores, x => x.userID == App.Instance.SocialService.UserId);

				if (scoreRec != null)
				{
					rank = scoreRec.rank;
					from = scores.Length;
					scoreRecExist = true;
				}
			});
		}

		SetRanking(rank, from, scoreRecExist);

		leaderboardTransform.gameObject.SetActive(App.Instance.SocialService.IsLogged());
		

	}


	void Start ()
	{}
	
	void Update () 
	{}
}
