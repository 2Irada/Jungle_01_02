using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraController _cameraController = (CameraController)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Auto Set Offset"))
        {
            _cameraController.ApplyCurrentOffset();
        }
        if (GUILayout.Button("Move Cam"))
        {
            _cameraController.MoveCameraToSpot();
        }
    }
}
#endif
