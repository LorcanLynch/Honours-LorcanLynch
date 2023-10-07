using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
public class TileMap : MonoBehaviour
{
    // Start is called before the first frame update
    public TileType[] tileTypes;
    public GameObject tile;
    int[,] tiles;
    List<GameObject> tileObjects;
    int mapSizeX = 10;
    int mapSizeY = 10;
    public Node[,] graph;
    float tileSize = 0.45f;
    List<Node> currentPath = null;
    public GameObject selectedUnit;
    public List<GameObject> Units;
    void Awake()
    {
        
        selectedUnit.transform.position = TileCoordToWorldCoord(selectedUnit.GetComponent<UnitScript>().tileX, selectedUnit.GetComponent<UnitScript>().tileY);
        GenerateMapVisual();
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
    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        float posX = x % 2 == 0 ? tileSize * 2 * y : tileSize * 2 * y + tileSize;

        float posY = ((tileSize * 3) / Mathf.Sqrt(3)) * x;
        return new Vector3(posX, posY, 0);
    }
    // Update is called once per frame
    public void GeneratePathTo(int x, int y, bool move)
    {
        selectedUnit.GetComponent<UnitScript>().map = this;
        currentPath = null;
        
        Dictionary<Node,float> dist = new Dictionary<Node,float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        Node source = graph[selectedUnit.GetComponent<UnitScript>().tileX,
                             selectedUnit.GetComponent<UnitScript>().tileY
                             ];

        Node target = graph[y,x];
        //if(target.containsUnit)
        
        dist[source] = 0;
        prev[source] = null;

        foreach(Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }

        while(unvisited.Count > 0)
        {
            Node u = null;
            foreach(Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u]) //&& !graph[possibleU.y, possibleU.x].containsUnit)
                {
                    u = possibleU;
                }
            }
            if(u.containsUnit)
            {
                print("Unit in location");
            }
            
            if(u == target)
            {
                
                break;
            }
            unvisited.Remove(u);
            foreach(Node v in u.connections)
            {
                float alt = dist[u] + costToEnter(v.x,v.y); 
                if( alt < dist[v] )
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
        if(move)
        {
            selectedUnit.GetComponent<UnitScript>().EnterCourse(x, y, currentPath);
        }
        
        
        


    }

    public List<Node> GenerateAttackPath(int x, int y)
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
                print("Unit in location");
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
        return tt.moveCost;
    }

    public void AddUnit(GameObject unit)
    {
        Units.Add(unit);
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = true;
    }

    public void EndTurn()
    {
        foreach (GameObject unit in Units)
        {
            unit.GetComponent<UnitScript>().TurnOver();
        }
    }

    public void UnitSelected(GameObject unit)
    {
        foreach (GameObject unit_ in Units)
        {
            unit_.GetComponent<SpriteRenderer>().color = Color.white;
        }

        unit.GetComponent<SpriteRenderer>().color = Color.red;
        selectedUnit = unit;
    }
    public void UnitMoving(GameObject unit)
    {
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = false;


    }

    public void UnitStopped(GameObject unit)
    {
        graph[unit.GetComponent<UnitScript>().tileX, unit.GetComponent<UnitScript>().tileY].containsUnit = true;

    }
}
