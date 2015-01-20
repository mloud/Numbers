using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class LevelEditorController
{
    private Db Db { get; set; }

    public List<LevelDb.LevelDef> Levels { get { return Db.LevelDb.Levels; } }

    public LevelEditorController()
    {
        Db = (Resources.Load("Prefabs/Db/Db") as GameObject).GetComponent<Db>();
    }

    public LevelDb.LevelDef LevelDef;

    public void AddNewLevel()
    {
        LevelDb.LevelDef level = new LevelDb.LevelDef();

        level.Name = "new level";
        level.Order = Db.LevelDb.LevelsCount;

        Db.LevelDb.Levels.Add(level);
    }

    public void Save()
    {
        EditorUtility.SetDirty(Db.gameObject);
        EditorApplication.SaveAssets();
    }

    public void PlayLevel(LevelDb.LevelDef level)
    {
        Db.LevelDb.DefaultLevel = level.Order;
        bool res = EditorApplication.OpenScene("Assets/GameScene.unity");
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    public void DeleteLevel(LevelDb.LevelDef level)
    {
        Db.LevelDb.Levels.Remove(level);
    }

    public void CreateFlipNumbers(LevelDb.LevelDef level)
    {
        level.FlipNumbers = new List<int>(level.FlipNumbersCount);
        for (int i = 0; i < level.FlipNumbers.Capacity; ++i)
            level.FlipNumbers.Add(0);
    }

    public void RandomizeFlipNumbers(LevelDb.LevelDef level)
    {
        var rndNum = GetRandomNumbers(level.FlipNumbersCount, level.FromNum, level.ToNum); 
        level.FlipNumbers = new List<int>(rndNum);
    }

    public void CreateNumbers(LevelDb.LevelDef level)
    {
        level.Numbers = new List<int>(level.Rows * level.Cols);
        for (int i = 0; i < level.Numbers.Capacity; ++i)
            level.Numbers.Add(0);
    }

    public void RandomizeInitialLevelNumbers(LevelDb.LevelDef level)
    {
        var rndNum = GetRandomNumbers(level.Rows * level.Cols, level.FromNum, level.ToNum); 
        level.Numbers = new List<int>(rndNum);
    }

    private int[] GetRandomNumbers(int count, int from, int to)
    {
        int[] rnds = new int[count];

        for (int i = 0; i < count; ++i)
        {
            rnds[i] = Random.Range(from, to);
        }

        return rnds;
    }

}