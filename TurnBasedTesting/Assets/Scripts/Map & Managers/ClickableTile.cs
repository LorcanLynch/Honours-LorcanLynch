using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public Color prev = Color.white;
    public TileMap map;

    // Start is called before the first frame update
     void OnMouseUp()
    {
        
           
            map.GeneratePathTo(tileY, tileX, true);
        
    }
   
    private void OnMouseEnter()
    {
        prev = GetComponent<SpriteRenderer>().color;
        if (prev != Color.red)
        {

            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    private void OnMouseExit()
    {
        if (GetComponent<SpriteRenderer>().color != Color.red && prev!= Color.red)
        {
            GetComponent<SpriteRenderer>().color = prev;
        }
    }



    
}
