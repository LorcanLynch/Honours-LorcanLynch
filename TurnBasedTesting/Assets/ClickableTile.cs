using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;

    public TileMap map;

    // Start is called before the first frame update
     void OnMouseUp()
    {
        print(tileX + tileY.ToString());
        map.GeneratePathTo(tileY, tileX,true);
    }
}
