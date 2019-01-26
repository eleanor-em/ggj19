using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var mgr = (DataManager)target;
        if (GUILayout.Button("Save")) {
            mgr.Save();
        }
        if (GUILayout.Button("Load")) {
            mgr.Load();
        }
    }
}
