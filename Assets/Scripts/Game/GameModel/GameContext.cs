using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameContext
{
    public LevelDb.LevelDef LevelDef { get; set; }
    public GameModel Model { get; set; }
    public Game Controller { get; set; }
}
