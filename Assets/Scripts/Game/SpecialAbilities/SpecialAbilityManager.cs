using System;
using System.Collections.Generic;

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
         bool oneTimeUsage = ability.Apply(Context);

         if (!oneTimeUsage)
         {
             Abilities.Add(ability);
         }
     }

     public void Update()
     {
         Abilities.RemoveAll(x => x.Update());
     }
 }
