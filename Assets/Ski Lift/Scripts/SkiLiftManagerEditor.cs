using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(LiftManager))]
[CanEditMultipleObjects]
public class SkiLiftManagerEditor : Editor
{
    SerializedProperty poles;
    SerializedProperty basePole;
    SerializedProperty topPole;
    SerializedProperty polePrefab;
    SerializedProperty rope;

    LiftManager self;

    private void OnEnable()
    {
        poles = serializedObject.FindProperty("poles");
        basePole = serializedObject.FindProperty("basePole");
        topPole = serializedObject.FindProperty("topPole");
        polePrefab = serializedObject.FindProperty("polePrefab");
        rope = serializedObject.FindProperty("rope");
        self = GameObject.Find(serializedObject.targetObject.name).GetComponent<LiftManager>();


    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(poles);
        EditorGUILayout.PropertyField(basePole);
        EditorGUILayout.PropertyField(topPole);
        EditorGUILayout.PropertyField(polePrefab);
        EditorGUILayout.PropertyField(rope);
        if(basePole == null || topPole == null)
        {
            Debug.LogError("Lift is missing anchor Pole");
        }

        if(GUILayout.Button("Add Pole"))
        {
            self.instantiateLiftPole();
        }


        serializedObject.ApplyModifiedProperties();
        //liftManager.Update();
        //base.OnInspectorGUI();
        //EditorGUILayout.PropertyField(liftManager);
    }


}
