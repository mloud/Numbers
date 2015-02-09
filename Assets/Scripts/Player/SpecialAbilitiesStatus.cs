using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace GameStatus
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
                App.Instance.Services.GetService<Srv.SaveGameService>().Save();

                Core.Dbg.Log("SpecialAbilitiesStatus.Unlock() " + name + " successful");
            }
        }




        public JSONNode GetAsJSON()
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

            Core.Dbg.Log("SpecialAbilitiesStatus.GetAsJson() " + root.ToString());

            return root;
        }

        public void Load(JSONNode jsonNode)
        {
            if (jsonNode == null)
            {
                Core.Dbg.Log("SpecialAbilitiesStatus.LoadFromJSON() jsoNode is null -> no record found", Core.Dbg.MessageType.Warning);
                return;
            }

            UnlockAbilities.Clear();


            string version = jsonNode["saveVersion"].Value;


            JSONArray abilities = jsonNode["unlockedAbilities"].AsArray;

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

   
     