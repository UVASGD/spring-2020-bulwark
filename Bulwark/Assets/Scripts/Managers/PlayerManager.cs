using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TDCGG {
    public class PlayerManager : MonoBehaviour {

        public static List<PlayerManager> instance;
        public int playerNum;

        public List<Card> cards;
        public Camera cam;

        [Header("Managers")]
        public GoldManager goldManager;
        public UIManager UIManager;
        public TowerSlotManager towerSlotManager;
        public WaveManager waveManager;
        public HealthManager healthManager;
        public PlacementManager placementManager;
        public SynergyManager synergyManager;


        [Header("Level and XP")]
        public int currentLevel;
        public static int maxLevel = 9;
        [Tooltip("y = sum(x[i]*(level)^i) (i.e. f(x) = [1,2,3] = 1+2x+3x^2...)")]
        public float[] xpToLevelFunction = { 1.33427762f, -0.1883852691f, 0.7946175637f };
        public int currentXP;
        public int neededXP;

        #region GAME LOOP
        private void Awake () {
            if (instance == null) {
                instance = new List<PlayerManager> ();
            }
            playerNum = instance.Count;
            instance.Add (this);

            if (cam == null) {
                cam = GetComponentInChildren<Camera>();
            }

            RegisterManagers();
        }

        //Set references so sub-managers can reference player manager with ease.
        private void RegisterManagers () {
            goldManager.playerManager = this;
            UIManager.playerManager = this;
            towerSlotManager.playerManager = this;
            waveManager.playerManager = this;
            healthManager.playerManager = this;
            placementManager.playerManager = this;
            synergyManager.playerManager = this;
        }

        public void Start () {
            cards = new List<Card>();
            currentLevel = Mathf.Clamp(currentLevel, 1, maxLevel);
            UpdateNeededXP ();

            UIManager.UpdateLevel (currentLevel);
            UIManager.UpdateGold (goldManager.gold);
            UIManager.UpdateXP (currentXP, neededXP);
            Roll ();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.D)) {
                Roll(false);
            }
            else if (Input.GetKeyDown(KeyCode.F)) {
                AddXP(false);
            } else if (Input.GetKeyDown(KeyCode.M)) {
                goldManager.ApplyInterest();
            }
        }
        #endregion

        #region ECONOMY
        public void Roll (bool free = true) {
            if (!free) {
                if (!goldManager.SpendGold(2)) return;
            }

            foreach (CardSlot slot in UIManager.cardSlots) {
                Card card = CardManager.GetCard (currentLevel);
                cards.Add(card);
                slot.PopulateCard(card);
            }
        }

        public void AddXP (bool free = true) {
            //Don't allow player to buy XP if they're already at max level
            if (currentLevel >= maxLevel) {
                return;
            }

            if (!free) {
                if (!goldManager.SpendGold (4)) return;
            }

            currentXP += 4;
            if (currentXP >= neededXP) {
                Level();
                currentXP = currentXP % neededXP;
                UpdateNeededXP();
            }

            if (currentLevel >= maxLevel) {
                //-1 means the UI will display "MAX" instead of XP
                UIManager.UpdateXP (currentXP, -1);
            } else {
                UIManager.UpdateXP (currentXP, neededXP);
            }
        }

        public void Level () {
            if (currentLevel >= maxLevel) return;
            currentLevel++;
            UIManager.UpdateLevel(currentLevel);
        }

        public void UpdateNeededXP () {
            float neededXPFloat = 0;
            for (int i = 0; i < xpToLevelFunction.Length; i++) {
                neededXPFloat += xpToLevelFunction [i] * Mathf.Pow (currentLevel, i);
            }
            neededXP = Mathf.RoundToInt(neededXPFloat);
            if (neededXP % 2 == 1) {
                neededXP--;
            }
        }
        #endregion
    }
}
