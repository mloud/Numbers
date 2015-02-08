using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Player
{

    public class PlayerStatus : MonoBehaviour
    {
        public LevelsStatus LevelsStatus { get; private set; }
        public SpecialAbilitiesStatus AbilitiesStatus { get; private set; }

        private void Awake()
        {
            LevelsStatus = new LevelsStatus();
            AbilitiesStatus = new SpecialAbilitiesStatus();
        }

        public void Reset()
        {
            LevelsStatus.Reset();
            AbilitiesStatus.Reset();

            PlayerPrefs.DeleteAll();
        }

   

        public void Load()
        {
            LevelsStatus.Load();
            AbilitiesStatus.Load();
        }

        private void Save()
        {
            LevelsStatus.Save();
            AbilitiesStatus.Save();
        }
    }
}
