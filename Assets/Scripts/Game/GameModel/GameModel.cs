using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameModel
{
    // Current number queue
    public List<int> Numbers { get; set; }

    // List of numbers on playground 
    public List<CircleVisual> Circles { get; set; }

    private List<string> Specialities { get; set; }
   
    private List<NumberPattern> NumberPatterns { get; set; }

    private GameContext Context { get; set; }

    public GameModel(GameContext context)
    {
        Init(context);
    }

    public void Init(GameContext context)
    {
        Context = context;
        Context.Model = this;

        NumberPatterns = new List<NumberPattern>();

        context.LevelDef.Patterns.ForEach(x => NumberPatterns.Add(PatternFactory.Create(x)));
       

        Circles = new List<CircleVisual> ();
        Numbers = new List<int>();
    }

    public struct ClickResult
    {
        public bool FitSequence;
    }

    public ClickResult ProcessPatterns(CircleVisual circle)
    {
        var result = new ClickResult();
       
        foreach (var pattern in NumberPatterns)
        {
            if (pattern.CanAdd(circle, Context))
            {
                result.FitSequence = true;
                break;
            }
        }

        return result;
    }

    public struct TapResult
    {
        public bool Remove;
    }
    public TapResult ProcessTapHandlers(CircleVisual circle)
    {
        var result = new TapResult();
        result.Remove = true;

        circle.Specialities.ForEach(x=>result.Remove = x.Handle(circle, Context));

        return result;
    }


    public int ComputeScore(List<int> nums)
    {
        var numPattern = NumberPatterns.Find(x=>x.IsPattern(nums));
        
        if (numPattern != null)
        {
            return numPattern.ComputeScore(nums);
        }
        
        return 0;
    }


}
