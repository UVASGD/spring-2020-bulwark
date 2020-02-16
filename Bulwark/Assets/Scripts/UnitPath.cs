using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG {
    public class UnitPath : MonoBehaviour {
        [SerializeField] private BezierSpline spline;

        private void Awake () {
            if (spline == null) {
                spline = GetComponent<BezierSpline>();
            }
        }

        public Vector3 TraversePath (float speed, ref float t) {
            t = Mathf.Clamp01(t);
            Vector3 target = spline.GetPoint(t);
            Vector3 velocity = spline.GetVelocity(t);
            t += Time.deltaTime * speed / velocity.magnitude;
            return target;
        }
    }
}