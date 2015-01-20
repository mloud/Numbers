using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class LevelEditorView : EditorWindow
{

    public LevelEditorController Controller
    { 
        get
        {
            if (controller == null)
                controller = new LevelEditorController();
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

    public void Setup()
    {
        this.title = "LevelEditor";

        FoldOut = new List<bool>(Controller.Levels.Count);
        Controller.Levels.ForEach(x => FoldOut.Add(false));

    }

    private void OnGUI ()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        GUILayout.BeginVertical();

        
        if (GUILayout.Button("Save"))
        {
            Controller.Save();
        }

        if (GUILayout.Button("Add new"))
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
                if (GUILayout.Button("Play"))
                {
                    Controller.PlayLevel(level);
                }

                if (GUILayout.Button("Delete"))
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
                level.FromNum = EditorGUILayout.IntField(level.FromNum);
                GUILayout.EndHorizontal();

                //To number
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("To number");
                level.ToNum = EditorGUILayout.IntField(level.ToNum);
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


                // Initial numbers
                DrawInitialNumbers(level);

                // Flip numbers
                DrawFlipNumbers(level);

                EditorGUILayout.Space();
            }
        }

        GUILayout.EndVertical();

        GUILayout.EndScrollView();
    }

    private void DrawFlipNumbers(LevelDb.LevelDef level)
    {
        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("Flip numbers");

        // Flip numbers count
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Count)");
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

        if (GUILayout.Button("Create Numbers"))
        {
            Controller.CreateFlipNumbers(level);
        }
        
        if (GUILayout.Button("Fill Random"))
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
                    level.Numbers [index] = EditorGUILayout.IntField(level.Numbers [index], GUILayout.Width(35));
                }

                GUILayout.EndHorizontal();
            }
        }

        if (GUILayout.Button("Create Numbers"))
        {
            Controller.CreateNumbers(level);
        }

        if (GUILayout.Button("Fill Random"))
        {
            Controller.RandomizeInitialLevelNumbers(level);
        }


        GUILayout.EndVertical();

    }

}