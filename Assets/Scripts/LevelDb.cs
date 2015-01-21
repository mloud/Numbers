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
      	public float TotalTime;
		public float MicroTime;
		public float FlipTime;
		public string Name;
		public List<int> Numbers;
		public List<int> FlipNumbers;
		public List<float> BonusTime;
        public List<string> SpecialitiesForNumbers;
        public List<float> Probabilities;

    }

	[SerializeField]
	public List<LevelDef> Levels;

	public int LevelsCount { get { return Levels.Count; } }


}
