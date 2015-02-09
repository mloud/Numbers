using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GameStatus
{

    public class PlayerStatus : MonoBehaviour
    {
        public LevelsStatus LevelsStatus { get; private set; }
        public SpecialAbilitiesStatus AbilitiesStatus { get; private set; }

      
        public void Init()
        {
            LevelsStatus = new LevelsStatus();
            AbilitiesStatus = new SpecialAbilitiesStatus();
        }

        public string GetSaveGame()
        {
            SimpleJSON.JSONNode levelsJson = LevelsStatus.GetAsJSON();
            SimpleJSON.JSONNode abilitiesJson = AbilitiesStatus.GetAsJSON();

            SimpleJSON.JSONClass root = new SimpleJSON.JSONClass();

            root["levelsStatus"] = levelsJson;
            root["abilitiesStatus"] = abilitiesJson;


            return root.ToString();
        }

        public void Reset()
        {
            LevelsStatus.Reset();
            AbilitiesStatus.Reset();
        }

        public void Load(string str, bool allowMerge)
        {
            var jsonRoot = SimpleJSON.JSON.Parse(str);

            if (jsonRoot != null)
            {
                LevelsStatus.Load(jsonRoot["levelsStatus"], allowMerge);
                AbilitiesStatus.Load(jsonRoot["abilitiesStatus"], allowMerge);
            }
        }
    }
}
