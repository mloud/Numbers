using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

	private void Refresh()
	{
		bool isUncloked = DbUtils.IsLevelUnlocked (LevelDef);

		txtLevel.text = (LevelDef.Order + 1).ToString ();

		Player.LevelStatistic lStats = DbUtils.GetLevelStatistic (LevelDef);

		alreadyPlayerIcon.gameObject.SetActive (lStats != null);
		txtScore.gameObject.SetActive (lStats != null);

		txtLevel.gameObject.SetActive (isUncloked);
		lockIcon.gameObject.SetActive (!isUncloked);

		if (lStats != null)
		{
			txtScore.text = lStats.Score.ToString();
		}
	}

	void Start ()
	{}
	
	void Update () 
	{}
}
