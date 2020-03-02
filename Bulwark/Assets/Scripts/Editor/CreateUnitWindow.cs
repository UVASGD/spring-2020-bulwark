using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TDCGG;

public class CreateUnitWindow : EditorWindow {
    new string name = "New Unit";
    Mesh unitMesh;
    UnitType type;
    float maxHealth = 100f;
    float moveSpeed = 1f;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Bulwark/Create Unit")]
    static void Init () {
        // Get existing open window or if none, make a new one:
        CreateUnitWindow window = (CreateUnitWindow)GetWindow(typeof(CreateUnitWindow));
        window.Show();
    }

    void OnGUI () {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        name = EditorGUILayout.TextField("Name", name);
        unitMesh = (Mesh)EditorGUILayout.ObjectField("Unit Model", unitMesh, typeof(Mesh));

        maxHealth = EditorGUILayout.FloatField("Max Health", maxHealth);
        moveSpeed = EditorGUILayout.FloatField("Speed (m/s)", moveSpeed);
        type = (UnitType)EditorGUILayout.EnumPopup("Type", type);

        if (GUILayout.Button("Create")) {
            CreateObject();
        }

        void CreateObject () {
            GameObject tempobj = new GameObject(name);
            tempobj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            //Mesh
            MeshRenderer mr = tempobj.AddComponent<MeshRenderer>();
            MeshFilter mf = tempobj.AddComponent<MeshFilter>();
            mf.mesh = unitMesh;

            //Unit
            Unit unit = tempobj.AddComponent<Unit>();
            unit.type = type;
            unit.maxHealth = maxHealth;
            unit.moveSpeed = moveSpeed;
        }
    }
}