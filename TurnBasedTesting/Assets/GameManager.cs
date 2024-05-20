using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Units;
    public TileMap tileMap;
    GameObject previousUnit;
    Node[,] graph;
    public GameObject pauseMenu;
    public List<GameObject> unitsSpawnable;
    public bool paused = false;
    List<Node> spawnableTiles = new List<Node> { };
    public AudioSource aS;
    // Start is called before the first frame update
    void Start()
    {


        aS = GetComponent<AudioSource>();
        
    }
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SpawnUnit(10, 10);
        }
    }

    public void Restart()
    {
        aS.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
    }
   public void Pause()
    {
        aS.Play();
        if (!paused)
        {
            Time.timeScale = 0;
            paused = true;
           pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            paused = false;
            pauseMenu.SetActive(false);
        }

    }

    public void Quit()
    {
        aS.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
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


        if (tileMap.graph[tileX, tileY].containsUnit == false)
        {
           
            if ((tileMap.tiles[tileX, tileY] == 0 || tileMap.tiles[tileX, tileY] == 2 || tileMap.tiles[tileX, tileY] == 3))
            {
                unitsSpawnable = tileMap.GetComponent<ObjectiveScript>().spawningPool;
                int spawned = UnityEngine.Random.Range(0, unitsSpawnable.Count);
                GameObject unit = Instantiate(unitsSpawnable[spawned]);
                unit.GetComponent<UnitScript>().tileX = tileX;
                unit.GetComponent<UnitScript>().tileY = tileY;
                unit.GetComponent<EnemyScript>().aggroRange = 200;
                tileMap.graph[tileX, tileY].containsUnit = true;

            }
        }
        else
        {
            
           
            int i = 0;
            foreach (Node neighbour in tileMap.graph[tileX, tileY].connections)
            {
                i++;
                if (neighbour.containsUnit == false)
                { 
                    if (  tileMap.tiles[neighbour.x, neighbour.y] == 0 || tileMap.tiles[neighbour.x, neighbour.y] == 2 || tileMap.tiles[neighbour.x, neighbour.y] == 3)
                    {
                        SpawnUnit(neighbour.x, neighbour.y);
                        break;
                    }
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

        if (tileMap.graph[tileX, tileY].containsUnit == false)
        {
           
            if ((tileMap.tiles[tileX, tileY] == 0 || tileMap.tiles[tileX, tileY] == 2 || tileMap.tiles[tileX, tileY] == 3))
            {
                unitsSpawnable = tileMap.GetComponent<ObjectiveScript>().spawningPool;
                int spawned = UnityEngine.Random.Range(0, unitsSpawnable.Count);
                GameObject unit = Instantiate(unitsSpawnable[spawned]);
                unit.GetComponent<UnitScript>().tileX = tileX;
                unit.GetComponent<UnitScript>().tileY = tileY;
                unit.GetComponent<EnemyScript>().aggroRange = 200;

                tileMap.graph[tileX, tileY].containsUnit = true;
            }
        }
        else
        {
           
           
            int i = 0;
            foreach (Node neighbour in tileMap.graph[tileX, tileY].connections)
            {
                if (neighbour.containsUnit == false)
                {
                    if (tileMap.tiles[neighbour.x, neighbour.y] == 0 || tileMap.tiles[neighbour.x, neighbour.y] == 2 || tileMap.tiles[neighbour.x, neighbour.y] == 3)
                    {
                        SpawnUnit(neighbour.x, neighbour.y);
                        break;
                    }
                }
                if (i > 5)
                {

                    SpawnUnit(tileX, tileY + 1);

                }


            }

        }



    }


}
