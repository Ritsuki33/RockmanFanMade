using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

[CustomEditor(typeof(UIDoTweemAnimator))]
public class UIDoTweemAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIDoTweemAnimator animator = (UIDoTweemAnimator)target;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play Open (runtime only) ▶▶"))
        {
            animator.PlayOpen();
        }


        if (GUILayout.Button("Play Close (runtime only) ◀◀"))
        {
            animator.PlayClose();
        }

        EditorGUILayout.EndHorizontal();
    }
}


[CustomEditor(typeof(UIDoTweenGroupAnimator))]
public class UIDoTweenGroupAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIDoTweenGroupAnimator animator = (UIDoTweenGroupAnimator)target;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play Open (runtime only) ▶▶"))
        {
            animator.PlayOpen();
        }

        if (GUILayout.Button("Play Close (runtime only) ◀◀"))
        {
            animator.PlayClose();
        }

        EditorGUILayout.EndHorizontal();
    }
}