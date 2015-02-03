using System;
using UnityEngine;

 public class TimeFreezeSpecialAbility : SpecialAbility
 {
     private float Duration { get; set; }

     private float Timer { get; set; }

     protected override bool OnApply()
     {
         Timer = Time.time + Duration;

         return false;
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