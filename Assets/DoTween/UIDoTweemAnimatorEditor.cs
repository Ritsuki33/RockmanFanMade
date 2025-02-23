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


[CustomEditor(typeof(UIDoTweenSelecterAnimator))]
public class UIDoTweenSelectsAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIDoTweenSelecterAnimator animator = (UIDoTweenSelecterAnimator)target;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Auto Vertical Rayout"))
        {
            animator.AutoRayout(false);
        }

        if (GUILayout.Button("Auto Horizon Rayout"))
        {
            animator.AutoRayout(true);
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play Open (runtime only) ▶▶"))
        {
            animator.PlayOpen(() => Debug.Log("完了"));
        }


        if (GUILayout.Button("Play Close (runtime only) ◀◀"))
        {
            animator.PlayClose();
        }

        EditorGUILayout.EndHorizontal();
    }
}
