using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

[CustomEditor(typeof(UIDoTweemAnimator))]
public class UIDoTweemAnimatorEditor : Editor
{
    SerializedProperty openTweensProperty;
    SerializedProperty isReverseProperty;
    SerializedProperty closeTweensProperty;

    void OnEnable()
    {
        openTweensProperty = serializedObject.FindProperty("openTweens");
        isReverseProperty = serializedObject.FindProperty("isReverse");
        closeTweensProperty = serializedObject.FindProperty("closeTweens");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(openTweensProperty);
        EditorGUILayout.PropertyField(isReverseProperty);

        // isReverseがtrueの場合、closeTweensを無効化して表示
        EditorGUI.BeginDisabledGroup(isReverseProperty.boolValue);
        EditorGUILayout.PropertyField(closeTweensProperty);
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();

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
    SerializedProperty openTweensProperty;
    SerializedProperty isReverseProperty;
    SerializedProperty closeTweensProperty;

    void OnEnable()
    {
        openTweensProperty = serializedObject.FindProperty("m_openSeq");
        isReverseProperty = serializedObject.FindProperty("isReverse");
        closeTweensProperty = serializedObject.FindProperty("m_closeSeq");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(openTweensProperty);
        EditorGUILayout.PropertyField(isReverseProperty);

        // isReverseがtrueの場合、closeTweensを無効化して表示
        EditorGUI.BeginDisabledGroup(isReverseProperty.boolValue);
        EditorGUILayout.PropertyField(closeTweensProperty);
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();

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
public class UIDoTweenSelecterAnimatorEditor : Editor
{
    SerializedProperty listProperty;
    SerializedProperty spaceProperty;
    SerializedProperty offsetStepProperty;
    SerializedProperty delayProperty;

    SerializedProperty openTweensProperty;

    void OnEnable()
    {
        listProperty = serializedObject.FindProperty("list");
        spaceProperty = serializedObject.FindProperty("space");
        offsetStepProperty = serializedObject.FindProperty("offsetStep");
        delayProperty = serializedObject.FindProperty("delay");

        openTweensProperty = serializedObject.FindProperty("openTweens");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(listProperty);
        EditorGUILayout.PropertyField(spaceProperty);
        EditorGUILayout.PropertyField(offsetStepProperty);

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

        EditorGUILayout.Space(50);

        EditorGUILayout.PropertyField(delayProperty);


        EditorGUILayout.PropertyField(openTweensProperty);

        serializedObject.ApplyModifiedProperties();
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
