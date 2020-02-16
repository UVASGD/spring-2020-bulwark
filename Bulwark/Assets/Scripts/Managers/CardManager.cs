using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG {
    /// <summary>
    /// This is currently the only manager that is global. Meaning there is one per game, not one per player.
    /// </summary>
    public class CardManager : MonoBehaviour {
        public static CardManager instance;

        public CardPool[] cardPools;
        public float[,] probabilityMatrix = {
            {1.00f, 1.00f, 0.70f, 0.50f, 0.35f, 0.35f, 0.20f, 0.15f, 0.10f},
            {0.00f, 0.00f, 0.25f, 0.35f, 0.35f, 0.35f, 0.30f, 0.20f, 0.15f},
            {0.00f, 0.00f, 0.05f, 0.15f, 0.25f, 0.25f, 0.33f, 0.35f, 0.30f},
            {0.00f, 0.00f, 0.00f, 0.00f, 0.05f, 0.05f, 0.15f, 0.22f, 0.30f},
            {0.00f, 0.00f, 0.00f, 0.00f, 0.00f, 0.00f, 0.02f, 0.08f, 0.15f},
            //{1.00f, 0.00f, 0.00f, 0.00f, 0.00f}, // Level 1
            //{1.00f, 0.00f, 0.00f, 0.00f, 0.00f}, // Level 2
            //{0.70f, 0.25f, 0.05f, 0.00f, 0.00f}, // Level 3
            //{0.50f, 0.35f, 0.15f, 0.00f, 0.00f}, // Level 4
            //{0.35f, 0.35f, 0.25f, 0.05f, 0.00f}, // Level 5
            //{0.35f, 0.35f, 0.25f, 0.05f, 0.00f}, // Level 6
            //{0.20f, 0.30f, 0.33f, 0.15f, 0.02f}, // Level 7
            //{0.15f, 0.20f, 0.35f, 0.22f, 0.08f}, // Level 8
            //{0.10f, 0.15f, 0.30f, 0.30f, 0.15f}, // Level 9
        };

        public static Card GetCard (int level) {
            float rand = Random.value;
            float offset = 0f;
            for (int i = 0; i < instance.cardPools.Length; i++) {
                if (level > PlayerManager.maxLevel) {
                    level = PlayerManager.maxLevel;
                }
                float prob = instance.probabilityMatrix[i, level-1];
                //print("Prob of " + (i+1) + " cost at level " + level + ": " + prob);
                if (rand < prob + offset) {
                    if (instance.cardPools[i].GetPoolSize() > 0) {
                        return instance.cardPools[i].PullRandomCard();
                    }
                } else {
                    offset += prob;
                }
            }
            return null;
        }

        public static void GeneratePools () {
            foreach (var cardPool in instance.cardPools) {
                cardPool.Init();
            }
        }

        private void Awake() {
            if (instance) {
                Destroy(gameObject);
            } else {
                instance = this;
            }
            GeneratePools();
        }
    }
}
