//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(VelocityControll))]
//public class VelocityControllEditor : Editor {

//    public override void OnInspectorGUI()
//    {
//        VelocityControll velocityControll = (VelocityControll)target;

//        DrawDefaultInspector();

//        GUI.enabled = false;
//        EditorGUILayout.Vector2Field("Velocity", velocityControll.Velocity);
//        EditorGUILayout.TextField("Physical LayerMask", LayerMaskToString(velocityControll.PhysicalLayer));
//        GUI.enabled = true;   // ここでGUIを再び有効化する
//    }


//    private string LayerMaskToString(LayerMask layerMask)
//    {
//        int mask = layerMask.value;
//        string layerNames = "";
//        for (int i = 0; i < 32; i++)
//        {
//            if ((mask & (1 << i)) != 0)
//            {
//                if (layerNames.Length > 0)
//                {
//                    layerNames += ", ";
//                }
//                layerNames += LayerMask.LayerToName(i);
//            }
//        }
//        return layerNames;
//    }
//}

   
