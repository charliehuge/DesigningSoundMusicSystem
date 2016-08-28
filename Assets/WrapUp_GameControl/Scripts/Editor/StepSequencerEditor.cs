using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StepSequencer))]
public class StepSequencerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StepSequencer sequencer = (StepSequencer) target;

        EditorGUI.BeginChangeCheck();

        DrawDefaultInspector();

        List<StepSequencer.Step> steps = sequencer.GetSteps();

        int numSteps = EditorGUILayout.IntSlider("# steps", steps.Count, 1, 32);

        while (numSteps > steps.Count)
        {
            steps.Add(new StepSequencer.Step());
        }
        while (numSteps < steps.Count)
        {
            steps.RemoveAt(steps.Count - 1);
        }

        for (int i = 0; i < steps.Count; ++i)
        {
            StepSequencer.Step step = steps[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 60;
            EditorGUILayout.LabelField("Step " + (i + 1), GUILayout.Width(60));
            step.Active = EditorGUILayout.Toggle("Active", step.Active, GUILayout.Width(80));
            step.MidiNoteNumber = EditorGUILayout.IntField("Note", step.MidiNoteNumber);
            step.Duration = EditorGUILayout.FloatField("Duration", (float)step.Duration);
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.EndHorizontal();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }
}