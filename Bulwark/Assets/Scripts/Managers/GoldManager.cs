using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG {
    public class GoldManager : MonoBehaviour {
        [HideInInspector] public PlayerManager playerManager; //Set in player manager via an Inspector-referenced variable to this script

        public int gold = 0;
        public int maxGold = 200;

        [Tooltip ("y = sum(x[i]*(gold)^i) (i.e. [1,2,3] = 1+2x+3x^2...)")]
        public static float [] interestFunction = { 5f, 0.1f };

        public bool CheckPurchase (int amount) {
            if (amount > gold) {
                print("Insufficient funds.");
                return false;
            }
            return true;
        }

        public bool SpendGold (int amount) {
            if (!CheckPurchase(amount)) {
                return false;
            }
            gold -= amount;
            playerManager.UIManager.UpdateGold(gold);
            return true;
        }

        public void AddGold (int amount) {
            gold += amount;
            if (gold > maxGold) {
                gold = maxGold;
            }
            playerManager.UIManager.UpdateGold (gold);
        }

        public void ApplyInterest () {
            float toAdd = 0f;
            for (int i = 0; i < interestFunction.Length; i++) {
                toAdd += interestFunction [i] * Mathf.Pow (gold, i);
            }
            gold += Mathf.FloorToInt (toAdd);
            playerManager.UIManager.UpdateGold(gold);
        }
    }
}