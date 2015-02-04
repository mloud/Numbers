using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;


public class LevelEditorView : EditorWindow
{

    public LevelEditorController Controller
    { 
        get
        {
            if (controller == null)
            {
                controller = new LevelEditorController();
                controller.OnNumLimitsChange += this.OnNumLimitsChanged;
                controller.OnProbabilityChange += this.OnProbabilityChanged;
            }
            return controller;
        }
    }

    private LevelEditorController controller; 
    private Vector2 scrollPos;
   


    [MenuItem ("Window/LevelEditor")]
    static void Init () {
      
        LevelEditorView window = (LevelEditorView)EditorWindow.GetWindow (typeof (LevelEditorView));
        window.Setup();
    }



    private List<bool> FoldOut { get; set; }
    private Dictionary<LevelDb.LevelDef, AnimationCurve> ProbabilityCurves { get; set; }

    private static class Config
    {
        public static int ButtonWidth = 200;
    }

    public void Setup()
    {
        this.title = "LevelEditor";

        FoldOut = new List<bool>(Controller.Levels.Count);
        Controller.Levels.ForEach(x => FoldOut.Add(false));
        ProbabilityCurves = new Dictionary<LevelDb.LevelDef, AnimationCurve>();
    }

    private void OnGUI ()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        GUILayout.BeginVertical();

        
        if (GUILayout.Button("Save", GUILayout.Width(Config.ButtonWidth)))
        {
            Controller.Save();
        }

        if (GUILayout.Button("Add new", GUILayout.Width(Config.ButtonWidth)))
        {
            Controller.AddNewLevel();
            Setup();
        }

        GUILayout.Space(30);

        for (int i = 0; i < Controller.Levels.Count; ++i)
        {
            var level = Controller.Levels[i];

            FoldOut[i] = EditorGUILayout.Foldout(FoldOut[i], new GUIContent(level.Name));


            if (FoldOut[i])
            {
                if (GUILayout.Button("Play", GUILayout.Width(Config.ButtonWidth)))
                {
                    Controller.PlayLevel(level);
                }

                if (GUILayout.Button("Delete", GUILayout.Width(Config.ButtonWidth)))
                {
                    Controller.DeleteLevel(level);
                }

                //Name
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Name");
                level.Name = EditorGUILayout.TextField(level.Name);
                GUILayout.EndHorizontal();

               
                //Order
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Order");
                level.Order = EditorGUILayout.IntField(level.Order);
                GUILayout.EndHorizontal();
            
                //From number
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("From number");

                int newFromNum = EditorGUILayout.IntField(level.FromNum);
                Controller.SetNumLimits(newFromNum, level.ToNum, level);

                GUILayout.EndHorizontal();

                //To number
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("To number");
                int newToNum = EditorGUILayout.IntField(level.ToNum);
                Controller.SetNumLimits(level.FromNum, newToNum, level);
                GUILayout.EndHorizontal();

                //Dimension
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("X-Y");
                level.Cols = EditorGUILayout.IntField(level.Cols);
                level.Rows = EditorGUILayout.IntField(level.Rows);
                GUILayout.EndHorizontal();

                //Score
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Score");
                level.Score = EditorGUILayout.IntField(level.Score);
                GUILayout.EndHorizontal();


				// LeaderboardId
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("LeaderboardId");
				level.LeaderboardId = EditorGUILayout.TextField(level.LeaderboardId);
				GUILayout.EndHorizontal();

                //Level duration
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Level duration (sec)");
                level.TotalTime = EditorGUILayout.FloatField(level.TotalTime);
                GUILayout.EndHorizontal();

                // MicroTime
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Micro duration (sec)");
                level.MicroTime = EditorGUILayout.FloatField(level.MicroTime);
                GUILayout.EndHorizontal();

                // FlipTime
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Flip time (sec)");
                level.FlipTime = EditorGUILayout.FloatField(level.FlipTime);
                GUILayout.EndHorizontal();

                // RefillTime
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Refill time (sec)");
                level.RefillTime = EditorGUILayout.FloatField(level.RefillTime);
                GUILayout.EndHorizontal();

                // Colors
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Colors");
                level.Colors = EditorGUILayout.IntField(level.Colors);
                GUILayout.EndHorizontal();


                GUILayout.Space(30);

                // Initial numbers
                DrawInitialNumbers(level);

                GUILayout.Space(30);

                // Flip numbers
                DrawFlipNumbers(level);

                GUILayout.Space(30);

                // Probability
                DrawProbabilityCurve(level);

                // Patterns
                DrawPatternsUsed(level);

                // Special Abilities
                DrawSpecialAbilities(level);

                EditorGUILayout.Space();
            }
        }

