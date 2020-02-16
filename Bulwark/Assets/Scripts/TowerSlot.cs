using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TDCGG {
    public class TowerSlot : MonoBehaviour {

        [HideInInspector] public TowerSlotManager slotManager;
        [HideInInspector] public PlayerManager playerManager;

        public Transform spawnPoint;
        public bool isVacant = true;
        public GameObject tower;
        public bool onField;

        private void Start () {
            if (spawnPoint == null) {
                spawnPoint = transform.Find("SpawnPoint");
                if (spawnPoint == null) {
                    Debug.LogError("SpawnPoint missing from bench slot!");
                }
            }
            isVacant = true;
        }

        public GameObject RemoveTower (bool destructive = false) {
            if (isVacant) return null;
            GameObject temp = tower;
            tower = null;
            //print("Tower: " + tower);
            isVacant = true;

            if (onField) {
                slotManager.deployedCount--;
            }

            if (destructive) {
                Destroy(temp);
                return null;
            } else {
                return temp;
            }
            
        }

        public bool PlaceTower (GameObject instance) {
            if (!isVacant) return false;
            //print("Bench: " + name);
            tower = instance;
            tower.transform.position = spawnPoint.position;
            tower.GetComponent<Tower>().benchSlot = this;
            isVacant = false;

            if (onField) {
                slotManager.deployedCount++;
                tower.GetComponent<Tower>().Activate();
            } else {
                tower.GetComponent<Tower>().Deactivate();
            }

            return true;
        }

        public bool SpawnTower (GameObject prefab) {
            if (!isVacant) return false;
            tower = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
            return PlaceTower(tower);
        }
    }
}