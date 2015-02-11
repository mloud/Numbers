using System;
using System.Collections.Generic;

namespace Evt
{
    public class LevelFinished : Event
    {
        public bool Win;
        public int Score;
        public LevelDb.LevelDef LeveDef;
    }
}
