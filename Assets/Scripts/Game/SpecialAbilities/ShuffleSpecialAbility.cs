using System;
using UnityEngine;

 public class ShuffleSpecialAbility : SpecialAbility
 {

     public ShuffleSpecialAbility(SpecialAbilityDb.SpecialAbility def, SpecialAbilityVisual visual)
         : base(def, visual)
     { }

     protected override bool OnApply()
     {
         var probabilities = Context.LevelDef.Probabilities.ToArray();

         Context.Model.Circles.ForEach( (CircleController circle) => 
         {
             circle.ChangeValueTo(Utils.Randomizer.GetRandom(Context.LevelDef.FromNum, Context.LevelDef.ToNum, probabilities));
         });

         return true;
     }

     
     public override bool Update()
     {
         return true;
     }

     protected override void OnFinished()
     {
         //nothing now
     }

 }