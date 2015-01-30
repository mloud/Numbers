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



	public Button Button { get; private set; }

	private LevelDb.LevelDef LevelDef { get; set; }

	private void Awake()
	{
		Button = GetComponent<Button> ();
	}

	public void Set(LevelDb.LevelDef levelDef)
	{
		LevelDef = levelDef;
	
		Refresh ();
	}

	public void OnClick()
	{
		Debug.Log ("LevelComponent.OnClick() " + LevelDef.Order);
	}

	private void SetRanking(int pos, int from, bool visible)
	{
		rankTransform.gameObject.SetActive(visible);

		txtRanking.text = pos.ToString();

		txtRankingFrom.text = pos.ToString();
	}

	private void Refresh()
	{
		bool isUncloked = DbUtils.IsLevelUnlocked (LevelDef);

		txtLevel.text = (LevelDef.Order + 1).ToString ();

		Num.NPlayer.LevelStatistic lStats = DbUtils.GetLevelStatistic (LevelDef);

		alreadyPlayerIcon.gameObject.SetActive (lStats != null);
		txtScore.gameObject.SetActive (lStats != null);

		txtLevel.gameObject.SetActive (isUncloked);
		lockIcon.gameObject.SetActive (!isUncloked);

		if (lStats != null)
		{
			txtScore.text = lStats.Score.ToString();
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
	}


	void Start ()
	{}
	
	void Update () 
	{}
}
