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


    public void AddNewLevel()
    {
    }


}