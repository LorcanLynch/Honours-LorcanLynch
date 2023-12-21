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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 6)
    //    {
    //        map.UnitStopped(collision.gameObject);

    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 6)
    //    {
    //        map.UnitMoving(collision.gameObject);

    //    }
    //}
}
