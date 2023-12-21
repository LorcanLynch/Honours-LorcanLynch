using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ObjectiveScript : MonoBehaviour
{
    public enum objective { Assassinate, Capture, Secure, Survive };
    objective MapObjective = objective.Secure;
    GameManager gm;
    TileMap map;
     public GameObject objPanel;
     public TextMeshProUGUI objText;
    public int numOfEnemies = 1;
    public int turnsRemaining = 5;
     GameObject boss;
    public GameObject[] bosses;
    GameObject[] playerUnits = new GameObject[4] ;
    public GameObject vicPanel;
    public UpgradeScript upgradePanels;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        map = GameObject.Find("Map").GetComponent<TileMap>();
        playerUnits[0] = GameObject.Find("Huntress");
        playerUnits[1] = GameObject.Find("Knight");
        playerUnits[2] = GameObject.Find("Samurai");
        playerUnits[3] = GameObject.Find("Wizard");
        int objective = Random.Range(0, 3);
            switch(objective)
        {
            case 0:
                MapObjective = ObjectiveScript.objective.Survive;
                SurviveStart();
                break;
            case 1:
                MapObjective = ObjectiveScript.objective.Secure;
                SecureStart();
                break;
            case 2:
                MapObjective = ObjectiveScript.objective.Assassinate;
                SpawnBoss();
                break;
            case 3:
                MapObjective = ObjectiveScript.objective.Capture;
                CreateCaptureZone();
                break;
        }

        

    }


    public void StartNewLevel()
    {
        int objective = Random.Range(0, 3);
        switch (objective)
        {
            case 0:
                MapObjective = ObjectiveScript.objective.Survive;
                SurviveStart();
                break;
            case 1:
                MapObjective = ObjectiveScript.objective.Secure;
                SecureStart();
                break;
            case 2:
                MapObjective = ObjectiveScript.objective.Assassinate;
                SpawnBoss();
                break;
            case 3:
                MapObjective = ObjectiveScript.objective.Capture;
                CreateCaptureZone();
                break;
        }
        map.ClearEnemies();
    }
    private void SurviveStart()
    {
        playerUnits[1].GetComponent<UnitScript>().tileX = 9;
        playerUnits[1].GetComponent<UnitScript>().tileY = 9;
        playerUnits[1].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(9, 9);
        playerUnits[1].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(9, 9);
        playerUnits[2].GetComponent<UnitScript>().tileX = 9;
        playerUnits[2].GetComponent<UnitScript>().tileY = 10;
        playerUnits[2].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(9, 10);
        playerUnits[2].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(9, 10);
        playerUnits[3].GetComponent<UnitScript>().tileX = 10;
        playerUnits[3].GetComponent<UnitScript>().tileY = 9;
        playerUnits[3].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(10, 9);
        playerUnits[3].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(10, 9);
        playerUnits[0].GetComponent<UnitScript>().tileX = 10;
        playerUnits[0].GetComponent<UnitScript>().tileY = 10;
        playerUnits[0].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(10, 10);
        playerUnits[0].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(10, 10);
        turnsRemaining = 10;
        objText.text = "Turns Remaining: " + turnsRemaining;
        //Spawn A whole chunk of enemies at each corner
        
        int enemiesToSpawn = Random.Range(2, 5);
        for(int  i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(2, 2);
        }
        enemiesToSpawn = Random.Range(2, 4);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX - 2, map.mapSizeY - 2);
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX - 2, 2);
        }
        enemiesToSpawn = Random.Range(2, 4);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(2, map.mapSizeY - 2);
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(2 + Random.Range(-2, 2), map.mapSizeY/2 + Random.Range(-2, 2));
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX - 2 + Random.Range(-2, 2), map.mapSizeY/2 + Random.Range(-2,2));
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX / 2 + Random.Range(-2, 2), 2 + Random.Range(-2, 2));
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX/ 2 + Random.Range(-2, 2), map.mapSizeY -2 + Random.Range(-2, 2));
        }
    }

    private void SecureStart()
    {
        playerUnits[1].GetComponent<UnitScript>().tileX = 9;
        playerUnits[1].GetComponent<UnitScript>().tileY = 9;
        playerUnits[1].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(9, 9);
        playerUnits[1].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(9, 9);
        playerUnits[2].GetComponent<UnitScript>().tileX = 9;
        playerUnits[2].GetComponent<UnitScript>().tileY = 10;
        playerUnits[2].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(9, 10);
        playerUnits[2].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(9, 10);
        playerUnits[3].GetComponent<UnitScript>().tileX = 10;
        playerUnits[3].GetComponent<UnitScript>().tileY = 9;
        playerUnits[3].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(10, 9);
        playerUnits[3].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(10, 9);
        playerUnits[0].GetComponent<UnitScript>().tileX = 10;
        playerUnits[0].GetComponent<UnitScript>().tileY = 10;
        playerUnits[0].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(10, 10);
        playerUnits[0].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(10, 10);
        numOfEnemies = 15;
        turnsRemaining = 1;
        objText.text = "Enemies Remaining: " + numOfEnemies;
        int enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(2, 2);
        }
        enemiesToSpawn = Random.Range(2, 4);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX - 2, map.mapSizeY - 2);
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX - 2, 2);
        }
        enemiesToSpawn = Random.Range(2, 4);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(2, map.mapSizeY - 2);
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(2 + Random.Range(-2, 2), map.mapSizeY / 2 + Random.Range(-2, 2));
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX - 2 + Random.Range(-2, 2), map.mapSizeY / 2 + Random.Range(-2, 2));
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX / 2 + Random.Range(-2, 2), 2 + Random.Range(-2, 2));
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX / 2 + Random.Range(-2, 2), map.mapSizeY - 2 + Random.Range(-2, 2));
        }
    
    }

    public void TurnEnd()
    {
        if(MapObjective == objective.Secure)
        {
            SecureEnd();
        }
        if (MapObjective == objective.Survive)
        {
           SurviveEnd();
        }

    }

    void CreateCaptureZone()
    {

    }

    void SpawnBoss()
    {
        boss = Instantiate(bosses[0]);
        boss.GetComponent<UnitScript>().tileX = map.mapSizeX / 2;
        boss.GetComponent<UnitScript>().tileY = map.mapSizeY / 2;
        map.graph[map.mapSizeX / 2, map.mapSizeY / 2].containsUnit = true;
        playerUnits[1].GetComponent<UnitScript>().tileX = 2;
        playerUnits[1].GetComponent<UnitScript>().tileY = 2;
        playerUnits[1].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(2, 2);
        playerUnits[1].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(2, 2);
        playerUnits[2].GetComponent<UnitScript>().tileX = 1;
        playerUnits[2].GetComponent<UnitScript>().tileY = 2;
        playerUnits[2].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(1, 2);
        playerUnits[2].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(1, 2);
        playerUnits[3].GetComponent<UnitScript>().tileX = 2;
        playerUnits[3].GetComponent<UnitScript>().tileY = 1;
        playerUnits[3].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(2, 1);
        playerUnits[3].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(2, 1);
        playerUnits[0].GetComponent<UnitScript>().tileX = 1;
        playerUnits[0].GetComponent<UnitScript>().tileY = 1;
        playerUnits[0].GetComponent<UnitScript>().target = map.TileCoordToWorldCoord(1, 1);
        playerUnits[0].GetComponent<UnitScript>().transform.position = map.TileCoordToWorldCoord(1, 1);



        for (int i = 0; i< 4; i++)
        {
            gm.SpawnUnit(map.mapSizeX / 2, map.mapSizeY / 2);
        }
        
        int enemiesToSpawn = Random.Range(2, 5);
        
        enemiesToSpawn = Random.Range(2, 4);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX - 2, map.mapSizeY - 2);
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX - 2, 2);
        }
        enemiesToSpawn = Random.Range(2, 4);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(2, map.mapSizeY - 2);
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(2 + Random.Range(-2, 2), map.mapSizeY / 2 + Random.Range(-2, 2));
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX - 2 + Random.Range(-2, 2), map.mapSizeY / 2 + Random.Range(-2, 2));
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX / 2 + Random.Range(-2, 2), 2 + Random.Range(-2, 2));
        }
        enemiesToSpawn = Random.Range(2, 5);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            gm.SpawnUnit(map.mapSizeX / 2 + Random.Range(-2, 2), map.mapSizeY - 2 + Random.Range(-2, 2));
        }
    }
    void SecureEnd()
    {
        if ( map.turnCounter % 5 == 0)
        {
            int spawnLoc = Random.Range(1, 5);
            int xCorner = 0;
            int xMulti = 0;
            int yMulti = 0;
            int yCorner = 0;
            switch (spawnLoc)
            {
                case 1:
                    xCorner = map.mapSizeX;
                    yCorner = map.mapSizeY;
                    xMulti = -1;
                    yMulti = -1;
                    break;
                case 2:
                    xCorner = 0;
                    yCorner = map.mapSizeY;
                    xMulti = 1;
                    yMulti = -1;
                    break;
                case 3:
                    xCorner = 0;
                    yCorner = 0;
                    xMulti = 1;
                    yMulti = 1;
                    break;
                case 4:
                    xCorner = map.mapSizeX;
                    yCorner = 0;
                    xMulti = -1;
                    yMulti = 1;
                    break;
            }

            for (int i = 0; i < 3; i++)
            {
                int spawner = Random.Range(1, 3);
                gm.SpawnUnit(xCorner + (spawner * xMulti), yCorner + (yMulti * spawner));
            }
        }
       
    }

    public void SurviveEnd()
    {
        turnsRemaining--;
        objText.text = "Turns Remaining: " + turnsRemaining;
        if (turnsRemaining <= 0)
        {
            print("You win");
            upgradePanels.upgradesAllowed();
        }
        
    }

    public void SecureKill()
    {
        if (MapObjective ==objective.Secure)
        {
            numOfEnemies--;
            objText.text = "Enemies Remaining: " + numOfEnemies;
            if (numOfEnemies <= 0)
            {
                print("You Win!");
                vicPanel.SetActive(true);
                upgradePanels.upgradesAllowed();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
