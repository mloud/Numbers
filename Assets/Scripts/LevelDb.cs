using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelDb : MonoBehaviour 
{   
   
    public int DefaultLevel = 0;
   
    [System.Serializable]
    public class SpecialityDef
    {
        public string Name;
        public string Param;
    }

	[System.Serializable]
	public class LevelDef
	{
		public int Order;
		public int FromNum;
		public int ToNum;
		public int Rows;
		public int Cols;
		public int Score;
        public int FlipNumbersCount;
        public int Colors;
        public float TotalTime;
		public float MicroTime;
		public float FlipTime;
        public float RefillTime;
        public string Name;
		public string LeaderboardId;
		public List<int> Numbers;
		public List<int> FlipNumbers;
		public List<float> BonusTime;
        public List<string> SpecialitiesForNumbers;
        public List<float> Probabilities;
        public List<string> Patterns;

        [System.Serializable]
        public class SpecialAbility
        {
            public float RechargeTime;
            public float Duration;
            public string Name;
        }

        public List<SpecialAbility> SpecialAbilities;
    }

	[SerializeField]
	public List<LevelDef> Levels;

	public int LevelsCount { get { return Levels.Count; } }


}
