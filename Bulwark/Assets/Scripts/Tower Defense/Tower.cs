using System.Collections;
using System.Collections.Generic;
using TDCCG;
using UnityEngine;
using UnityEngine.Events;

namespace TDCGG {
    public class Tower : MonoBehaviour {
        public Dictionary<TowerPropertyOption, TowerProperty> properties;

        [Header("Synergy")]
        public List<Synergy> synergies;

        [Header("Targeting")]
        public TowerTargetType targetType;

        [HideInInspector]
        public List<Material> materials;

        [Header("Default Stats")]
        public float defaultRange = 3f;
        public float defaultAttackSpeed = 1f;
        public float defaultMinDamage = 1f;
        public float defaultMaxDamage = 2f;
        public float defaultCritChance = 0.25f;

        [Header("Realtime Info")]
        public List<GameObject> queue;
        public bool activated;
        public bool targetInRange;
        public bool firing;
        public GameObject currentTarget; //TODO: Probably make this a Unit, not GameObject

        public TowerSlot benchSlot;

        void InitProperties () {
            properties = new Dictionary<TowerPropertyOption, TowerProperty>();
            AddProperty(TowerPropertyOption.Range, defaultRange);
            AddProperty(TowerPropertyOption.AttackSpeed, defaultAttackSpeed);
            AddProperty(TowerPropertyOption.MinDamage, defaultMinDamage);
            AddProperty(TowerPropertyOption.MaxDamage, defaultMaxDamage);
            AddProperty(TowerPropertyOption.CritChance, defaultCritChance);
            AddProperty(TowerPropertyOption.SlowPercent, 0.0f);
            AddProperty(TowerPropertyOption.FreezePercent, 0.0f);
            AddProperty(TowerPropertyOption.StunPercent, 0.0f);
            AddProperty(TowerPropertyOption.RootPercent, 0.0f);
            AddProperty(TowerPropertyOption.BurnPercent, 0.0f);
            AddProperty(TowerPropertyOption.SunderPercent, 0.0f);
            AddProperty(TowerPropertyOption.ShatterPercent, 0.0f);
            AddProperty(TowerPropertyOption.SinPercent, 0.0f);
        }

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
                case TowerTargetType.first:
                    return queue[0];
                case TowerTargetType.last:
                    return queue[queue.Count - 1];
                case TowerTargetType.strongest:
                    //TODO
                    return null;
                case TowerTargetType.weakest:
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
            float maxCD = properties[TowerPropertyOption.AttackSpeed].value;
            float currentCD = maxCD;
            while (currentTarget != null) {
                if (currentCD <= 0f) {
                    maxCD = properties[TowerPropertyOption.AttackSpeed].value;
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

        #region PROPERTIES
        public enum TowerPropertyOption {
            Range,
            AttackSpeed,
            MinDamage,
            MaxDamage,
            CritChance,
            SlowPercent,
            FreezePercent,
            StunPercent,
            RootPercent,
            BurnPercent,
            SunderPercent,
            ShatterPercent,
            SinPercent,
        }

        public void AddProperty (TowerPropertyOption type, float value) {
            KeyValuePair<TowerPropertyOption, TowerProperty> kvp = GetProperty(type, value);
            properties.Add(kvp.Key, kvp.Value);
        }

        public static KeyValuePair<TowerPropertyOption, TowerProperty> GetProperty (TowerPropertyOption type, float value) {
            var kvp = new KeyValuePair<TowerPropertyOption, TowerProperty>(type,
                new TowerProperty(type.ToString(), value));
            return kvp;
        }
        #endregion
    }

    public enum TowerTargetType { first, last, strongest, weakest }
}