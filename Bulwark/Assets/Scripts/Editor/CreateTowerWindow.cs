using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateTowerWindow : EditorWindow {
    new string name = "New Tower";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    Mesh towerBase;
    Mesh towerGun;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Bulwark/Create Tower")]
    static void Init () {
        // Get existing open window or if none, make a new one:
        CreateTowerWindow window = (CreateTowerWindow)GetWindow(typeof(CreateTowerWindow));
        window.Show();
    }

    void OnGUI () {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        name = EditorGUILayout.TextField("Name", name);
        towerBase = (Mesh)EditorGUILayout.ObjectField("Tower Base Model", towerBase, typeof(Mesh));
        towerGun = (Mesh)EditorGUILayout.ObjectField("Tower Gun Model", towerGun, typeof(Mesh));
        //EditorGUILayout.b
    }
}