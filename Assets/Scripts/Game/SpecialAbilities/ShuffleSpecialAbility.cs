using System;
using UnityEngine;

 public class ShuffleSpecialAbility : SpecialAbility
 {

     public ShuffleSpecialAbility(SpecialAbilityDb.SpecialAbility def, SpecialAbilityVisual visual)
         : base(def, visual)
     { }

     protected override bool OnApply()
     {
         var probabilities = Context.LevelDef.Probabilities != null ? Context.LevelDef.Probabilities.ToArray() : null;

         Context.Model.Circles.ForEach( (CircleController circle) => 
         {
             if (probabilities != null)
                circle.ChangeValueTo(Utils.Randomizer.GetRandom(Context.LevelDef.FromNum, Context.LevelDef.ToNum, probabilities));
             else
                circle.ChangeValueTo(Utils.Randomizer.GetRandom(Context.LevelDef.FromNum, Context.LevelDef.ToNum));
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