using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TileMap : MonoBehaviour
{
    // Start is called before the first frame update
    public TileType[] tileTypes;
    public GameObject tile;
    public int[,] tiles;
    List<GameObject> tileObjects;
   public int mapSizeX = 10;
    public int mapSizeY = 10;
    public Node[,] graph;
    float tileSize = 0.45f;
    List<Node> currentPath = null;
    public GameObject selectedUnit;
    public List<GameObject> Units;
    public bool canSelect = true;
    public GameObject panelEnd;
    public GameObject panelBackdrop;
    private bool targetting;
    float wack = 0;
    public int turnCounter = 0;
   public GameManager gm;
    public int numOfEnemies;
    public ObjectiveScript obj;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI dodgeText;
    public TextMeshProUGUI armourText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI rangeText;
    public List<GameObject> activeTiles;
    public enum mapType {Fort, Swamp, Desert, Hills, Camp }
    void Awake()
    {
        obj = GetComponent<ObjectiveScript>();
        selectedUnit.transform.position = TileCoordToWorldCoord(selectedUnit.GetComponent<UnitScript>().tileX, selectedUnit.GetComponent<UnitScript>().tileY);
        //GenerateFort();
        ChooseMap();
        GeneratePathfindingGraph();
        GenerateMap();
     
        
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
    }

    public void ChooseMap()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("tile");
        for(int i = 0; i < tiles.Length; i++)
        {
            Destroy(tiles[i]);
        }
        int j = Random.Range(0, 2);
        if(j == 0)
        {
            GenerateFort();

        }
        else
        {
            GenerateGrove();
        }

        GeneratePathfindingGraph();
        GenerateMap();

    }

    private void Update()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y].containsUnit = false;
            }
        }
        foreach (GameObject unit in Units)
        {
            //unit.GetComponent<UnitScript>().
            
            graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = true;
        }
    }

    public void ClearEnemies()
    {
        foreach(GameObject unit in Units)
        {
            if (unit.tag == "Enemy")
            {
                Destroy(unit);
            }
        }
        Units.Clear();
       Units.Add(GameObject.Find("Huntress"));
        Units.Add(GameObject.Find("Knight"));
        Units.Add(GameObject.Find("Wizard"));
        Units.Add(GameObject.Find("Samurai"));
    }
    void GenerateMap()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];
                float posX = x % 2 == 0 ? tileSize * 2 * y : tileSize * 2 * y + tileSize;

                float posY = ((tileSize * 3) / Mathf.Sqrt(3)) * x;
                GameObject gO = Instantiate(tt.tileVisual, new Vector3(posX, posY, 1), Quaternion.identity);

                ClickableTile ct = gO.GetComponent<ClickableTile>();
              
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
                
            }
        }

    }

    public GameObject GenerateSecureZone(int x,int y)
    {
        
        tiles[x, y] =4;
        TileType tt = tileTypes[tiles[x,y]];
        float posX = x % 2 == 0 ? tileSize * 2 * y : tileSize * 2 * y + tileSize;

        float posY = ((tileSize * 3) / Mathf.Sqrt(3)) * x;
        GameObject gO = Instantiate(tt.tileVisual, new Vector3(posX, posY, .99f), Quaternion.identity);
        ClickableTile ct = gO.GetComponent<ClickableTile>();
        ct.tileX = x;
        ct.tileY = y;
        ct.map = this;
        return gO;
    }
    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        float posX = x % 2 == 0 ? tileSize * 2 * y : tileSize * 2 * y + tileSize;

        float posY = ((tileSize * 3) / Mathf.Sqrt(3)) * x;
        return new Vector3(posX, posY, 0);
    }
    // Update is called once per frame
    public void GeneratePathTo(int x, int y, bool move)
    {
        if (selectedUnit != null)
        {
            selectedUnit.GetComponent<UnitScript>().map = this;
            currentPath = null;

            Dictionary<Node, float> dist = new Dictionary<Node, float>();
            Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

            List<Node> unvisited = new List<Node>();

            Node source = graph[selectedUnit.GetComponent<UnitScript>().tileX,
                                 selectedUnit.GetComponent<UnitScript>().tileY
                                 ];

            Node target = graph[y, x];
            //if(target.containsUnit)

            dist[source] = 0;
            prev[source] = null;

            foreach (Node v in graph)
            {
                if (v != source)
                {
                    dist[v] = Mathf.Infinity;
                    prev[v] = null;
                }
                unvisited.Add(v);
            }

            while (unvisited.Count > 0)
            {
                Node u = null;
                foreach (Node possibleU in unvisited)
                {
                    if (u == null || dist[possibleU] < dist[u]) //&& !graph[possibleU.y, possibleU.x].containsUnit)
                    {

                        u = possibleU;
                    }
                }
                if (u.containsUnit)
                {
                    unvisited.Remove(u);
                }

                if (u == target)
                {

                    break;
                }
                unvisited.Remove(u);
                foreach (Node v in u.connections)
                {
                    float alt = dist[u] + costToEnter(v.x, v.y);
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }

            if (prev[target] == null)
            {
                return;
            }
            currentPath = new List<Node>();
            Node curr = target;
            while (curr != null)
            {
                currentPath.Add(curr);
                curr = prev[curr];
            }
            currentPath.Reverse();
            if (move)
            {
                selectedUnit.GetComponent<UnitScript>().EnterCourse(x, y, currentPath);
            }
        }
        
        


    }

    public List<Node> GenerateMovePath(GameObject unitRequesting,int startX, int startY,int x, int y)
    {
        unitRequesting.GetComponent<UnitScript>().map = this;
        currentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        Node source = graph[startX,
                             startY
                             ];

        Node target = graph[y, x];
        //if(target.containsUnit)

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            Node u = null;
            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u]) //&& !graph[possibleU.y, possibleU.x].containsUnit)
                {
                    u = possibleU;
                }
            }
            

            if (u == target)
            {

                break;
            }
            unvisited.Remove(u);
            foreach (Node v in u.connections)
            {
                float alt = dist[u] + costToEnter(v.x, v.y);
                if ((alt < dist[v]))
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }
        
        if (prev[target] == null)
        {
            return null;
        }
        currentPath = new List<Node>();
        Node curr = target;
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }
        if (currentPath[0].containsUnit)
        {
            currentPath.RemoveAt(0);
            if (currentPath.Count == 1)
            { 

                    return null;
             }
        }
        currentPath.Reverse();
       
           return currentPath;
       









    }

    public List<Node> GenerateAttackPath(GameObject unitRequesting, int startX, int startY, int x, int y)
    {
        unitRequesting.GetComponent<UnitScript>().map = this;
        currentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        Node source = graph[startX,
                             startY
                             ];

        Node target = graph[y, x];
        //if(target.containsUnit)

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            Node u = null;
            foreach (Node possibleU in unvisited)
            {
                if ((u == null || dist[possibleU] < dist[u]) ) //&& !graph[possibleU.y, possibleU.x].containsUnit)
                {
                    u = possibleU;
                }
            }
            if (u.containsUnit)
            {
                print("Unit in location");
            }

            if (u == target)
            {

                break;
            }
            unvisited.Remove(u);
            foreach (Node v in u.connections)
            {
                float alt = dist[u] + 1;
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        if (prev[target] == null)
        {
            return null;
        }
        currentPath = new List<Node>();
        Node curr = target;
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }
        
        currentPath.Reverse();

        return currentPath;










    }
        public void GenerateMapVisual()
    {
        tiles = new int[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }

        for (int i = 4; i < 8; i++)
        {
            for (int j = 4; j < 7; j++)
            {
                tiles[i, j] = 2;
            }
        }

        for (int i = 1; i < 3; i++)
        {
            for (int j = 1; j < 3; j++)
            {
                tiles[i, j] = 1;
            }
        }
    }
    
   
    
    void GeneratePathfindingGraph()
    {
        graph = new Node[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                
                if (x % 2 == 0)
                {
                    if (x > 0)
                    {
                        graph[x, y].connections.Add(graph[x - 1, y]);
                    }
                    if (x < mapSizeX - 1)
                    {
                        graph[x, y].connections.Add(graph[x + 1, y]);
                    }
                    if (y > 0)
                    {
                        graph[x, y].connections.Add(graph[x, y - 1]);
                    }
                    if (y < mapSizeY - 1)
                    {
                        graph[x, y].connections.Add(graph[x, y + 1]);
                    }
                    if (x < mapSizeX - 1 && y > 0)
                    {
                        graph[x, y].connections.Add(graph[x + 1, y - 1]);
                    }
                    if (x  > 0 && y >0)
                    {
                        graph[x, y].connections.Add(graph[x - 1, y - 1]);
                    }
                    
                }
                else
                {
                    if (x > 0)
                    {
                        graph[x, y].connections.Add(graph[x - 1, y]);
                    }
                    if (x < mapSizeX - 1)
                    {
                        graph[x, y].connections.Add(graph[x + 1, y]);
                    }
                    if (y > 0)
                    {
                        graph[x, y].connections.Add(graph[x, y - 1]);
                    }
                    if (y < mapSizeY - 1)
                    {
                        graph[x, y].connections.Add(graph[x, y + 1]);
                    }
                    if (x > 0&& y < mapSizeY - 1)
                    {
                        graph[x, y].connections.Add(graph[x - 1, y + 1]);
                    }
                    if (x < mapSizeX - 1 && y < mapSizeY - 1)
                    {
                        graph[x, y].connections.Add(graph[x + 1, y + 1]);
                    }
                }
            }
        }
    }

   public float costToEnter(int x, int y)
    {
        TileType tt = tileTypes[ tiles[x, y]];
        if(graph[x, y].containsUnit)
        {
            return 99;
        }
        return tt.moveCost;
    }

    public void AddUnit(GameObject unit)
    {
        Units.Add(unit);
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = true;
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].unit = unit;
    }
    public void RemoveUnit(GameObject unit)
    {
        Units.Remove(unit);
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = false;
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].unit = null;
        
            obj.SecureKill();
        
        
        
    }
    public void EndTurn()
    {
        selectedUnit = null;
        canSelect = false;
        foreach (GameObject unit_ in Units)
        {
            unit_.GetComponent<SpriteRenderer>().color = Color.white;
        }
        obj.TurnEnd();
            StartCoroutine("EndTurnA");
        turnCounter++;
        
        
    }

    public void GenerateFort()
    {
        tiles = new int[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }

        for (int i = 3; i < 10; i++)
        {
            for (int j = 3; j < 10; j++)
            {
                tiles[i, j] = 2;
            }
        }
        for (int i = 4; i < 9; i++)
        {
            for (int j = 11; j < 16; j++)
            {
                tiles[i, j] = 3;
            }
        }

        for (int i = 5; i < 15; i++)
        {
            for (int j =5; j < 15; j++)
            {
                tiles[i, j] = 1;
            }
        }
        for (int i = 7; i < 13; i++)
        {
            for (int j = 7; j < 13; j++)
            {
                
                tiles[i, j] = 0;
                
            }
        }

        for (int i = 3; i < 15; i++)
        {
            for (int j = 9; j < 11; j++)
            {
                tiles[i, j] = 0;
                if (i < 11&& j == 10)
                    tiles[i, j] = 3;
                
            }
        }
        for (int i = 9; i < 11; i++)
        {
            for (int j = 5; j < 15; j++)
            {
                tiles[i, j] = 0;
                if(j>11 && i == 9)
                    tiles[i, j] = 3;
            }
        }
        for (int i = 13; i < 18; i++)
        {
            for (int j = 3; j < 7; j++)
            {
                tiles[i, j] = 1;
            }

        }
        for (int i = 7; i < 9; i++)
        {
            for (int j = 11; j < 13; j++)
            {
                tiles[i, j] = 3;
            }
        }





    }

    public void GenerateGrove()
    {
        tiles = new int[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }



        for (int x = 15; x < 16; x++)
            for (int y = 2; y < 9; y++)
            {
                tiles[x, y] = 3;
            }

        for (int x = 6; x < 12; x++)
            for (int y = 13; y < 19; y++)
            {
                tiles[x, y] = 2;
            }

        for (int x = 11; x < 20; x++)
            for (int y = 16; y < 18; y++)
            {
                tiles[x, y] = 1;
            }


        for (int x = 7; x < 11; x++)
            for (int y = 14; y < 18; y++)
            {
                tiles[x, y] = 1;
            }

       


        for (int x =9; x < 12; x++)
            for (int y = 14; y < 18; y++)
            {
                tiles[x, y] = 1;
            }
        for (int x = 16; x < 17; x++)
            for (int y = 16; y < 18; y++)
            {
                tiles[x, y] = 2;
            }

        for (int x = 0; x < 20; x++)
            for (int y = 12; y < 14; y++)
            {
                tiles[x, y] = 3;
            }
        for (int x = 0; x < 15; x++)
            for (int y = 2; y < 9; y++)
            {
                tiles[x, y] = 3;
            }
        for (int x = 2; x < 4; x++)
            for (int y = 0; y < 20; y++)
            {
                tiles[x, y] = 0;
            }
        for (int x = 6; x < 12; x++)
            for (int y = 2; y < 6; y++)
            {
                tiles[x, y] = 1;
            }
        tiles[12, 3] = 1;
        tiles[12, 4] = 1;
        tiles[13, 3] = 1;

       





    }

    public void GenerateSwamp()
    {
        tiles = new int[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }



        for (int x = 15; x < 16; x++)
            for (int y = 2; y < 9; y++)
            {
                tiles[x, y] = 3;
            }

        for (int x = 6; x < 12; x++)
            for (int y = 13; y < 19; y++)
            {
                tiles[x, y] = 2;
            }

        for (int x = 11; x < 20; x++)
            for (int y = 16; y < 18; y++)
            {
                tiles[x, y] = 1;
            }


        for (int x = 7; x < 11; x++)
            for (int y = 14; y < 18; y++)
            {
                tiles[x, y] = 1;
            }




        for (int x = 9; x < 12; x++)
            for (int y = 14; y < 18; y++)
            {
                tiles[x, y] = 1;
            }
        for (int x = 16; x < 17; x++)
            for (int y = 16; y < 18; y++)
            {
                tiles[x, y] = 2;
            }

        for (int x = 0; x < 20; x++)
            for (int y = 12; y < 14; y++)
            {
                tiles[x, y] = 3;
            }
        for (int x = 0; x < 15; x++)
            for (int y = 2; y < 9; y++)
            {
                tiles[x, y] = 3;
            }
        for (int x = 2; x < 4; x++)
            for (int y = 0; y < 20; y++)
            {
                tiles[x, y] = 0;
            }
        for (int x = 6; x < 12; x++)
            for (int y = 2; y < 6; y++)
            {
                tiles[x, y] = 1;
            }
        tiles[12, 3] = 1;
        tiles[12, 4] = 1;
        tiles[13, 3] = 1;







    }

    public IEnumerator EndTurnA()
    {
        panelEnd.SetActive(true);
        foreach (GameObject unit in Units)
        {
            unit.GetComponent<UnitScript>().TurnOver();
            
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyScript>().turnStart();
                yield return new WaitUntil(() => enemy.GetComponent<UnitScript>().currentPath == null);
                enemy.GetComponent<EnemyScript>().FinishedMove();
            }
           
        }
        canSelect = true;
        panelEnd.SetActive(false);
        yield return null;
    }
    
    public void UnitSelected(GameObject unit)
    {
        if (canSelect && unit.tag!= "Enemy")
        {
            foreach (GameObject unit_ in Units)
            {
                unit_.GetComponent<SpriteRenderer>().color = Color.white;
                for (int i = 0 ; i < unit_.GetComponent<UnitScript>().abilitiesTarget.Length; i++)
                {
                    
                    unit_.GetComponent<UnitScript>().abilitiesTarget[i] = false;
                }
              

            }
            panelBackdrop.GetComponent<BackDropScript>().newUnit(unit.GetComponent<UnitScript>().abilityIcons, unit.GetComponent<UnitScript>().abilitiesCooldown);
            unit.GetComponent<SpriteRenderer>().color = Color.red;
            selectedUnit = unit;
        }
    }
    public void UpdateCooldowns(GameObject unit)
    {
        panelBackdrop.GetComponent<BackDropScript>().newUnit(unit.GetComponent<UnitScript>().abilityIcons, unit.GetComponent<UnitScript>().abilitiesCooldown) ;
    }

    public void UpdateIconSelection(int index) 
    {
        panelBackdrop.GetComponent<BackDropScript>().IconSelection(index);
    }   
    public void ClearIconSelection()
    {
        panelBackdrop.GetComponent<BackDropScript>().ClearSelection();
    }
    public void UnitMoving(GameObject unit)
    {
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = false;


    }

    public void UnitStopped(GameObject unit)
    {
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = true;

    }   

    public void ShowStats(GameObject target)
    {
        nameText.text = target.GetComponent<UnitScript>().UnitName;
        healthText.text = "Health: " +target.GetComponent<UnitScript>().health.ToString() + "/" + target.GetComponent<UnitScript>().maxhealth.ToString();
        dodgeText.text = "Evasion: " + target.GetComponent<UnitScript>().dodgeRating.ToString();
        armourText.text = "Armour: " + target.GetComponent<UnitScript>().damageReduction.ToString();
        accuracyText.text = "Accuracy: " + target.GetComponent<UnitScript>().accuracy.ToString();
        rangeText.text = "Range: " + target.GetComponent<UnitScript>().attackRange;


    }
    //public List<Node> aAlgothRecreate(Node prev, Node curr)
    //{

    //}
    //public List<Node> aAlgothe(Node curr, Node goal, int estCost)
    //{
    //    List<Node> PossiblePath = new List<Node>();
    //    PossiblePath.Add(graph[curr.x, curr.y]);
    //    Node cameFrom = null;
    //    float costToMove = Mathf.Infinity;
    //    ;
    //    List<float> gScore = new List<float>();
    //    gScore.Add(0);

    //    float fScore = Mathf.Infinity;

    //    List<float> fScoreL = new List<float>();
    //    fScoreL.Add(costToEnter(curr.x, curr.y));

    //    while (PossiblePath.Count != 0)
    //    {
    //        Node currentNode = null;
    //        foreach(Node currNode in PossiblePath)
    //        {
    //            if(currentNode == null)
    //            {
    //                currentNode = currNode;
    //            }
    //            if(costToEnter(currentNode.x, currentNode.y) > costToEnter(currNode.x,currNode.y))
    //            {
    //                currentNode = currNode;
    //            }
    //        }

    //        if(currentNode == goal)
    //        {
    //           return aAlgothRecreate()
    //        }

    //        PossiblePath.Remove(currentNode);   
    //        foreach (Node neighbour in currentNode.connections)
    //        {
    //            float tenativeScore = gScore[costToEnter(currentNode.x, currentNode.y])] + costToEnter(neighbour.x,neighbour.y)
    //            if(tenativeScore < 
    //        }


    //    }



    //}
}
