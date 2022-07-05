using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileSpeedIncrementation)), CanEditMultipleObjects]
public class SpeedIncrement : Editor
{

    public SerializedProperty
    incrementMode_prop,
    currentTileSpeed_prop,
    useSpeedLimit_prop,
    speedLimit_prop,
    startingTileSpeed_prop,
    linearIncrementFactor_prop,
    intervalTime_prop,
    intervalIncreaseFactor_prop;

    // Start is called before the first frame update
    void OnEnable()
    {
        this.incrementMode_prop = this.serializedObject.FindProperty("incrementMode");
        this.currentTileSpeed_prop = this.serializedObject.FindProperty("currentTileSpeed");
        this.useSpeedLimit_prop = this.serializedObject.FindProperty("useSpeedLimit");
        this.speedLimit_prop = this.serializedObject.FindProperty("speedLimit");
        this.startingTileSpeed_prop = this.serializedObject.FindProperty("startingTileSpeed");
        this.linearIncrementFactor_prop = this.serializedObject.FindProperty("linearIncrementFactor");
        this.intervalTime_prop = this.serializedObject.FindProperty("intervalTime");
        this.intervalIncreaseFactor_prop = this.serializedObject.FindProperty("intervalIncreaseFactor");
    }


    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        EditorGUILayout.PropertyField(this.currentTileSpeed_prop);
        EditorGUILayout.PropertyField(this.incrementMode_prop);
        EditorGUILayout.PropertyField(this.useSpeedLimit_prop);

        if (this.useSpeedLimit_prop.boolValue == true)
        {
            EditorGUILayout.PropertyField(this.speedLimit_prop);
        }
        EditorGUILayout.PropertyField(this.startingTileSpeed_prop);

        SpeedIncrementMode selectedMode = (SpeedIncrementMode)this.incrementMode_prop.enumValueIndex;

        switch (selectedMode)
        {
            case SpeedIncrementMode.linear:
                EditorGUILayout.PropertyField(this.linearIncrementFactor_prop);
                break;
            case SpeedIncrementMode.intervals:
                {
                    EditorGUILayout.PropertyField(this.intervalTime_prop);
                    EditorGUILayout.PropertyField(this.intervalIncreaseFactor_prop);
                    break;
                }
            case SpeedIncrementMode.linearMidInterval:
                {
                    EditorGUILayout.PropertyField(this.intervalTime_prop);
                    EditorGUILayout.PropertyField(this.intervalIncreaseFactor_prop);
                    EditorGUILayout.PropertyField(this.linearIncrementFactor_prop);
                }
                break;
            default:
                break;

        }

        this.serializedObject.ApplyModifiedProperties();
    }


}
