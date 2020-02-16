using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public struct BenchS


namespace TDCGG {
    public class TowerSlotManager : MonoBehaviour {
        [HideInInspector] public PlayerManager playerManager; //Set in player manager via an Inspector-referenced variable to this script

        public TowerSlot[] benchSlots;
        public TowerSlot[] fieldSlots;

        public int deployedCount;

        private void Start () {
            RegisterSlots();
            deployedCount = 0;
        }

        public bool CanPlace () {
            return deployedCount < playerManager.currentLevel;
        }

        private void RegisterSlots () {
            foreach (TowerSlot slot in benchSlots) {
                slot.slotManager = this;
                slot.playerManager = playerManager;
                slot.onField = false;
            }
            foreach (TowerSlot slot in fieldSlots) {
                slot.slotManager = this;
                slot.playerManager = playerManager;
                slot.onField = true;
            }
        }

        public bool SpawnTowerOnBench (GameObject tower) {
            return SpawnTowerOnBench(tower, -1);
        }

        public bool SpawnTowerOnBench (GameObject tower, int benchIndex) {
            if (benchIndex < 0) {
                foreach (TowerSlot slot in benchSlots) {
                    if (slot.isVacant) {
                        slot.SpawnTower(tower);
                        return true;
                    }
                }
                return false;
            } else {
                if (benchIndex >= benchSlots.Length) {
                    benchIndex = benchSlots.Length - 1;
                }

                benchSlots[benchIndex].SpawnTower(tower);
                return true;
            }
        }
    }
}