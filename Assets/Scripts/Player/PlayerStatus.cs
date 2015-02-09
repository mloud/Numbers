using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GameStatus
{

    public class PlayerStatus : MonoBehaviour
    {

		public MoneyStatus MoneyStatus {get; private set; }
        public LevelsStatus LevelsStatus { get; private set; }
        public SpecialAbilitiesStatus AbilitiesStatus { get; private set; }



        public void Init()
        {
			MoneyStatus = new MoneyStatus();
            LevelsStatus = new LevelsStatus();
            AbilitiesStatus = new SpecialAbilitiesStatus();
        }

        public string GetSaveGame()
        {

			SimpleJSON.JSONNode moneyJson = MoneyStatus.GetAsJSON();
            SimpleJSON.JSONNode levelsJson = LevelsStatus.GetAsJSON();
			SimpleJSON.JSONNode abilitiesJson = AbilitiesStatus.GetAsJSON();


            SimpleJSON.JSONClass root = new SimpleJSON.JSONClass();

            
			root["moneyStatus"] = moneyJson;
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
				MoneyStatus.Load(jsonRoot["moneyStatus"], allowMerge);
                LevelsStatus.Load(jsonRoot["levelsStatus"], allowMerge);
                AbilitiesStatus.Load(jsonRoot["abilitiesStatus"], allowMerge);
            }
        }
    }
}
