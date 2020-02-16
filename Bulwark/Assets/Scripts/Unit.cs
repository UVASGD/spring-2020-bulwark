using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG {
    public enum UnitType { grounded, flying, hybrid, boss }

    public class Unit : MonoBehaviour {
        [HideInInspector]
        public PlayerManager owner;

        [SerializeField]
        private UnitType type;

        [SerializeField]
        private float maxHealth = 100f;
        private float currentHealth;
        public bool alive;

        public float moveSpeed = 5f;

        [SerializeField]
        private UnitPath path;
        private bool traversing;

        private Vector3 target;

        private void Awake () {
            traversing = false;
            alive = true;
            currentHealth = maxHealth;
        }

        public void SetPath (UnitPath path) {
            this.path = path;
        }

        public void TakeDamage (float damage) {
            currentHealth -= damage;
            if (currentHealth <= 0f) {
                currentHealth = 0f;
                Die();
            }
        }

        public void Die () {
            owner.waveManager.currentRemaining--;
            Destroy(gameObject, 0f);
        }

        public void TraversePath () {
            if (path == null || traversing) return;
            StartCoroutine(TraversePathCR());
        }

        private IEnumerator TraversePathCR () {
            traversing = true;
            float t = 0f;
            while (t < 1f) {
                target = path.TraversePath(moveSpeed, ref t);

                //transform.forward = target.normalized;
                //transform.up = Vector3.up;
                transform.LookAt(target + (target - transform.position).normalized * 20f, Vector3.up);
                transform.position = target;
                yield return new WaitForEndOfFrame();
            }
            t = 1f;
            transform.position = path.TraversePath(moveSpeed, ref t);

            owner.healthManager.TakeDamage(currentHealth / 10f);
            Die();
            traversing = false;
        }

        private void OnDrawGizmos () {
            Gizmos.color = Color.cyan;
            //Gizmos.DrawSphere(target + (target - transform.position).normalized * 20f, 3f);
        }
    }
}