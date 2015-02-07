using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameModel
{
    
    public GameContext Context { get; set; }

    // Current number queue
    public List<Number> Numbers { get; set; }
      // List of numbers on playground 
    public List<CircleController> Circles { get; set; }

    private List<string> Specialities { get; set; }
   
    private List<NumberPattern> NumberPatterns { get; set; }

    public List<string> SpecialAbilities { get; set; }

    public List<Slot> Slots { get; set; }

    public GameModel(GameContext context)
    {
        Init(context);
    }

    public void Init(GameContext context)
    {
        Context = context;
        Context.Model = this;

        NumberPatterns = new List<NumberPattern>();
        SpecialAbilities = new List<string>();
        Slots = new List<Slot>();

        context.LevelDef.Patterns.ForEach(x => NumberPatterns.Add(PatternFactory.Create(x)));
       
      
        Circles = new List<CircleController>();
        Numbers = new List<Number>();
    }

    public struct ClickResult
    {
        public bool FitSequence;
        public NumberPattern Pattern;
    }

    public ClickResult ProcessPatterns(CircleController circle)
    {
        var result = new ClickResult();
       
        foreach (var pattern in NumberPatterns)
        {
            if (pattern.CanAdd(circle, Context))
            {
                result.FitSequence = true;
                result.Pattern = pattern;
                break;
            }
        }

        return result;
    }

    public struct TapResult
    {
        public bool Remove;
    }
    public TapResult ProcessTapHandlers(CircleController circle)
    {
        var result = new TapResult();
        result.Remove = true;

        circle.Model.Specialities.ForEach(x=>result.Remove = x.Handle());

        return result;
    }


    public NumberResult ComputeScore(List<Number> nums)
    {
        var numPattern = NumberPatterns.Find(x=>x.IsPattern(nums));
        
        if (numPattern != null)
        {
            return numPattern.ComputeScore(nums);
        }

        return new NumberResult();
    }


}
