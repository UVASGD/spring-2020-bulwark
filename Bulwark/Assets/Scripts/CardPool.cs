using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG
{
    [System.Serializable]
    public class CardPool {
        [Range(0, 100)] [SerializeField] private int cardCount = 1;
        [SerializeField] private List<Card> cards = null;
        private List<Card> pool;

        public void Init () {
            pool = new List<Card>();
            for (int i = 0; i < cardCount; i++)
            {
                foreach (Card card in cards)
                {
                    pool.Add(card);
                }
            }
            ShuffleCards();
        }

        public void ShuffleCards () {
            int n = pool.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n);
                Card value = pool[k];
                pool[k] = pool[n];
                pool[n] = value;
            }
        }

        public Card PullRandomCard () {
            int index = Random.Range(0, pool.Count);
            Card cardToPull = pool[index];
            //pool.RemoveAt(index);
            return cardToPull;
        }

        public int GetPoolSize () {
            return cards.Count;
        }
    }
}