using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraControllArea))]
public class CameraControllArea2Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var cameraControllArea = target as CameraControllArea;
        //ƒ{ƒ^ƒ“‚ð•\Ž¦
        if (GUILayout.Button("Create TransitCameraArea"))
        {
            cameraControllArea.SendMessage("AddTransitCameraArea", null, SendMessageOptions.DontRequireReceiver);
        }
    }

}
