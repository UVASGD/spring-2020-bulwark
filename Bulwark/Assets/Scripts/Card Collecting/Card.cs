using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Card", menuName="Bulwark/Card")]
public class Card : ScriptableObject {
    public new string name;
    public Sprite image;
    public GameObject tower;
    [Range(1,5)]
    public int cost = 1;
}
