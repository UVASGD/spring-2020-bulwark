using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TDCGG {
    public class Tower : MonoBehaviour {
        public Dictionary<TowerPropertyOption, TowerProperty> properties;
        public List<Synergy> synergies;

        public TargetType targetType;
        public float range = 3f;

        public List<Material> materials;
        
        List<GameObject> queue;
        public bool activated;
        public bool targetInRange;
        public bool firing;
        public GameObject currentTarget; //Probably make this a Unit, not GameObject

        public TowerSlot benchSlot;

        void InitProperties () {
            properties = new Dictionary<TowerPropertyOption, TowerProperty>();
            AddProperty(TowerPropertyOption.AttackSpeed, 1f);
            AddProperty(TowerPropertyOption.MinDamage, 1f);
            AddProperty(TowerPropertyOption.MaxDamage, 5f);
            AddProperty(TowerPropertyOption.CritChance, 0.25f);
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

    public enum TargetType { first, last, strongest, weakest }

    public class TowerProperty : MonoBehaviour {
        public new string name;
        public float value;
        private float initValue;
        public List<UnityEvent> events;

        public TowerProperty (string name, float defaultValue) {
            this.name = name;
            value = defaultValue;
        }

        void Init () {
            initValue = value;
        }

        public void Reset () {
            value = initValue;
        }

        public void Add (float x) {
            value += x;
        }

        public void Multiply (float x) {
            value *= x;
        }


    }
}