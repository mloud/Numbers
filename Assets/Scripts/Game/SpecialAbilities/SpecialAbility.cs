using System;
using UnityEngine;

 public abstract class SpecialAbility
 {
     protected GameContext Context { get; private set; }

     public bool Apply(GameContext context) 
     {
         Context = context;

         return OnApply();
     }

     public abstract bool Update();

     protected abstract bool OnApply();
 }