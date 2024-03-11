using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public TileMap map;
   public  GameObject unitTarget;
   public int aggroRange = 3;
    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Map").GetComponent<TileMap>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

   public virtual bool turnStart()
    {
        
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("team1");
        float targetDistance = 100;
        
        foreach (GameObject target in playerUnits)
        {
            if(map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileX) == null)

            {
                targetDistance = 0;
                unitTarget = target;
                
            }
            else if (map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileX).Count < targetDistance )
            {
                targetDistance = map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileX).Count ;
                unitTarget = target;
            }
        }
        List<Node> possiblePath = map.GenerateMovePath(gameObject,gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileX);
        
        if (possiblePath != null && possiblePath.Count-2 < aggroRange)
        {

            gameObject.GetComponent<UnitScript>().EnterCourse(unitTarget.GetComponent<UnitScript>().tileX, unitTarget.GetComponent<UnitScript>().tileY, possiblePath);
            gameObject.GetComponent<UnitScript>().EnterCourse(unitTarget.GetComponent<UnitScript>().tileX, unitTarget.GetComponent<UnitScript>().tileY, possiblePath);
        }
        
        
        return true ;
       
            
    }
    public virtual void FinishedMove()
    {
        if (unitTarget != null) 
        {
            
            if (GetComponent<UnitScript>().attackAvailable)
            {
                List<Node> possiblePath = map.GenerateAttackPath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileX);
                if (possiblePath.Count - 1 < GetComponent<UnitScript>().attackRange)
                {
                    GetComponent<UnitScript>().attack(unitTarget);
                }
            }
        }
    }

}
