using System;
using System.Collections.Generic;
using System.Linq;

 public class SpecialAbilityManager
 {
     private List<SpecialAbility> Abilities { get; set; }

     private GameContext Context { get; set; }

     public SpecialAbilityManager(GameContext context)
     {
         Context = context;

         Abilities = new List<SpecialAbility>();  
     }

     public void Run(SpecialAbility ability)
     {
         bool oneTimeUsage = ability.Start(Context);

         if (!oneTimeUsage)
         {
             Abilities.Add(ability);
         }
         else
         {
             ability.Finish();
         }
     }

     public void Update()
     {
         var toRemove = Abilities.FindAll(x => x.Update());
         toRemove.ForEach(x => x.Finish());
         Abilities.RemoveAll(x => toRemove.Contains(x));
     }
 }
