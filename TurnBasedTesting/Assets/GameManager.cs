using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Units;
    public TileMap tileMap;
    GameObject previousUnit;
    Node[,] graph;
    // Start is called before the first frame update
    void Start()
    {
        graph = tileMap.graph;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUnit(GameObject unit)
    {
        Units.Add(unit);
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = true;
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].unit = unit;
    }
    
    public void EndTurn()
    {
        foreach(GameObject unit in Units)
        {
            unit.GetComponent<UnitScript>().TurnOver();
          GameObject[] enemies =  GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyScript>().turnStart();
            }
        }
    }

    public void UnitSelected(GameObject unit)
    {
        foreach (GameObject unit_ in Units)
        {
            unit_.GetComponent<SpriteRenderer>().color = Color.white;
        }

        unit.GetComponent<SpriteRenderer>().color = Color.red;
        tileMap.selectedUnit = unit;
    }
    public void UnitMoving(GameObject unit)
    {
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = false;
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].unit = null;
        tileMap.graph = graph;
        
    }

    public void UnitStopped(GameObject unit)
    {
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = true;
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].unit = unit;
        tileMap.graph = graph;
    }
   
}
