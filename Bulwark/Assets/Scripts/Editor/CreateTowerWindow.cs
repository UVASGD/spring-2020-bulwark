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
    Synergy synergy1;
    Synergy synergy2;
    Synergy synergy3;

    Synergy[] synergyOptions;
    string[] synergyNames;
    int synergyIndex1;
    int synergyIndex2;
    int synergyIndex3;


    // Add menu named "My Window" to the Window menu
    [MenuItem("Bulwark/Create Tower")]
    static void Init () {
        // Get existing open window or if none, make a new one:
        CreateTowerWindow window = (CreateTowerWindow)GetWindow(typeof(CreateTowerWindow));
        window.Show();
    }

    void OnGUI () {
        synergyOptions = Resources.LoadAll<Synergy>("Synergies");
        synergyNames = new string[synergyOptions.Length+1];
        synergyNames[0] = "None";
        for (int i = 1; i < synergyNames.Length; i++) {
            synergyNames[i] = synergyOptions[i-1].name;
        }

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        name = EditorGUILayout.TextField("Name", name);
        towerBase = (Mesh)EditorGUILayout.ObjectField("Tower Model", towerBase, typeof(Mesh));
        range = EditorGUILayout.FloatField("Attack Range", range);
        attackSpeed = EditorGUILayout.FloatField("Attack Speed", attackSpeed);
        critChance = EditorGUILayout.FloatField("Critical Strike Chance", critChance);
        minDamage = EditorGUILayout.FloatField("Minimum Damage", minDamage);
        maxDamage = EditorGUILayout.FloatField("Maximum Damage", maxDamage);
        synergyIndex1 = EditorGUILayout.Popup("Synergy 1", synergyIndex1, synergyNames);
        synergyIndex2 = EditorGUILayout.Popup("Synergy 2", synergyIndex2, synergyNames);
        synergyIndex3 = EditorGUILayout.Popup("Synergy 3", synergyIndex3, synergyNames);

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
            tower.defaultRange = range;
            tower.defaultAttackSpeed = attackSpeed;
            tower.defaultMinDamage = minDamage;
            tower.defaultMaxDamage = maxDamage;
            tower.defaultCritChance = critChance;

            tower.synergies = new List<Synergy>();
            if (synergyIndex1 > 0) {
                tower.synergies.Add(synergyOptions[synergyIndex1 - 1]);
            }
            if (synergyIndex2 > 0) {
                tower.synergies.Add(synergyOptions[synergyIndex2 - 1]);
            }
            if (synergyIndex3 > 0) {
                tower.synergies.Add(synergyOptions[synergyIndex3 - 1]);
            }
        }
    }
}