using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TDCCG {
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