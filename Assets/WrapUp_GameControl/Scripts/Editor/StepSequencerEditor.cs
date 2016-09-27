using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom inspector for the StepSequencer.
/// Makes sequence editing a little prettier/faster.
/// </summary>
[CustomEditor(typeof(StepSequencer))]
public class StepSequencerEditor : Editor
{
    /// <summary>
    /// This gets called whenever a GameObject with the StepSequencer script
    /// attached has its inspector refreshed. Draw the controls in here.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // get the StepSequencer instance
        StepSequencer sequencer = (StepSequencer) target;

        // start listening for changes
        EditorGUI.BeginChangeCheck();

        // Draw the controls we aren't hiding with [HideInInspector]
        DrawDefaultInspector();

        List<StepSequencer.Step> steps = sequencer.GetSteps();

        // Set the number of steps in the sequence
        int numSteps = EditorGUILayout.IntSlider("# steps", steps.Count, 1, 32);

        // Add or remove steps based on the above slider's value
        while (numSteps > steps.Count)
        {
            steps.Add(new StepSequencer.Step());
        }
        while (numSteps < steps.Count)
        {
            steps.RemoveAt(steps.Count - 1);
        }

        // Draw the steps
        for (int i = 0; i < steps.Count; ++i)
        {
            StepSequencer.Step step = steps[i];

            // Draw all the step fields on one line
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 60;
            EditorGUILayout.LabelField("Step " + (i + 1), GUILayout.Width(60));
            step.Active = EditorGUILayout.Toggle("Active", step.Active, GUILayout.Width(80));
            step.MidiNoteNumber = EditorGUILayout.IntField("Note", step.MidiNoteNumber);
            step.Duration = EditorGUILayout.FloatField("Duration", (float)step.Duration);
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.EndHorizontal();
        }

        // if there were changes, mark the StepSequencer dirty,
        // that is, let Unity know it should be re-saved when saving the scene/project.
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }
}