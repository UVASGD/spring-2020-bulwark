using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG {
    public enum TargetType { first, last, strongest, weakest }

    public class Tower : MonoBehaviour {
        public TargetType targetType;
        public float range = 3f;

        public List<Material> materials;

        List<GameObject> queue;
        public bool activated;
        public bool targetInRange;
        public bool firing;
        public GameObject currentTarget; //Probably make this a Unit, not GameObject

        public TowerSlot benchSlot;

        void Start () {
            queue = new List<GameObject>();
            activated = false;
            targetInRange = false;
            firing = false;
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers) {
                foreach (Material mat in meshRenderer.materials) {
                    materials.Add(mat);
                }
            }
            StartCoroutine(Dissolve(false));
        }

        IEnumerator Dissolve(bool fadeOut) {
            float t = fadeOut ? 0f : 1f;
            while (fadeOut ? t < 1f : t > 0f) {
                //print("BUP");
                materials.ForEach(material => material.SetFloat("_Dissolve", t));
                t += (fadeOut ? 1 : -1) * Time.deltaTime; // Half a second
                yield return new WaitForEndOfFrame();
            }
            materials.ForEach(material => material.SetFloat("_Dissolve", fadeOut ? 1f : 0f));
        }

        GameObject GetTarget () {
            if (queue.Count == 0) return null;
            switch (targetType) {
                case TargetType.first:
                    return queue[0];
                case TargetType.last:
                    return queue[queue.Count - 1];
                case TargetType.strongest:
                    //TODO
                    return null;
                case TargetType.weakest:
                    //TODO
                    return null;
                default:
                    return null;
            }
        }

        void OnTriggerEnter (Collider other) {
            if (!targetInRange) targetInRange = true;
            queue.Add(other.gameObject);
            currentTarget = GetTarget();
        }

        void OnTriggerExit (Collider other) {
            queue.Remove(other.gameObject);
            if (queue.Count == 0) targetInRange = false;
            else currentTarget = GetTarget();
        }

        void Update () {
            if (!activated) return;
            if (targetInRange && currentTarget == null) {
                currentTarget = GetTarget();
            }
            if (targetInRange && !firing) {
                Fire();
            }
        }

        void Fire () {
            StartCoroutine(FireCR());
        }

        IEnumerator FireCR () {
            float maxCD = 2f;
            float currentCD = maxCD;
            while (currentTarget != null) {
                if (currentCD <= 0f) {
                    currentCD = maxCD;
                    print(gameObject.name + "firing on " + currentTarget);
                }
                currentCD -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        public void Activate () {
            if (activated) return;
            activated = true;
        }

        public void Deactivate () {
            if (!activated) return;
            activated = false;
        }
    }
}