using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TDCGG;

public class CreateTowerWindow : EditorWindow {
    new string name = "New Tower";
    Mesh towerBase;
    Mesh towerGun;
    TowerTargetType targetType;
    float range = 5f;
    float attackSpeed = 1f;
    float critChance = 0.25f;
    float minDamage = 1f;
    float maxDamage = 2f;


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
        towerBase = (Mesh)EditorGUILayout.ObjectField("Tower Model", towerBase, typeof(Mesh));
        range = EditorGUILayout.FloatField("Attack Range", range);
        attackSpeed = EditorGUILayout.FloatField("Attack Speed", attackSpeed);
        critChance = EditorGUILayout.FloatField("Critical Strike Chance", critChance);
        minDamage = EditorGUILayout.FloatField("Minimum Damage", minDamage);
        maxDamage = EditorGUILayout.FloatField("Maximum Damage", maxDamage);

        if (GUILayout.Button("Create")) {
            CreateObject();
        }

        void CreateObject () {
            GameObject tempobj = new GameObject(name);
            tempobj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            //Mesh
            MeshRenderer mr = tempobj.AddComponent<MeshRenderer>();
            MeshFilter mf = tempobj.AddComponent<MeshFilter>();
            mf.mesh = towerBase;

            //Tower
            Tower tower = tempobj.AddComponent<Tower>();
            tower.properties[Tower.TowerPropertyOption.Range] = new TDCCG.TowerProperty("range", range);
            tower.properties[Tower.TowerPropertyOption.AttackSpeed] = new TDCCG.TowerProperty("attackSpeed", attackSpeed);
            tower.properties[Tower.TowerPropertyOption.CritChance] = new TDCCG.TowerProperty("critChance", critChance);
            tower.properties[Tower.TowerPropertyOption.MinDamage] = new TDCCG.TowerProperty("minDamage", minDamage);
            tower.properties[Tower.TowerPropertyOption.MaxDamage] = new TDCCG.TowerProperty("maxDamage", maxDamage);
        }
    }
}