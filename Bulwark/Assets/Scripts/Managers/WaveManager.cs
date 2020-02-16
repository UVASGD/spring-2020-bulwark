using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System;

namespace TDCGG {
    public class WaveManager : MonoBehaviour {
         
        [HideInInspector] public PlayerManager playerManager; //Set in player manager via an Inspector-referenced variable to this script

        public int seed = 1337;
        public GameObject[] units; // Sort by unit strength in Inspector
        public int waveNum;
        public float difficulty;
        public List<GameObject> currentWave;
        public int currentRemaining;
        public bool waveActive;

        public UnitPath path;

        private void Awake () {
            waveNum = 0;
            waveActive = false;
            currentRemaining = 0;

            if (path == null) {
                path = transform.parent.GetComponentInChildren<UnitPath>();
            }
        }

        private void Update () {
            if (Input.GetKeyDown(KeyCode.Space)) {
                NextWave();
            }
        }

        public void NextWave () {
            if (waveActive) return;
            waveActive = true;
            GenerateWave();
            StartCoroutine(WaveCR());
        }

        private float Gauss (float z) {
            float a1 = 0.254829592f;
            float a2 = -0.284496736f;
            float a3 = 1.421413741f;
            float a4 = -1.453152027f;
            float a5 = 1.061405429f;
            float p = 0.3275911f;

            int sign = z < 0f ? -1 : 1;
            z = Mathf.Abs(z) / Mathf.Sqrt(2.0f);

            //A&S formula 7.1.26
            float t = 1.0f / (1.0f + p * z);
            float y = 1.0f - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Mathf.Exp(-z * z);
            //print(1.0f + sign * y);
            return (1.0f + sign * y);
        }

        private void GenerateWave () {
            if (currentWave != null) {
                currentWave.Clear();
            } else {
                currentWave = new List<GameObject>();
            }
            difficulty = 2f / (1f + Mathf.Exp(-waveNum / 100f)) - 1f; //Sigmoid function. Starts pseudo-linear at 0, then slowly approaches 1. See in Desmos: \frac{2}{1+e^{-\frac{x}{10}}}-1
            int unitCount = Mathf.RoundToInt(difficulty * 295 + 5); //Number of units: [5, 300), [Level 0, Level infinity)

            float mean = difficulty * units.Length * 4f;
            print("Unit count: " + unitCount);
            string probString = "Unit Probabilities: { ";
            float[] probabilities = new float[units.Length];
            for (int i = 0; i < probabilities.Length; i++) {
                float z = (float)i - mean;
                //if ((waveNum + 1) % 10 == 0) z += 1f;
                probabilities[i] = Gauss(z);
                probString += probabilities[i].ToString() + ", ";
            }
            probString += " }";
            print(probString);

            for (int i = 0; i < unitCount; i++) {
                float val = UnityEngine.Random.value;
                bool unitFound = false;
                for (int j = 0; j < probabilities.Length; j++) {
                    if (val < probabilities[j]) {
                        currentWave.Add(units[j]);
                        unitFound = true;
                        break;
                    }
                }
                if (!unitFound) {
                    currentWave.Add(units[units.Length - 1]);
                }
            }
        }

        IEnumerator WaveCR () {
            Queue<GameObject> toInstantiate = new Queue<GameObject>(currentWave);
            currentRemaining = toInstantiate.Count;
            while (toInstantiate.Count > 0) {
                GameObject instance = Instantiate(toInstantiate.Dequeue(), path ? path.transform.position : Vector3.zero, path ? path.transform.rotation : Quaternion.identity) as GameObject;
                Unit unit = instance.GetComponent<Unit>();

                if (unit == null) {
                    print("Error: " + instance.name + " does not contain a unit component. Deleting.");
                    Destroy(instance);
                    continue;
                }

                unit.SetPath(path);
                unit.owner = playerManager;
                unit.TraversePath();
                yield return new WaitForSeconds(1f / unit.moveSpeed); // Higher movespeed means higher spawn rate to preserve gap between units.
            }

            while (currentRemaining > 0) {
                yield return new WaitForEndOfFrame();
            }

            waveNum++;
            waveActive = false;
        }
    }
}