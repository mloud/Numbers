using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;


public class LevelEditorController
{
    public Action<LevelDb.LevelDef> OnNumLimitsChange;
    public Action<LevelDb.LevelDef> OnProbabilityChange;


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

    public void CreateProbability(LevelDb.LevelDef level)
    {
        int count = level.ToNum - level.FromNum + 1;
        level.Probabilities = new List<float>(count);
        float equalProbab = 1.0f / count; 
        for (int i = 0; i < count; ++i)
        {
            level.Probabilities.Add(equalProbab);
        }

        if (OnProbabilityChange != null)
            OnProbabilityChange(level);
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

    public void CreatePatterns(LevelDb.LevelDef level)
    {
        level.Patterns = new List<string>();
    }

    public void AddPattern(LevelDb.LevelDef level, string pattern)
    {
        if (level.Patterns.Find(x => x== pattern) == null)
            level.Patterns.Add(pattern);
    }

    public void RemovePattern(LevelDb.LevelDef level, string pattern)
    {
        if (level.Patterns.Find(x => x== pattern) != null)
            level.Patterns.Remove(pattern);
    }

    public void CreateNumbers(LevelDb.LevelDef level)
    {
        int count = level.Rows * level.Cols;
        // numbers
        level.Numbers = new List<int>(count);
        for (int i = 0; i < level.Numbers.Capacity; ++i)
            level.Numbers.Add(0);

        //specialities for numbers
        level.SpecialitiesForNumbers = new List<string>(count);
        for (int i = 0; i < level.SpecialitiesForNumbers.Capacity; ++i)
            level.SpecialitiesForNumbers.Add("");
    }

    public void RandomizeInitialLevelNumbers(LevelDb.LevelDef level)
    {
        //var rndNum = GetRandomNumbers(level.Rows * level.Cols, level.FromNum, level.ToNum); 
        //level.Numbers = new List<int>(rndNum);
       
        int count = level.Rows * level.Cols;

        List<int> rndNums = new List<int>();

        if (level.Probabilities != null)
        {
            for (int i = 0; i < count; ++i)
                rndNums.Add(Utils.Randomizer.GetRandom(level.FromNum, level.ToNum, level.Probabilities.ToArray()));
        }
        else
        {
            for (int i = 0; i < count; ++i)
                rndNums.Add(Utils.Randomizer.GetRandom(level.FromNum, level.ToNum));
        }

       
        level.Numbers = rndNums;
    }

    public void SetNumLimits(int fromNum, int toNum, LevelDb.LevelDef level)
    {
        bool updated = false;
        if (level.FromNum != fromNum)
        {
            level.FromNum = fromNum;
            updated = true;
        }

        if (level.ToNum != toNum)
        {

            level.ToNum = toNum;
            updated = true;
        }

        if (updated)
        {
            CreateProbability(level);

            if (OnNumLimitsChange != null)
                OnNumLimitsChange(level);
        }

    }


    public void SetNumbers(LevelDb.LevelDef level, int colors)
    {
        level.Colors = colors;
    }
    

    private int[] GetRandomNumbers(int count, int from, int to)
    {
        int[] rnds = new int[count];

        for (int i = 0; i < count; ++i)
        {
            rnds[i] = UnityEngine.Random.Range(from, to);
        }

        return rnds;
    }


   
}