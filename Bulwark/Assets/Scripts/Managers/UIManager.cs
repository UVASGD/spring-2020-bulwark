using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Michsky.UI.ModernUIPack;

namespace TDCGG {
    public class UIManager : MonoBehaviour {

        [HideInInspector] public PlayerManager playerManager; //Set in player manager via an Inspector-referenced variable to this script

        public TMP_Text levelText;
        public ProgressBar XPProgressBar;
        public TMP_Text goldText;
        public CardSlot [] cardSlots;
        public ProgressBar healthBar;
        

        private void Start () {
            foreach (CardSlot slot in cardSlots) {
                slot.owner = playerManager;
            }            
        }

        public void UpdateXP (int currentXP, int maxXP) {
            XPProgressBar.UpdateProgress (currentXP, maxXP);
        }

        public void UpdateLevel (int level) {
            levelText.SetText (level.ToString());
        }

        public void UpdateGold (int gold) {
            goldText.SetText (gold.ToString());
        }

        public void UpdateHealth(float currentHealth, float maxHealth) {
            healthBar.UpdateProgress(Mathf.RoundToInt(currentHealth), Mathf.RoundToInt(maxHealth));
        }
    }
}