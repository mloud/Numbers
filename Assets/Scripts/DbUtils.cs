using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class DbUtils 
{
	public static LevelDb.LevelDef GetNextLevel(LevelDb.LevelDef level)
	{
		return App.Instance.Db.LevelDb.Levels.Find (x => x.Order == (level.Order + 1));
	}

	public static bool IsLevelUnlocked(LevelDb.LevelDef level)
	{
		return level.Order <= App.Instance.Player.LastReachedLevel;
	}

	public static LevelDb.LevelDef GetLevelByOrder(int order)
	{
		return App.Instance.Db.LevelDb.Levels.Find (x => x.Order == order);
	}

	public static bool IsGameFinished()
	{
		int maxOrder = 0;
		App.Instance.Db.LevelDb.Levels.ForEach (x => maxOrder = Mathf.Max (maxOrder, x.Order));

		return maxOrder == App.Instance.Player.LastReachedLevel && IsLevelFinished(GetLevelByOrder(maxOrder));
	}

	public static bool IsLevelFinished(LevelDb.LevelDef level)
	{
		return App.Instance.Player.CompletedLevels.ContainsKey (level.Name);
	}

	public static Player.LevelStatistic GetLevelStatistic(LevelDb.LevelDef level)
	{
		if (App.Instance.Player.CompletedLevels.ContainsKey (level.Name))
		{
			return App.Instance.Player.CompletedLevels[level.Name];
		}
		return null;
	}

}
