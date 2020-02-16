using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG {
    public class PlacementManager : MonoBehaviour {
        [HideInInspector]
        public PlayerManager playerManager;

        public LayerMask placementLayer;
        public LayerMask towerLayer;
        public LayerMask unitLayer;
        public LayerMask mapLayer;

        private Camera cam;

        RaycastHit placementHit;
        RaycastHit towerHit;
        RaycastHit unitHit;
        RaycastHit mapHit;

        public Tower heldTower;

        private void Start () {
            cam = playerManager.cam;
        }

        void Update () {
            

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out towerHit, 1000f, towerLayer)) {
                //print("Tower: " + towerHit.transform.name);
            }

            if (Physics.Raycast(ray, out unitHit, 1000f, unitLayer)) {
                //print("Unit: " + unitHit.transform.name);
            }

            if (Physics.Raycast(ray, out placementHit, 1000f, placementLayer)) {
                //print("Slot: " + placementHit.transform.name);
            }

            if (Physics.Raycast(ray, out mapHit, 1000f, mapLayer)) {
                if (heldTower) {
                    heldTower.transform.position = mapHit.point;
                }
            } else {
                
            }

            if (Input.GetMouseButtonDown(0) && heldTower == null) {
                if (towerHit.transform != null) {
                    heldTower = towerHit.transform.GetComponent<Tower>();
                    heldTower.benchSlot.RemoveTower();
                }

                if (unitHit.transform != null) {
                    //print("INFO ABOUT UNIT: " + unitHit.transform.name);
                }


            } else if (Input.GetMouseButtonUp(0)) {
                if (heldTower != null) {
                    /* If nothing hovering, return tower
                     * Otherwise, if it's a field slot and no wave is active,
                     *   
                     * 
                     * 
                     * 
                     * 
                     */

                    if (placementHit.transform != null) {
                        TowerSlot slot = null;
                        if (slot = placementHit.transform.GetComponent<TowerSlot>()) {
                            //Field slot
                            if (slot.onField) {
                                //Wave Active
                                if (playerManager.waveManager.waveActive) {
                                    heldTower.benchSlot.PlaceTower(heldTower.gameObject);
                                    return;
                                }
                                //Wave Inactive
                                else {
                                    //Slot empty
                                    if (slot.isVacant) {
                                        //Not reached max units
                                        if (slot.slotManager.CanPlace()) {
                                            heldTower.benchSlot = slot;
                                            slot.PlaceTower(heldTower.gameObject);
                                            heldTower = null;
                                            return;
                                        }
                                        //Max units
                                        else {
                                            heldTower.benchSlot.PlaceTower(heldTower.gameObject);
                                            return;
                                        }
                                    }
                                    //Slot filled
                                    else {
                                        //Swap towers
                                        GameObject temp = slot.RemoveTower();
                                        slot.PlaceTower(heldTower.gameObject);
                                        heldTower = temp.GetComponent<Tower>();
                                        return;
                                    }
                                }
                            }
                            // Bench slot
                            else {
                                //Slot empty
                                if (slot.isVacant) {
                                    heldTower.benchSlot = slot;
                                    slot.PlaceTower(heldTower.gameObject);
                                    heldTower = null;
                                    return;
                                }
                                //Slot filled
                                else {
                                    //Swap towers
                                    GameObject temp = slot.RemoveTower();
                                    slot.PlaceTower(heldTower.gameObject);
                                    heldTower = temp.GetComponent<Tower>();
                                    return;
                                }
                            }
                        } else {
                            heldTower.benchSlot.PlaceTower(heldTower.gameObject);
                        }
                    } else {
                        heldTower.benchSlot.PlaceTower(heldTower.gameObject);
                    }

                }
            }
        }
    }
}