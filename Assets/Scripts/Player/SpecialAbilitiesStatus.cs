using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Player
{

    public class SpecialAbilitiesStatus
    {
        public class SpecialAbilityStatus
        {
            public string Name;
            public int Count;
        }

        private List<SpecialAbilityStatus> UnlockAbilities { get; set; }

        public void UseAbilities(List<string> abilities)
        {
            foreach(var abName in abilities)
            {
                var status = UnlockAbilities.Find(x=>x.Name == abName);
                
                Core.Dbg.Assert(status != null, "SpecialAbilitiesStatus.UseAbilities() " + abName + " NOT UNLOCKED");

                if (status != null)
                {
                    Core.Dbg.Assert(status.Count > 0, "SpecialAbilitiesStatus.UseAbilities() " + abName + " has no count left");

                    status.Count = Math.Max(0, status.Count - 1);
                }
                
            }
        
        }

        public SpecialAbilitiesStatus()
        {
            UnlockAbilities = new List<SpecialAbilityStatus>();
        }

        public void Reset()
        {
            UnlockAbilities.Clear();
        }

        public SpecialAbilityStatus GetStatus(string name)
        {
            return UnlockAbilities.Find(x => x.Name == name);
        }

        public bool IsUnlock(string name)
        {
            return GetStatus(name) != null;
        }

        public void Unlock(string name)
        {
            Core.Dbg.Assert(!IsUnlock(name), "SpecialAbilityStatus.Unlock() " + name + " ability already unlocked!");

            if (!IsUnlock(name))
            {
                // find def.
                var abilityDef = App.Instance.Db.SpecialAbilityDb.SpecialAbilities.Find(x => x.Name == name);

                var specialAbilityStatus = new SpecialAbilityStatus();

                specialAbilityStatus.Name = abilityDef.Name;
                specialAbilityStatus.Count = abilityDef.InitialCount;

                UnlockAbilities.Add(specialAbilityStatus);
            
                // Save
                Save();

                Core.Dbg.Log("SpecialAbilitiesStatus.Unlock() " + name + " successful");
            }
        }




        public void Save()
        {
            JSONClass root = new JSONClass();

            root["saveVersion"] = Versions.SaveData;
           

            // level statuses
            var array = new JSONArray();

            foreach (var ability in UnlockAbilities)
            {
                var abilityJson = new JSONClass();
                abilityJson["name"] = ability.Name;
                abilityJson["count"].AsInt = ability.Count;

                array.Add(abilityJson);
            }

            root["unlockedAbilities"] = array;

            Core.Dbg.Log("Json:" + root.ToString());

            UnityEngine.PlayerPrefs.SetString("AbilitiesStatus", root.ToString());
        }

        public void Load()
        {
            string str = UnityEngine.PlayerPrefs.GetString("AbilitiesStatus");

            if (string.IsNullOrEmpty(str))
            {
                Core.Dbg.Log("SpecialAbilitiesStatus.Load() no record found");
                return;
            }

            UnlockAbilities.Clear();


            var root = JSON.Parse(str);

            string version = root["saveVersion"].Value;


            JSONArray abilities = root["unlockedAbilities"].AsArray;

            for (int i = 0; i < abilities.Count; ++i)
            {
                var status = new SpecialAbilityStatus();
                
                status.Name = abilities[i]["name"].Value;
                status.Count = abilities[i]["count"].AsInt;

                UnlockAbilities.Add(status);
            }
        }

        
    }
}

   
     