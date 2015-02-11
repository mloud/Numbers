using System;
using UnityEngine;

 public class RefillSpecialAbility : SpecialAbility
 {

     public RefillSpecialAbility(SpecialAbilityDb.SpecialAbility def, SpecialAbilityVisual visual)
         : base(def, visual)
     { }

     protected override bool OnApply()
     {
      
         var freeSlots = Context.Model.Slots.FindAll(x => x.Circle == null);

         freeSlots.ForEach( (Slot slot) => 
         {
              Context.Controller.CreateCircleOnSlot(slot);
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