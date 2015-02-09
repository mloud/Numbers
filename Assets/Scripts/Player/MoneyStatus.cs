using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;



namespace GameStatus
{

    public class MoneyStatus
    {

		public int Pogs { get { return pogs; } }

        private int pogs;


		public MoneyStatus()
        {
			pogs = 100;// inital pogs
		}

        public void Reset()
        {
            UnityEngine.PlayerPrefs.DeleteKey("MoneyStatus");
        }


		public void AddPogs(int count)
		{
			pogs += count;
		}

		public bool UsePogs(int count)
		{
			if (pogs >= count)
			{
				pogs -= count;
				return true;
			}

			return false;
		}


        public JSONNode GetAsJSON()
        {
            JSONClass root = new JSONClass();

            // last level reached
            root["saveVersion"] = Versions.SaveData;
            root["pogs"].AsInt = Pogs;

            Core.Dbg.Log("MoneyStatus.GetAsJson() " + root.ToString());

            return root;
        }

        public void Load(JSONNode jsonNode, bool allowMerge)
        {
            if (jsonNode == null)
            {
                Core.Dbg.Log("MoneyStatus.Load() jsoNode is null -> no MoneyStatus found", Core.Dbg.MessageType.Warning);
                return; 
            }

            string version = jsonNode["saveVersion"].Value;

			if (allowMerge)
			{
				pogs = Math.Max(pogs, jsonNode["pogs"].AsInt);
			}
			else
			{
				pogs = jsonNode["pogs"].AsInt;
			}
        }

    }
}

   
     