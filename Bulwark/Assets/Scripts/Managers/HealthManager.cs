using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG {
    public class HealthManager : MonoBehaviour {
        [HideInInspector] public PlayerManager playerManager;

        private float health;
        [SerializeField] private float maxHealth = 100f;
        public bool dead;

        private void Awake () {
            health = maxHealth;
            dead = false;
        }

        public void TakeDamage(float damage) {
            if (dead) return;
            health -= damage;
            if (health < 0f) {
                health = 0f;
                playerManager.UIManager.UpdateHealth(health, maxHealth);
                Die();
            } else {
                playerManager.UIManager.UpdateHealth(health, maxHealth);
            }
        }

        public void Die () {
            print("Game over!");
            dead = true;
        }
    }
}