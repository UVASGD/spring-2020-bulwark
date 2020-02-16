using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TDCGG {
    public class CardSlot : MonoBehaviour {

        public PlayerManager owner;

        public Card card;

        public Button button;
        public new TMP_Text name;
        public TMP_Text cost;
        public Image image;

        private void Start () {
            if (button == null) {
                button = gameObject.GetComponentInChildren<Button>();
            }
            if (name == null) {
                name = transform.Find("Name").GetComponent<TMP_Text>();
            }
            if (cost == null) {
                cost = transform.Find("Cost").GetComponent<TMP_Text>();
            }
            if (image == null) {
                image = transform.GetComponent<Image>();
            }
        }

        public void PopulateCard(Card card) {
            if (this.card != null) {
                owner.cards.Remove (card);
            }

            if (card == null) {
                button.interactable = false;
                this.card = null;
                this.name.SetText ("");
                this.cost.SetText ("");
                this.image.sprite = null;
            } else {
                button.interactable = true;
                this.card = card;
                this.name.SetText (card.name);
                this.cost.SetText (card.cost.ToString ());
                this.image.sprite = card.image;
            }
        }

        public void PurchaseCard () {
            if (card == null) return;

            if (owner.goldManager.CheckPurchase(card.cost)) {
                if (owner.towerSlotManager.SpawnTowerOnBench(card.tower)) {
                    PopulateCard(null);
                }
            }
        }
    }
}
