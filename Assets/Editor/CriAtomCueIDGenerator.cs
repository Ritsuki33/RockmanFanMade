# if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using CriWare;


public class CriAtomCueIDGenerator :EditorWindow
{
    private string acbFileName = "sampleAcb";
    private string awbFileName = "SampleAwb";
    private string className = "CriAtomCueIDs";
    private string outputPath = "Assets/MyGame/Scripts";

    private const string PrefKeyAcb = "CriAtomCueID_AcbFileName";
    private const string PrefKeyAwb = "CriAtomCueID_AwbFileName";
    private const string PrefKeyClass = "CriAtomCueID_ClassName";
    private const string PrefKeyPath = "CriAtomCueID_OutputPath";

    private void OnEnable()
    {
        // エディタを開いたときにキャッシュを読み込む
        acbFileName = EditorPrefs.GetString(PrefKeyAcb, "sampleAcb");
        awbFileName = EditorPrefs.GetString(PrefKeyAwb, "SampleAwb");
        className = EditorPrefs.GetString(PrefKeyClass, "CriAtomCueIDs");
        outputPath = EditorPrefs.GetString(PrefKeyPath, "Assets/MyGame/Scripts");
    }

    [MenuItem("Tools/Generate Cri Atop CueIDs (Runtime only)")]
    public static void ShowWindow()
    {
        GetWindow<CriAtomCueIDGenerator>("Gnerate Cri Atop CueIDs");
    }

    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            GUILayout.FlexibleSpace(); // 上スペースを作る

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace(); // 左側のスペース
            GUILayout.Label("Available only at runtime", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace(); // 右側のスペース
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace(); // 下スペース
            return;
        }

        GUILayout.Label("CRI Atom CueID Genarator", EditorStyles.boldLabel);

        acbFileName = EditorGUILayout.TextField("ACB File Name", acbFileName);
        awbFileName = EditorGUILayout.TextField("AWB File Name", awbFileName);
        className = EditorGUILayout.TextField("Class Name", className);
        outputPath = EditorGUILayout.TextField("Output Path", outputPath);

        // 入力内容をキャッシュ
        EditorPrefs.SetString(PrefKeyAcb, acbFileName);
        EditorPrefs.SetString(PrefKeyAwb, awbFileName);
        EditorPrefs.SetString(PrefKeyClass, className);
        EditorPrefs.SetString(PrefKeyPath, outputPath);

        GUILayout.Space(50);

        // ボタンを中央揃え
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // 左側のスペース
        if (GUILayout.Button("Generate C# File", GUILayout.Width(200), GUILayout.Height(40)))
        {
            GenerateCueIDScript();
        }
        GUILayout.FlexibleSpace(); // 右側のスペース
        GUILayout.EndHorizontal();
    }

    public void GenerateCueIDScript()
    {
        string acbPath = Path.Combine(Application.streamingAssetsPath, acbFileName);
        string awbPath = (string.IsNullOrEmpty(awbFileName)) ? null : Path.Combine(Application.streamingAssetsPath, awbFileName);
        CriAtomExAcb acb = CriAtomExAcb.LoadAcbFile(null, acbPath, awbPath);

        if (acb == null)
        {
            Debug.LogError("");
        }
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"public static class {className}");
        sb.AppendLine("{");

        CriAtomEx.CueInfo[] cueInfos = acb.GetCueInfoList();
        foreach (var cue in cueInfos)
        {
            sb.AppendLine($"    public static readonly int {cue.name} = {cue.id};");
        }

        sb.AppendLine("}");

        string path = $"{outputPath}/{className}.cs";
        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);

        Debug.Log($"{className}.csを作成しました: {outputPath}");
        AssetDatabase.Refresh();
    }
}
#endif