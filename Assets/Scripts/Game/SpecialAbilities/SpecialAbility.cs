using System;
using UnityEngine;

 public abstract class SpecialAbility
 {
     public Action<SpecialAbility> AbilityFinished;
     public Action<SpecialAbility> AbilityStarted;

     protected GameContext Context { get; private set; }

     protected LevelDb.LevelDef.SpecialAbility Def { get; private set; }
     protected SpecialAbilityVisual Visual { get; set; }

     public SpecialAbility(LevelDb.LevelDef.SpecialAbility def, SpecialAbilityVisual visual)
     {
         Def = def;
         Visual = visual;
     }

     // Start ability on context
     public bool Start(GameContext context) 
     {
         Context = context;
     

         if (AbilityStarted != null)
             AbilityStarted(this);

         return OnApply();
     }

     public void Finish()
     {
         OnFinished();

         if (AbilityFinished != null)
             AbilityFinished(this);
     }


     public abstract bool Update();

     protected abstract bool OnApply();
     protected abstract void OnFinished();
 }