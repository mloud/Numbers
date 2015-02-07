using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Num
{

	public class NPlayer : MonoBehaviour 
	{
		public class LevelStatistic
		{
			public int Score;
		}

		public int LastReachedLevel 
		{
			get { return lastReachedLevel; } 
			private set { lastReachedLevel = value; Save(); }
		}

		public bool TutorialDone 
		{
			get { return tutorialDone; }
			set { tutorialDone = value; Save (); }
		}

		private int lastReachedLevel;
		private bool tutorialDone;

		public Dictionary<string, LevelStatistic> CompletedLevels { get; private set; }

		private void Awake()
		{
			lastReachedLevel = 0;
			tutorialDone = false;
			CompletedLevels = new Dictionary<string, LevelStatistic> ();
		}

		public void Reset()
		{
			PlayerPrefs.DeleteAll ();
			lastReachedLevel = 0;
		}

		public void LevelFinished(LevelDb.LevelDef level, int score)
		{
			var nextLevel = Db.DbUtils.GetNextLevel (level);
			if (nextLevel != null) 
			{
				LastReachedLevel = nextLevel.Order;
			}

			LevelStatistic levelStats = null;
			CompletedLevels.TryGetValue (level.Name, out levelStats);

			if (levelStats == null)
			{
				levelStats = new LevelStatistic() { Score = score };
				CompletedLevels.Add(level.Name, levelStats);
			}
			else
			{
				levelStats.Score = Mathf.Max (score, levelStats.Score);
			}

			Save ();
		}

		public void Load()
		{
			if (PlayerPrefs.HasKey("LastReachedLevel"))
			{
				lastReachedLevel = PlayerPrefs.GetInt("LastReachedLevel");
			}

			if (PlayerPrefs.HasKey("TutorialDone"))
			{
				tutorialDone = PlayerPrefs.GetInt("TutorialDone") == 0 ? false : true;
			}

			if (PlayerPrefs.HasKey("Levels"))
			{
				string s = PlayerPrefs.GetString("Levels");

				while (s.Length > 0)
				{
					int dotIndex = s.IndexOf(":");
					string levelName = s.Substring(0, dotIndex);
					int commaIndex = s.IndexOf(";");
					string score = s.Substring(dotIndex + 1, commaIndex - (dotIndex + 1));

					int scoreValue;
					int.TryParse(score, out scoreValue);

					s = s.Remove (0, commaIndex + 1);

					CompletedLevels.Add(levelName, new LevelStatistic() { Score = scoreValue });
				}

			}
		}

		private void Save()
		{
			PlayerPrefs.SetInt ("LastReachedLevel", LastReachedLevel);
			PlayerPrefs.SetInt ("TutorialDone", TutorialDone ? 1 : 0);

			string s = "";

			foreach (var pair in CompletedLevels)
			{
				s += pair.Key + ":" + pair.Value.Score + ";";
			}

			PlayerPrefs.SetString ("Levels", s);
		}
	}
}
