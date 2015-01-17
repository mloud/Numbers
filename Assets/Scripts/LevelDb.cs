using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelDb : MonoBehaviour 
{
	[System.Serializable]
	public class LevelDef
	{
		public int Order;
		public int FromNum;
		public int ToNum;
		public int Rows;
		public int Cols;
		public int Score;
		public float TotalTime;
		public float MicroTime;
		public float FlipTime;
		public string Name;
		public List<int> Numbers;
		public List<int> FlipNumbers;
	}

	[SerializeField]
	public List<LevelDef> Levels;

	public int LevelsCount { get { return Levels.Count; } }


}
