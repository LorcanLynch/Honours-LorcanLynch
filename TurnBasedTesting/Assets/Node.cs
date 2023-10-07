using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    // Start is called before the first frame update
    public class Node
    {
        public bool containsUnit;
        public List<Node> connections;
        public GameObject unit;
        public int x;
        public int y;
        public Node()
        {
            connections = new List<Node>();
        }
        public float DistanceTo(Node n)
        {
            return Vector2.Distance(new Vector2(x, y), new Vector2(n.x, n.y));
        }
    }

