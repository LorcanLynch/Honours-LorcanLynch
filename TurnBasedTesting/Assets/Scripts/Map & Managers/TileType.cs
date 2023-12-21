using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TileType
{
    public string name;
    public GameObject tileVisual;

    public bool isWalkable;
    public int moveCost;
}
