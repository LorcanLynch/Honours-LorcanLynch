using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public TileMap map;
    GameObject unitTarget;
    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Map").GetComponent<TileMap>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void turnStart()
    {
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("team1");
        int randTarget = Random.Range(0, playerUnits.Length);
        unitTarget = playerUnits[randTarget];

        List<Node> possiblePath = map.GenerateMovePath(gameObject,gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileX);
        print(unitTarget.gameObject.name);
        if (possiblePath != null)
        {
            {
                foreach (Node node in possiblePath)
                {
                    print(node);
                }
            }


            gameObject.GetComponent<UnitScript>().EnterCourse(unitTarget.GetComponent<UnitScript>().tileX, unitTarget.GetComponent<UnitScript>().tileY, possiblePath);
            gameObject.GetComponent<UnitScript>().EnterCourse(unitTarget.GetComponent<UnitScript>().tileX, unitTarget.GetComponent<UnitScript>().tileY, possiblePath);
        }
        StartCoroutine(FinishedMove());
            
    }
    IEnumerator FinishedMove()
    {
        yield return new WaitForSeconds(5F);
        List<Node> possiblePath = map.GenerateAttackPath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileX);
        if (possiblePath.Count -1 < GetComponent<UnitScript>().attackRange)
        {
            GetComponent<UnitScript>().attack(unitTarget);
        }
    }

}
