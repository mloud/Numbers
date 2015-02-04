using System;
using UnityEngine;

 public class AnyNumberSpecialAbility : SpecialAbility
 {

     public AnyNumberSpecialAbility(LevelDb.LevelDef.SpecialAbility def, SpecialAbilityVisual visual)
         : base(def, visual)
     { }

     protected override bool OnApply()
     {
         GameObject circlePrefab = Resources.Load<GameObject>("Prefabs/Circle");
         var circleController = (GameObject.Instantiate(circlePrefab) as GameObject).GetComponent<CircleController>();

         circleController.SetPosition(Visual.transform.position);
         circleController.SetValue(0);
         Context.Controller.OnCircleClick(circleController);

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