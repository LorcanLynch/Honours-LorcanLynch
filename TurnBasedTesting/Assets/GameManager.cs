using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Units;
    public TileMap tileMap;
    GameObject previousUnit;
    Node[,] graph;
    public GameObject unitPrefab;
    public List<GameObject> unitsSpawnable;
    List<Node> spawnableTiles = new List<Node> { };   
    // Start is called before the first frame update
    void Start()
    {
          
            
       
        
    }
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyUp(KeyCode.Y))
        {
            
                SpawnUnit(1, 3);
            
         
        }
    }

    void SpawnUnit()
    {
        spawnableTiles.Clear();
        for (int x = 0; x < tileMap.mapSizeX; x++)
        {
            for (int y = 0; y < tileMap.mapSizeY; y++)
            {

                if (tileMap.tiles[x, y] == 0 || tileMap.tiles[x, y] == 2 || tileMap.tiles[x, y] == 3)
                {
                    spawnableTiles.Add(tileMap.graph[x, y]);
                }

            }
        }
        //print(spawnableTiles.Count);
        
        int spawnLoc = UnityEngine.Random.Range(0, spawnableTiles.Count);
        if(tileMap.graph[spawnableTiles[spawnLoc].x, spawnableTiles[spawnLoc].y].containsUnit == false)
        {
            unitsSpawnable = tileMap.GetComponent<ObjectiveScript>().spawningPool;
            int spawned = UnityEngine.Random.Range(0, unitsSpawnable.Count) ;
            GameObject unit = Instantiate(unitsSpawnable[spawned]);
            tileMap.graph[spawnableTiles[spawnLoc].x, spawnableTiles[spawnLoc].y].containsUnit = true;
            unit.GetComponent<UnitScript>().tileX = spawnableTiles[spawnLoc].x;
            unit.GetComponent<UnitScript>().tileY = spawnableTiles[spawnLoc].y;
            
            
        }
        else
        {
            print("happened");
            SpawnUnit();
        }
        
        

    }
    
    public void SpawnUnit(int tileX,int tileY)
    {
        if(tileX< 0 ||  tileY<0|| tileX>tileMap.mapSizeX || tileY> tileMap.mapSizeY)
        {
            return;
        }    
        spawnableTiles.Clear();
        for (int x = 0; x < tileMap.mapSizeX; x++)
        {
            for (int y = 0; y < tileMap.mapSizeY; y++)
            {

                if (tileMap.tiles[x, y] == 0 || tileMap.tiles[x, y] == 2 || tileMap.tiles[x, y] == 3)
                {
                    spawnableTiles.Add(tileMap.graph[x, y]);
                }

            }
        }
        //print(spawnableTiles.Count);

       
        if (tileMap.graph[tileX,tileY].containsUnit == false && (tileMap.tiles[tileX, tileY] == 0|| tileMap.tiles[tileX, tileY] == 2|| tileMap.tiles[tileX, tileY] == 3))
        {
            unitsSpawnable = tileMap.GetComponent<ObjectiveScript>().spawningPool;
            int spawned = UnityEngine.Random.Range(0, unitsSpawnable.Count);
            GameObject unit = Instantiate(unitsSpawnable[spawned]);
           
            tileMap.graph[tileX,tileY].containsUnit = true;
           
           
            unit.GetComponent<UnitScript>().tileX = tileX;
            unit.GetComponent<UnitScript>().tileY = tileY;
            unit.transform.position = tileMap.TileCoordToWorldCoord(tileX, tileY);


        }
        else
        {
            print("happened");
            int i = 0;
            foreach(Node neighbour in tileMap.graph[tileX,tileY].connections)
            {
                i++;
                if (neighbour.containsUnit == false && (tileMap.tiles[neighbour.x, neighbour.y] == 0 || tileMap.tiles[neighbour.x, neighbour.y] == 2 || tileMap.tiles[neighbour.x, neighbour.y] == 3))
                {
                    SpawnUnit(neighbour.x, neighbour.y);
                    break;
                }
                if(i>5)
                {
                   
                    SpawnUnit(tileX, tileY +1);
                   
                }
                
                
            }
           
        }



    }

    public void SpawnUnit(int tileX, int tileY, bool survive)
    {
        if (tileX < 0 || tileY < 0 || tileX > tileMap.mapSizeX || tileY > tileMap.mapSizeY)
        {
            return;
        }
        spawnableTiles.Clear();
        for (int x = 0; x < tileMap.mapSizeX; x++)
        {
            for (int y = 0; y < tileMap.mapSizeY; y++)
            {

                if (tileMap.tiles[x, y] == 0 || tileMap.tiles[x, y] == 1 || tileMap.tiles[x, y] == 3)
                {
                    spawnableTiles.Add(tileMap.graph[x, y]);
                }

            }
        }
        //print(spawnableTiles.Count);


        if (tileMap.graph[tileX, tileY].containsUnit == false && (tileMap.tiles[tileX, tileY] == 0 || tileMap.tiles[tileX, tileY] == 2 || tileMap.tiles[tileX, tileY] == 3))
        {
            unitsSpawnable = tileMap.GetComponent<ObjectiveScript>().spawningPool;
            int spawned = UnityEngine.Random.Range(0, unitsSpawnable.Count);
            GameObject unit = Instantiate(unitsSpawnable[spawned]);
            unit.GetComponent<UnitScript>().tileX = tileX;
            unit.GetComponent<UnitScript>().tileY = tileY;
            unit.GetComponent<EnemyScript>().aggroRange = 200;


        }
        else
        {
            print("happened");
            int i = 0;
            foreach (Node neighbour in tileMap.graph[tileX, tileY].connections)
            {
                i++;
                if (neighbour.containsUnit == false && (tileMap.tiles[neighbour.x, neighbour.y] == 0 || tileMap.tiles[neighbour.x, neighbour.y] == 2  || tileMap.tiles[neighbour.x, neighbour.y] == 3))
                {
                    SpawnUnit(neighbour.x, neighbour.y);
                    break;
                }
                if (i > 5)
                {

                    SpawnUnit(tileX, tileY + 1);

                }


            }

        }



    }


}
