using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/* TILE SPEED INCREMENTATION EDITOR SCRIPT
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */
/// <summary>
/// Ensures that only fields relevant to the currently selected speed incrementation mode 
/// are shown in the Unity inspector GUI so that the designers are not confused by the unused fields for that mode
/// </summary>
[CustomEditor(typeof(TileSpeedIncrementation)), CanEditMultipleObjects]
public class TileSpeedIncrementationEditor : Editor
{
    /* RESEARCH SOURCE REFERENCE
     * Title: Customizing inspector by Enum type likes
     * Author: Various
     * Date Published: 2017 Onwards
     * Date Accessed: 16/08/2022
     * URL: https://stackoverflow.com/questions/44557742/customizing-inspector-by-enum-type-likes
     * Usage: Learned how to implement a basic editor script 
     * that uses conditionals to vary the fields visible in the editor.
     */


    // Create property variables
    public SerializedProperty
    incrementMode_prop,
    calculatedTargetTileSpeed_prop,
    useSpeedLimit_prop,
    speedLimit_prop,
    startingTileSpeed_prop,
    linearIncrementFactor_prop,
    intervalTime_prop,
    intervalIncreaseFactor_prop;

    // Start is called before the first frame update
    void OnEnable()
    {
        // Enable all the properties as serializedObjects and find the related field in the TileSpeedIncrementation class
        this.incrementMode_prop = this.serializedObject.FindProperty("incrementMode");
        this.calculatedTargetTileSpeed_prop = this.serializedObject.FindProperty("_calculatedTargetTileSpeed");
        this.useSpeedLimit_prop = this.serializedObject.FindProperty("useSpeedLimit");
        this.speedLimit_prop = this.serializedObject.FindProperty("speedLimit");
        this.startingTileSpeed_prop = this.serializedObject.FindProperty("startingTileSpeed");
        this.linearIncrementFactor_prop = this.serializedObject.FindProperty("linearIncrementFactor");
        this.intervalTime_prop = this.serializedObject.FindProperty("intervalTime");
        this.intervalIncreaseFactor_prop = this.serializedObject.FindProperty("intervalIncreaseFactor");
    }

    // Arranges the GUI depending on certain conditions
    // so that only relevant fields are shown in the editor to be edited
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        // These properties are always visible regardless of the chosen speed incrementation setup
        EditorGUILayout.PropertyField(this.calculatedTargetTileSpeed_prop);
        EditorGUILayout.PropertyField(this.incrementMode_prop);
        EditorGUILayout.PropertyField(this.useSpeedLimit_prop);
        EditorGUILayout.PropertyField(this.startingTileSpeed_prop);

        // Only show the speed limit property if the useSpeedLimit checkbox has been ticked
        if (this.useSpeedLimit_prop.boolValue == true)
        {
            EditorGUILayout.PropertyField(this.speedLimit_prop);
        }

        // Use a switch statement with the selected mode
        // to display the required fields for that speed increment mode
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
