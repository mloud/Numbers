using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;


public class SpecialAbilityEditorView : EditorWindow
{

    public SpecialAbilityEditorController Controller
    { 
        get
        {
            if (controller == null)
            {
                controller = new SpecialAbilityEditorController();
            }
            return controller;
        }
    }

    private SpecialAbilityEditorController controller; 
    private Vector2 scrollPos;
   


    [MenuItem ("Window/SpecialAbilityEditor")]
    static void Init () 
    {

        SpecialAbilityEditorView window = (SpecialAbilityEditorView)EditorWindow.GetWindow(typeof(SpecialAbilityEditorView));
        window.Setup();
    }


    private List<bool> FoldOut { get; set; }
  
    private static class Config
    {
        public static int ButtonWidth = 200;
    }

    public void Setup()
    {
        this.title = "SpecialAbilityEditor";

        FoldOut = new List<bool>(Controller.SpecialAbilityDb.SpecialAbilities.Count);
        Controller.SpecialAbilityDb.SpecialAbilities.ForEach(x => FoldOut.Add(false));
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
            Controller.AddNewAbility();
            Setup();
        }

        GUILayout.Space(30);

        for (int i = 0; i < Controller.SpecialAbilityDb.SpecialAbilities.Count; ++i)
        {
            var ability = Controller.SpecialAbilityDb.SpecialAbilities[i];

            FoldOut[i] = EditorGUILayout.Foldout(FoldOut[i], new GUIContent(ability.Name));


            if (FoldOut[i])
            {
              
                if (GUILayout.Button("Delete", GUILayout.Width(Config.ButtonWidth)))
                {
                    Controller.RemoveSpecialAbility(ability);
                }

                //Name
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Name");
                ability.Name = EditorGUILayout.TextField(ability.Name);
                GUILayout.EndHorizontal();

               
                //Unlock on level
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Level");
                ability.AvailableForLevel = EditorGUILayout.IntField(ability.AvailableForLevel);
                GUILayout.EndHorizontal();

                //Unlock on level
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Initial count");
                ability.InitialCount = EditorGUILayout.IntField(ability.InitialCount);
                GUILayout.EndHorizontal();

                //Recharge time
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Recharge time");
                ability.RechargeTime = EditorGUILayout.FloatField(ability.RechargeTime);
                GUILayout.EndHorizontal();

                //Duration time
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Duartion time");
                ability.Duration = EditorGUILayout.FloatField(ability.Duration);
                GUILayout.EndHorizontal();

                // Description
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Description");
                ability.Description = EditorGUILayout.TextField(ability.Description);
                GUILayout.EndHorizontal();



        
            }
        }

        GUILayout.EndVertical();

        GUILayout.EndScrollView();
    }
}