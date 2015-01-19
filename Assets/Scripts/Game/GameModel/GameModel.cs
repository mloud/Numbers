using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameModel
{
    // Current number queue
    public List<int> Numbers { get; set; }

    // List of numbers on playground 
    public List<Circle> Circles { get; set; }


    private Dictionary<Circle.TapBehaviour, TapHandler> TapHandlers { get; set; }
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
  
        TapHandlers = new Dictionary<Circle.TapBehaviour, TapHandler> 
        { 
            {Circle.TapBehaviour.Minus, new MinusOneTapHandler()}
        };

        // Register Pattern recognizers
        NumberPatterns = new List<NumberPattern> () 
        {   new EqualNumberPattern(), 
            new PlusOneNumberPattern() 
        };

        Circles = new List<Circle> ();
        Numbers = new List<int>();
    }

    public struct ClickResult
    {
        public bool FitSequence;
    }

    public ClickResult ProcessPatterns(Circle circle)
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
    public TapResult ProcessTapHandlers(Circle circle)
    {
        var result = new TapResult();
        result.Remove = true;

        TapHandler tapHandler = null;
        TapHandlers.TryGetValue(circle.TapBehav, out tapHandler); 
        if (tapHandler != null)
        {
            result.Remove = tapHandler.Handle(circle, Context);
        }
        
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