        GUILayout.EndVertical();

        GUILayout.EndScrollView();
    }

    private int SpecialAbilityIndex = 0;
    private void DrawSpecialAbilities(LevelDb.LevelDef level)
    {
        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("SpecialAbilities");

        
        if (level.SpecialAbilities != null)
        {
            var fields = typeof(SpecialAbilityDef).GetFields();
            List<string> allAbilities = new List<string>();
            Array.ForEach(fields, x => allAbilities.Add(x.GetValue(null).ToString()));

            allAbilities.RemoveAll(x=>level.SpecialAbilities.Find(y => y.Name == x) != null);

            SpecialAbilityIndex = EditorGUILayout.Popup(PatternIndex, allAbilities.ToArray());
	        if (GUILayout.Button("Add", GUILayout.Width(Config.ButtonWidth)))
	        {
                Controller.AddSpecialAbility(level, allAbilities[SpecialAbilityIndex], 0);
	        }

    
            for (int i = 0; i < level.SpecialAbilities.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(level.SpecialAbilities[i].Name, GUILayout.Width(35));
                level.SpecialAbilities[i].RechargeTime = EditorGUILayout.FloatField(level.SpecialAbilities[i].RechargeTime, GUILayout.Width(35));
                level.SpecialAbilities[i].Duration = EditorGUILayout.FloatField(level.SpecialAbilities[i].Duration, GUILayout.Width(35));

                if (GUILayout.Button("Remove", GUILayout.Width(Config.ButtonWidth)))
                {
                    Controller.RemoveSpecialAbility(level, level.SpecialAbilities[i].Name);
                }
        
                GUILayout.EndHorizontal();
            }
        }


        if (GUILayout.Button("Create SpecialAbilities", GUILayout.Width(Config.ButtonWidth)))
        {
            Controller.CreateSpecialAbilities(level);
        }
        

        GUILayout.EndVertical();
    }
    

    private void DrawFlipNumbers(LevelDb.LevelDef level)
    {
        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("Flip numbers");

        // Flip numbers count
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Count");
        level.FlipNumbersCount = EditorGUILayout.IntField(level.FlipNumbersCount);
        GUILayout.EndHorizontal();

        if (level.FlipNumbers != null)
        {
            GUILayout.BeginHorizontal();

            for (int i = 0; i < level.FlipNumbers.Count; ++i)
            {
                level.FlipNumbers [i] = EditorGUILayout.IntField(level.FlipNumbers [i], GUILayout.Width(35));
            }

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Create Numbers", GUILayout.Width(Config.ButtonWidth)))
        {
            Controller.CreateFlipNumbers(level);
        }

        if (GUILayout.Button("Fill Random", GUILayout.Width(Config.ButtonWidth)))
        {
            Controller.RandomizeFlipNumbers(level);
        }

        GUILayout.EndVertical();
    }

    private void DrawInitialNumbers(LevelDb.LevelDef level)
    {
        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("InitialNumbers");

        if (level.Numbers != null && level.Numbers.Count == level.Rows * level.Cols)
        {
            for (int y = 0; y < level.Rows; ++y)
            {
                GUILayout.BeginHorizontal();

                for (int x = 0; x < level.Cols; ++x)
                {
                    int index = y * level.Cols + x;
                    level.Numbers [index] = EditorGUILayout.IntField(level.Numbers [index], GUILayout.Width(30));

                    // spec string
                    if (level.SpecialitiesForNumbers != null && index < level.SpecialitiesForNumbers.Count)
                        level.SpecialitiesForNumbers [index] = EditorGUILayout.TextField(level.SpecialitiesForNumbers [index], GUILayout.Width(30));
                }

                GUILayout.EndHorizontal();
            }
        }

        if (GUILayout.Button("Create Numbers", GUILayout.Width(Config.ButtonWidth)))
        {
            Controller.CreateNumbers(level);
        }

        if (GUILayout.Button("Fill Random", GUILayout.Width(Config.ButtonWidth)))
        {
            Controller.RandomizeInitialLevelNumbers(level);
        }


        GUILayout.EndVertical();
    }

    private bool SyncProbCurve = false;
    private void DrawProbabilityCurve(LevelDb.LevelDef level)
    {
        if (ProbabilityCurves == null)
            return;//todo

        EditorGUILayout.LabelField("Probablity");

      
        if (level.Probabilities != null)
        {
            AnimationCurve curve = null;
            bool exist = ProbabilityCurves.TryGetValue(level, out curve);
            SyncProbabilityCurve(level, ref curve, true && !SyncProbCurve);
            SyncProbCurve = false;
            if (!exist)
                ProbabilityCurves.Add(level, curve);

            ProbabilityCurves[level] = EditorGUILayout.CurveField("Number probability", curve);

            if (GUILayout.Button("Update Probability", GUILayout.Width(Config.ButtonWidth)))
            {
                UpdateProbabilityFromCurve( ProbabilityCurves[level], level);
                SyncProbCurve = true;
            }
        }

        if (GUILayout.Button("Create Probability", GUILayout.Width(Config.ButtonWidth)))
        {
            Controller.CreateProbability(level);
        }


    }


    private int PatternIndex = 0;
    public void DrawPatternsUsed(LevelDb.LevelDef level)
    {
        EditorGUILayout.LabelField("Patterns");

        var fields = typeof(PatternDef).GetFields();
        List<string> allPatterns = new List<string>();
        Array.ForEach(fields, x => allPatterns.Add(x.GetValue(null).ToString()));

        if (level.Patterns != null)
            allPatterns.RemoveAll(x=>level.Patterns.Find(y=>y ==x) != null);

        if (GUILayout.Button("Create Patterns", GUILayout.Width(Config.ButtonWidth)))
		{
			Controller.CreatePatterns(level);
		}

		if (level.Patterns != null)
		{

	        PatternIndex = EditorGUILayout.Popup(PatternIndex, allPatterns.ToArray());
            if (GUILayout.Button("Add", GUILayout.Width(Config.ButtonWidth)))
	        {
	            Controller.AddPattern(level, allPatterns[PatternIndex]);
	        }


	        for (int i = 0; i < level.Patterns.Count; ++i)
	        {
	            EditorGUILayout.BeginHorizontal();
	            EditorGUILayout.LabelField(level.Patterns[i]);

                if (GUILayout.Button("Remove", GUILayout.Width(Config.ButtonWidth)))
	            {
	                Controller.RemovePattern(level, level.Patterns[i]);
	            }

	            EditorGUILayout.EndHorizontal();
	        }
		}

    }



    private void SyncProbabilityCurve(LevelDb.LevelDef level, ref AnimationCurve curve, bool onlyIfNull)
    {
        bool isNull = false;
        if (curve == null || (curve.keys.Length != level.Probabilities.Count))
        {
            curve = new AnimationCurve();
            isNull = true;
        }

        var keys = curve.keys;

        if (keys.Length != level.Probabilities.Count)
        {
            keys = new Keyframe[level.Probabilities.Count];
            isNull = true;
        }

        if (!onlyIfNull || isNull)
        {
		
            Core.Dbg.Log("Syncing curve from LevelDef.Probability");
            for (int i = 0; i < level.Probabilities.Count; ++i)
            {
                var keyFrame = keys [i];
                keyFrame.time = level.FromNum + i;
                keyFrame.value = level.Probabilities [i];
                keys [i] = keyFrame;
            }
        
            curve.keys = keys;
        }
    }

    private void UpdateProbabilityFromCurve(AnimationCurve curve, LevelDb.LevelDef level)
    {
        float sum = 0;
        Array.ForEach<Keyframe>(curve.keys, x => sum += x.value);

        if (sum > 0)
        {
            float coef = 1.0f / sum;

            var keys = curve.keys;

            for (int i = 0; i < keys.Length; ++i)
            {
                var keyframe = keys[i];
                keyframe.value *= coef;
                keys[i] = keyframe;
                level.Probabilities[i] = keys[i].value;
            }
        }

        String str = "Updating LevelDef.Probability -> ";
        level.Probabilities.ForEach(x => str += x.ToString() + " " );
		Core.Dbg.Log(str);

    }

    private void OnNumLimitsChanged(LevelDb.LevelDef level)
    {
    
    }

    private void OnProbabilityChanged(LevelDb.LevelDef level)
    {
        AnimationCurve curve = null;
        bool exist = ProbabilityCurves.TryGetValue(level, out curve);
        SyncProbabilityCurve(level, ref curve, false);
        if (!exist)
            ProbabilityCurves.Add(level, curve);
    }

}