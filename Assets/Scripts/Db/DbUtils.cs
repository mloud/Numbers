using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;


namespace Data
{
    public static class DbUtils
    {
        public static LevelDb.LevelDef GetNextLevel(LevelDb.LevelDef level)
        {
            return App.Instance.Db.LevelDb.Levels.Find(x => x.Order == (level.Order + 1));
        }

        public static bool IsLevelUnlocked(LevelDb.LevelDef level)
        {
            return level.Order <= App.Instance.PlayerStatus.LevelsStatus.LastReachedLevel;
        }

        public static LevelDb.LevelDef GetLevelByOrder(int order)
        {
            return App.Instance.Db.LevelDb.Levels.Find(x => x.Order == order);
        }

        public static bool IsLastLevel(LevelDb.LevelDef level)
        {
            int maxOrder = 0;
            App.Instance.Db.LevelDb.Levels.ForEach(x => maxOrder = Mathf.Max(maxOrder, x.Order));

            return maxOrder == level.Order;
        }

        public static bool IsGameFinished()
        {
            int maxOrder = 0;
            App.Instance.Db.LevelDb.Levels.ForEach(x => maxOrder = Mathf.Max(maxOrder, x.Order));

            return maxOrder == App.Instance.PlayerStatus.LevelsStatus.LastReachedLevel && IsLevelFinished(GetLevelByOrder(maxOrder));
        }

        public static bool IsLevelFinished(LevelDb.LevelDef level)
        {
            return App.Instance.PlayerStatus.LevelsStatus.IsFinished(level.Name);
        }

        public static GameStatus.LevelsStatus.LevelStatus GetLevelStatus(LevelDb.LevelDef level)
        {
            if (App.Instance.PlayerStatus.LevelsStatus.IsFinished(level.Name))
            {
                return App.Instance.PlayerStatus.LevelsStatus.GetStatus(level.Name);
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

        public static void MergeLevelDb(LevelDb leveDb, string jsonString)
        {
            var jsonRoot =  JSON.Parse(jsonString);

            var levelArray = jsonRoot as JSONArray;

            int levelcount = levelArray.Count;

            for (int i = 0; i < levelcount; ++i)
            {
                var level = levelArray[i];

                string name = level["Name"].Value;
                int sizeX = (int)level["SizeX"].AsFloat;
                int sizeY = (int)level["SizeY"].AsFloat;
                int fromNum = (int)level["NumFrom"].AsFloat;
                int toNum = (int)level["NumTo"].AsFloat;
                int score = (int)level["Score"].AsFloat;
                float time = level["Time"].AsFloat;
                float utime = level["UTime"].AsFloat;
                float refillTime = level["RefillTime"].AsFloat;
                float flipTime = level["FlipTime"].AsFloat;
                string goal = level["Goal"].Value;
                string patterns = level["Patterns"];
                JSONArray matrix = level["Matrix"].AsArray;

                var currLevel = leveDb.Levels.Find(x => x.Name == name);

                if (currLevel == null)
                {
                    currLevel = new LevelDb.LevelDef();
                    leveDb.Levels.Add(currLevel);
                }
                
                currLevel.Cols = sizeX;
                currLevel.Rows = sizeY;
                currLevel.FromNum = fromNum;
                currLevel.ToNum = toNum;
                currLevel.Score = score;
                currLevel.TotalTime = time;
                currLevel.MicroTime = utime;
                currLevel.RefillTime = refillTime;
                currLevel.FlipTime = flipTime;
                currLevel.Goal = goal;
                currLevel.Name = name;

                var patternArray = patterns.Split(',').ToList<string>();
                currLevel.Patterns = patternArray;
                
                // custom matrix
                if (matrix != null && matrix.Count > 0)
                {
                    currLevel.Numbers = new List<int>(matrix.Count);
                    for (int j = 0; j < matrix.Count; ++j)
                        currLevel.Numbers.Add(matrix[j].AsInt);
                }

                // if no matrx at all -> generate random
                if ((matrix == null || matrix.Count == 0) && currLevel.Numbers == null)
                {
                    int count = currLevel.Cols * currLevel.Rows;
                    currLevel.Numbers = new List<int>(count);
                    for (int j = 0; j < count; ++j)
                        currLevel.Numbers.Add(Utils.Randomizer.GetRandom(currLevel.FromNum, currLevel.ToNum));
                }

                if ( (currLevel.RefillTime > 0 || currLevel.FlipTime > 0) && (currLevel.FlipNumbers == null || currLevel.FlipNumbers.Count == 0))
                {
                    int count = (int)((currLevel.TotalTime / currLevel.FlipTime) + (currLevel.TotalTime / currLevel.RefillTime));
                    currLevel.FlipNumbers = new List<int>(count);
                    for (int j = 0; j < count; ++j)
                        currLevel.FlipNumbers.Add(Utils.Randomizer.GetRandom(currLevel.FromNum, currLevel.ToNum));
                }


                if (currLevel.SpecialitiesForNumbers == null)
                {
                    int count = currLevel.Rows * currLevel.Cols;
                    currLevel.SpecialitiesForNumbers = new List<string>(count);
                    for (int j = 0; j < count; ++j)
                        currLevel.SpecialitiesForNumbers.Add("");

                }


            }

        
        }

    }
}
