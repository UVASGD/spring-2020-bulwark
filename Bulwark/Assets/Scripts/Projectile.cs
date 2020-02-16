using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG {
    public class Projectile : MonoBehaviour {
        public Unit target;
        public bool active = false;
        public float speed = 10f;
        public int durability = 1;
        public float damage = 20f;



        private Vector3 targetPosition;

        public void Activate () {
            active = true;
            targetPosition = transform.forward;
        }

        private void Update () {
            if (!active) return;

            if (target) {
                targetPosition = target.transform.position + target.transform.forward * target.moveSpeed * Time.deltaTime;
            } else {
                targetPosition = transform.forward * 100f;
            }

            transform.position += (targetPosition - transform.position).normalized * speed * Time.deltaTime;
        }

        private void OnTriggerEnter (Collider other) {
            if (other.transform.root.GetComponent<Unit>()) {
                other.transform.root.GetComponent<Unit>().TakeDamage(damage);
            }
            durability--;
            if (durability <= 0) {
                //TODO: Add explody bit, don't just destroy 
                Destroy(gameObject);
            }
        }
    }
}