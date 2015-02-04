using System;
using UnityEngine;

 public class TimeFreezeSpecialAbility : SpecialAbility
 {
   
     private float Timer { get; set; }


     public TimeFreezeSpecialAbility(LevelDb.LevelDef.SpecialAbility def, SpecialAbilityVisual visual)
         : base(def, visual)
     { }

     protected override bool OnApply()
     {
         Timer = Def.Duration;

         return false;
     }

     public float GetProgress()
     {
         return Timer / Def.Duration;
     }
     
     public override bool Update()
     {
         Timer -= Time.deltaTime;

         bool finished = Timer <= 0;

         if (!finished)
         {
             Context.Controller.LevelTimer += Time.deltaTime; // stop timer by compensating frame time
             Context.Controller.MicroTimer += Time.deltaTime; // stop timer by compensating frame time
         }

         return finished;
     }
 }