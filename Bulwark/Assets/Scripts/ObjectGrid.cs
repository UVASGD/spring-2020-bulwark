using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectGrid : MonoBehaviour {
    public int rowCount = 3;
    public float cellSize = 2f;

    int i;
    float x;
    float y;

    private void OnValidate () {
        i = 0;
        foreach (Transform child in transform) {
            x = (i % rowCount) * cellSize;
            y = (i / rowCount) * cellSize;

            child.position = new Vector3(x,0f,y);
            i++;
        }
    }
}
