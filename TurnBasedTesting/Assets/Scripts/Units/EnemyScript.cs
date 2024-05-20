using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        GameObject playerUnit = FindTarget();
       
        List<Node> possiblePath = map.GenerateMovePath(gameObject,gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, playerUnit.GetComponent<UnitScript>().tileY, playerUnit.GetComponent<UnitScript>().tileX);
        
        
        if(possiblePath!= null)
        {
            gameObject.GetComponent<UnitScript>().EnterCourse(playerUnit.GetComponent<UnitScript>().tileX, playerUnit.GetComponent<UnitScript>().tileY, possiblePath);
            gameObject.GetComponent<UnitScript>().EnterCourse(playerUnit.GetComponent<UnitScript>().tileX, playerUnit.GetComponent<UnitScript>().tileY, possiblePath);
        }
      
        
        
        
        return true ;
       
            
    }
    public virtual GameObject FindTarget()
    {
        if (GetComponent<UnitScript>().taunted)
        {

            return GetComponent<UnitScript>().tauntTarget;
        }
        else
        {
            GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("team1");
            GameObject TargetUnit = null;
            
            List<int> playerDistances = new List<int> {0,0,0,0 };
            List<float> playerHealth = new List<float> { 0,0,0,0};
            List<float> playerEvasion = new List<float> { 0,0,0,0};
            List<float> playerAP = new List<float> { 0,0,0,0};
            List<float> playerValues = new List<float> {0,0,0,0 };
            for (int i = 0; i < playerUnits.Length; i++)
            {
                
                if (map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, playerUnits[i].GetComponent<UnitScript>().tileY, playerUnits[i].GetComponent<UnitScript>().tileX) == null)
                {
                    playerDistances[i] = 0;
                }
                else
                {
                    playerDistances[i] = map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, playerUnits[i].GetComponent<UnitScript>().tileY, playerUnits[i].GetComponent<UnitScript>().tileX).Count;
                }
                playerHealth[i] = playerUnits[i].GetComponent<UnitScript>().health;
                playerEvasion[i] = playerUnits[i].GetComponent<UnitScript>().dodgeRating;
                playerAP[i] = playerUnits[i].GetComponent<UnitScript>().attackPower;
            }

            for (int i = 0; i < playerUnits.Length; i++)
            {
                playerValues[i] = 0;
                if (playerDistances[i] <= GetComponent<UnitScript>().moveSpeed + GetComponent<UnitScript>().attackRange)
                {
                   
                    playerValues[i] += 10;
                }
                print(GetComponent<UnitScript>().moveSpeed + GetComponent<UnitScript>().attackRange);
                print(playerDistances[i] + playerUnits[i].name);
                

                playerValues[i] +=  GetComponent<UnitScript>().attackPower / playerHealth[i] ;
                playerValues[i] += (GetComponent<UnitScript>().accuracy/playerEvasion[i])/10  ;
                playerValues[i] += playerAP[i] / 10;
                playerValues[i] += Random.Range(0f, 3f);
                playerValues[i] -= playerUnits[i].GetComponent<UnitScript>().damageReduction / 10;
            }
            float targetValue = 0;
            for (int i = 0; i < playerUnits.Length; i++)
            {
              
                print(playerValues[i]);
                if (targetValue < playerValues[i])
                {
                    
                    targetValue = playerValues[i];
                    
                    TargetUnit = playerUnits[i];
                    
                }
            }
            unitTarget = TargetUnit;
                return TargetUnit;
        }
    }

    public virtual GameObject FindAttackTarget()
    {
        if (GetComponent<UnitScript>().taunted)
        {

            return GetComponent<UnitScript>().tauntTarget;
        }
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("team1");
        GameObject TargetUnit = null;

        List<int> playerDistances = new List<int> { 0, 0, 0, 0 };
        List<float> playerHealth = new List<float> { 0, 0, 0, 0 };
        List<float> playerEvasion = new List<float> { 0, 0, 0, 0 };
        List<float> playerAP = new List<float> { 0, 0, 0, 0 };
        List<float> playerValues = new List<float> { 0, 0, 0, 0 };
        List<bool> playerValid = new List<bool> { false, false, false, false };
        for (int i = 0; i < playerUnits.Length; i++)
        {

            if (map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, playerUnits[i].GetComponent<UnitScript>().tileY, playerUnits[i].GetComponent<UnitScript>().tileX) == null)
            {
                playerDistances[i] = 0;
            }
            else
            {
                playerDistances[i] = map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, playerUnits[i].GetComponent<UnitScript>().tileY, playerUnits[i].GetComponent<UnitScript>().tileX).Count;
            }
            playerHealth[i] = playerUnits[i].GetComponent<UnitScript>().health;
            playerEvasion[i] = playerUnits[i].GetComponent<UnitScript>().dodgeRating;
            playerAP[i] = playerUnits[i].GetComponent<UnitScript>().attackPower;
        }

        for (int i = 0; i < playerUnits.Length; i++)
        {
            playerValues[i] = 0;
            if (playerDistances[i]-1 <=  GetComponent<UnitScript>().attackRange)
            {

                playerValid[i] = true;
            }
            print(GetComponent<UnitScript>().moveSpeed + GetComponent<UnitScript>().attackRange);
            print(playerDistances[i] + playerUnits[i].name);


            playerValues[i] += GetComponent<UnitScript>().attackPower / playerHealth[i];
            playerValues[i] += (GetComponent<UnitScript>().accuracy / playerEvasion[i]) / 10;
            playerValues[i] += playerAP[i] / 10;
            playerValues[i] += Random.Range(0f, 3f);
            playerValues[i] -= playerUnits[i].GetComponent<UnitScript>().damageReduction / 10;
        }
        float targetValue = 0;
        for (int i = 0; i < playerUnits.Length; i++)
        {

            
            if ((targetValue < playerValues[i]) && playerValid[i])
            {

                targetValue = playerValues[i];

                TargetUnit = playerUnits[i];

            }
        }
        unitTarget = TargetUnit;
        return TargetUnit;
    }

    public virtual void FinishedMove()
    {
        GameObject target = FindAttackTarget();
        if (target != null)
        {

            if (GetComponent<UnitScript>().attackAvailable)
            {
                if (target != null)
                {
                    List<Node> possiblePath = map.GenerateAttackPath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileX);
                    if (possiblePath != null)
                    {
                        if (possiblePath.Count - 1 < GetComponent<UnitScript>().attackRange)
                        {
                            GetComponent<UnitScript>().attack(target);
                        }
                    }
                }
            }
        }
    }

}
