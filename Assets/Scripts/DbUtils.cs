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


    public static string SpecialityToString(LevelDb.SpecialityDef spec)
    {
        return spec.Name + "(" + spec.Param + ")";
    }

    public static string SpecialitiesToString(List<LevelDb.SpecialityDef> specs)
    {
        string res = "";

        foreach (var spec in specs)
        {
            res += SpecialityToString(spec);
        }

        return res;
    }

    public static List<LevelDb.SpecialityDef> SpecialitiesFromString(string s)
    {
        var specList = new List<LevelDb.SpecialityDef>();

        int index = 0;
        while (index < s.Length)
        {
            string name = s.Substring(index, 1);
            string param = "";

            int pL = s.IndexOf('(', index + 1);
           

            if (pL != -1)
            {
                int pR = s.IndexOf(')', pL + 1);

                param = s.Substring(pL + 1, pR - (pL + 1));
                index = pR + 1;
            }
            else
            {
                index++;
            }
        
            specList.Add(new LevelDb.SpecialityDef() { Name = name, Param = param });
        }

        return specList;
    }

}
