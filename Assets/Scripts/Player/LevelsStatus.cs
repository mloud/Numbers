using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;



namespace GameStatus
{

    public class LevelsStatus
    {

        public int LastReachedLevel 
        { 
            get
            {
                return lastReachedLevel;
            } 
            set 
            { 
                lastReachedLevel = value;
                App.Instance.Services.GetService<Srv.SaveGameService>().Save();
            }
        }
        
        public class LevelStatus
        {
            public string Name;
            public int BestScore;
        }

        private List<LevelStatus> FinishedLevels { get; set; }
        private int lastReachedLevel;


        public LevelsStatus()
        {
            FinishedLevels = new List<LevelStatus>();
        }

        public void Reset()
        {
            FinishedLevels.Clear();
            lastReachedLevel = 0;

            UnityEngine.PlayerPrefs.DeleteKey("LevelsStatus");
        }


        public LevelStatus GetStatus(string levelName)
        {
            return FinishedLevels.Find(x => x.Name == levelName);
        }

        public bool IsBestScore(string levelName, int score)
        {
            var status = GetStatus(levelName);

            if (status == null)
                return true;

            return score > status.BestScore;
        }


        public bool IsFinished(string levelName)
        {
            return GetStatus(levelName) != null;
        }

        public void Finish(string levelName, int score)
        {
            Core.Dbg.Log("LevelsStatus.Finish() " + levelName + " score " + score);

            var status = GetStatus(levelName);

            // finished for the first time
            if (status == null)
            {
                status = new LevelStatus();
                status.BestScore = score;
                status.Name = levelName;
                Core.Dbg.Log("LevelsStatus.Finish() - level " + levelName + " finished for the first time with score " + score);

                FinishedLevels.Add(status);
            }
            // already finished -> actualize best score
            else
            {
                status.BestScore = Math.Max(status.BestScore, score);
                Core.Dbg.Log("LevelsStatus.Finish() - level " + levelName + " already finished -> current best score " + status.BestScore);
            }


            // update lastreachaged level
            var leveldef = App.Instance.Db.LevelDb.Levels.Find(x => x.Name == levelName);
            lastReachedLevel = leveldef.Order + 1;

            App.Instance.Services.GetService<Srv.SaveGameService>().Save();
        }


        public JSONNode GetAsJSON()
        {
            JSONClass root = new JSONClass();

            // last level reached
            root["saveVersion"] = Versions.SaveData;
            root["lastLevelReached"].AsInt = LastReachedLevel;

            // level statuses
            var array = new JSONArray();
            
            foreach (var status in FinishedLevels)
            {
                var level = new JSONClass();
                level["name"] = status.Name;
                level["bestScore"].AsInt = status.BestScore;

                array.Add(level);
            }

            root["finishedLevels"] = array;

            Core.Dbg.Log("LevelsStatus.GetAsJson() " + root.ToString());

            return root;
        }

        public void Load(JSONNode jsonNode)
        {
            if (jsonNode == null)
            {
                Core.Dbg.Log("LevelsStatus.Load() jsoNode is null -> no LevelsStatus found", Core.Dbg.MessageType.Warning);
                return; 
            }
            
            FinishedLevels.Clear();

            string version = jsonNode["saveVersion"].Value;

            lastReachedLevel = jsonNode["lastLevelReached"].AsInt;

            JSONArray levels = jsonNode["finishedLevels"].AsArray;

            for (int i = 0; i < levels.Count; ++i )
            {
                var levelStatus = new LevelStatus();


                levelStatus.Name = levels[i]["name"].Value;
                levelStatus.BestScore = levels[i]["bestScore"].AsInt;

                FinishedLevels.Add(levelStatus);
            }
        }



    }
}

   
     