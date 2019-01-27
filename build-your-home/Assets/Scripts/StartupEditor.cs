using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Startup))]
public class StartupEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("Delete Save")) {
            DataManager.DeleteSave();
        }
    }
}
