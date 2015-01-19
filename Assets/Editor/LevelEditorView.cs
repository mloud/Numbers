using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class LevelEditorView : EditorWindow
{

    private LevelEditorController Controller
    { 
        get
        {
            if (controller == null)
                controller = new LevelEditorController();
            return controller;
        }
    }

    private LevelEditorController controller; 


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
        GUILayout.BeginVertical();


        for (int i = 0; i < Controller.Levels.Count; ++i)
        {
            var level = Controller.Levels[i];

            FoldOut[i] = EditorGUILayout.Foldout(FoldOut[i], new GUIContent(level.Name));

            if (FoldOut[i])
            {   //Name
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Name");
                EditorGUILayout.TextField(level.Name);
                GUILayout.EndHorizontal();

                //Order
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Order");
                EditorGUILayout.IntField(level.Order);
                GUILayout.EndHorizontal();
            
                //From number
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("From number");
                EditorGUILayout.IntField(level.FromNum);
                GUILayout.EndHorizontal();

                //To number
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("To number");
                EditorGUILayout.IntField(level.FromNum);
                GUILayout.EndHorizontal();

                //Dimension
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("X-Y");
                EditorGUILayout.IntField(level.Cols);
                EditorGUILayout.IntField(level.Rows);
                GUILayout.EndHorizontal();

                //Score
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Score");
                EditorGUILayout.IntField(level.Score);
                GUILayout.EndHorizontal();

                //Level duration
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Level duration (sec)");
                EditorGUILayout.FloatField(level.TotalTime);
                GUILayout.EndHorizontal();

                // MicroTime
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Micro duration (sec)");
                EditorGUILayout.FloatField(level.MicroTime);
                GUILayout.EndHorizontal();

                // FlipTime
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Flip time (sec)");
                EditorGUILayout.FloatField(level.FlipTime);
                GUILayout.EndHorizontal();


                
                
            }
        }


        GUILayout.EndVertical();


        //GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
        //myString = EditorGUILayout.TextField ("Text Field", myString);
        
        //groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
        //myBool = EditorGUILayout.Toggle ("Toggle", myBool);
        //myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
        //EditorGUILayout.EndToggleGroup ();
    }



}