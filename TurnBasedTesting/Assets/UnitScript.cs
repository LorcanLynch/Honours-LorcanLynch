using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static UnityEngine.GraphicsBuffer;

public class UnitScript : MonoBehaviour
{

    public int tileY;
    public int tileX;
    public bool selected = false;
    public enum heroClass{knight,wizard,ranger,samurai };
    public heroClass unitClass;
    public bool attackAvailable = true;    
    public TileMap map;
    public List<Node> currentPath = null;
    public bool move = false;
    public Vector3 target;
    public int targetX;
    public int targetY;
    public MonoBehaviour[] abilities;
    
    public bool[] abilitiesTarget = new bool[1] { false } ;
    public bool attacking;
    public Animator animator;
    //unit stats//

    public float maxMoveSpeed = 2;
    public float moveSpeed;
    public float maxhealth = 10;
    public float health;
    public float attackPower = 1;
    public float attackRange = 3;
    public Collider2D claveHb;
    
    public float accuracy;
    public int visibility;
    public float dodgeRating;
    public int damageReduction;
    
    LineRenderer lineRenderer;
    
    private void Start()
    {
        health = maxhealth;
        map = GameObject.Find("Map").GetComponent<TileMap>();
        map.AddUnit(gameObject);
        target = map.TileCoordToWorldCoord(tileX, tileY);
        gameObject.transform.position = target;
        moveSpeed = maxMoveSpeed;
        animator = GetComponent<Animator>();
    }

    private void OnMouseUp()
    {
        if(map.selectedUnit.GetComponent<UnitScript>().attacking && map.selectedUnit.GetComponent<UnitScript>().CheckAttackDistance(tileX,tileY) && map.selectedUnit.GetComponent<UnitScript>().attackAvailable && map.selectedUnit.tag != gameObject.tag)
        {
            if ( map.selectedUnit.GetComponent<UnitScript>().abilitiesTarget[0])
            {
                switch(map.selectedUnit.GetComponent<UnitScript>().unitClass)
                {
                    case heroClass.knight:
                        map.selectedUnit.GetComponent<UnitScript>().Cleave(gameObject);
                        break;
                    case heroClass.samurai:
                        map.selectedUnit.GetComponent<UnitScript>().Execute(gameObject);
                        break;
                    case heroClass.wizard:
                        map.selectedUnit.GetComponent<UnitScript>().LightningStrike(gameObject);
                        break;
                    case heroClass.ranger:
                        map.selectedUnit.GetComponent<UnitScript>().Cleave(gameObject);
                        break;
                }
                
            }
            else
            map.selectedUnit.GetComponent<UnitScript>().attack(gameObject);
        }
        else
        map.UnitSelected(gameObject);
        
    }

    public void attack(GameObject target)
    {
        int hitChance = Random.Range(0, 100); 
        
        animator.SetTrigger("attack");
        if(hitChance < accuracy - target.GetComponent<UnitScript>().dodgeRating)

        {
            target.GetComponent<UnitScript>().UnitDamage(attackPower); 
        }
        
        attackAvailable = false;
    }
    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        

    }

    public void UnitDamage( float AP)
    {
        health -= Mathf.Clamp(AP-damageReduction,1,100);
        if(health <= 0)
        {
            animator.SetTrigger("death");
            Destroy(gameObject, 1.2f);
        }
        else
        {
            animator.SetTrigger("damage");
        }
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            attacking = !attacking;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            abilitiesTarget[0] = !abilitiesTarget[0];
        }
        //if(currentPath != null)
        //    {
        //    int currNode = 0;
        //    while(currNode < currentPath.Count -1)
        //    {
        //        Vector3 start = map.TileCoordToWorldCoord(currentPath[currNode].x,currentPath[currNode].y) + new Vector3(0,0,-2)  ;
        //        Vector3 end = map.TileCoordToWorldCoord(currentPath[currNode+1].x, currentPath[currNode+1].y ) + new Vector3(0, 0, -2);


        //        Debug.DrawLine(start, end, Color.black);

        //        currNode++;
        //    }
        //}
        


        
            if (currentPath != null)
            {
            if (gameObject.transform.position == target)
            {
                if(currentPath.Count-1 <= attackRange && currentPath[1].containsUnit)
                {//.GetComponent<UnitScript>().UnitDamage(attackPower);

                    Destroy(currentPath[1].unit);
                    currentPath = null;
                    move = false;
                    
                }
                if  (currentPath != null &&moveSpeed >= map.costToEnter(currentPath[1].x, currentPath[1].y))
                {
                    moveSpeed -= map.costToEnter(currentPath[1].x, currentPath[1].y);
                    
                    target = map.TileCoordToWorldCoord(currentPath[1].x, currentPath[1].y);

                  //  map.UnitMoving(gameObject);
                    tileX = currentPath[1].x;
                        tileY = currentPath[1].y;
                    
                    currentPath.RemoveAt(0);
                    
                        if (currentPath.Count == 1)
                        {

                            move = false;
                            currentPath = null;
                        

                    }
                    


                }
                else
                {
                    //map.UnitStopped(gameObject);
                    target = map.TileCoordToWorldCoord(tileX, tileY);
                    tileX = currentPath[0].x;
                    tileY = currentPath[0].y;
                    targetX = tileX;
                    targetY = tileY;
                    currentPath = null;
                    move = false;
                    animator.SetBool("moving", false);
                }
               


            
            


                
                
            }

          

               
            }


                

        
        if (currentPath == null && target == gameObject.transform.position)
        {
            animator.SetBool("moving", false);
        }
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target, 2 * Time.deltaTime);
        

    }

    public void EnterCourse(int x, int y,List<Node> possiblePath)
    {
        print("poss" +possiblePath.Count);
        if (!move)
        {
            float moveRemaining = moveSpeed;
            if (targetX == x && targetY == y)
            {
                move = true;
                currentPath = possiblePath;
                lineRenderer.positionCount = possiblePath.Count;
                for (int i = 0; i < possiblePath.Count; i++)
                {
                    lineRenderer.SetPosition(i, gameObject.transform.position);
                }
                animator.SetBool("moving", true);
                print("yep");

            }
            else if (currentPath == null)
            {
                
                    Vector3[] Points = new Vector3[possiblePath.Count];

                    int currNode = 0;
                    lineRenderer.positionCount = possiblePath.Count;
                    Vector3 start = map.TileCoordToWorldCoord(possiblePath[currNode].x, possiblePath[currNode].y) + new Vector3(0, 0, -2);
                while (currNode < possiblePath.Count - 1 && moveRemaining >= map.costToEnter(possiblePath[1].x, possiblePath[1].y))
                {

                    moveRemaining -= map.costToEnter(possiblePath[1].x, possiblePath[1].y);
                    Vector3 end = map.TileCoordToWorldCoord(possiblePath[currNode + 1].x, possiblePath[currNode + 1].y) + new Vector3(0, 0, -2);


                    Points[currNode] = end;

                    currNode++;
                }
                lineRenderer.positionCount = currNode + 1;
                print(currNode);
                    lineRenderer.SetPosition(0, start);
                    for (int i = 0; i < currNode; i++)
                    {
                        
                            lineRenderer.SetPosition(i + 1, Points[i]);
                        
                    }
                    print("nope");
                    targetX = x;
                    targetY = y;
            }
                
            
            
        }
        
    }


    public bool CheckAttackDistance(int x, int y)
    {
        

        List<Node> possiblePath = map.GenerateAttackPath(y, x);
        if (possiblePath.Count - 1 <= attackRange)
        {
            return true;

        }
        else return false;
        
      


    }


    //IEnumerator Moving()
    // {

    //     move = false;

    //     yield return null;
    // }

    public void TurnOver()
    {
        moveSpeed = maxMoveSpeed;
        attackAvailable = true;
    }

   
    public void Cleave(GameObject targetUnit)
    {
        //int enemyX = targetUnit.GetComponent<UnitScript>().tileX;
        //int enemyY = targetUnit.GetComponent<UnitScript>().tileY;
        targetUnit.GetComponent<UnitScript>().UnitDamage(100);
       RaycastHit2D[] wack = Physics2D.CircleCastAll(gameObject.transform.position, 2, new Vector2(0, 0));
        foreach(RaycastHit2D wacked in wack)
        {
            wacked.collider.GetComponent<UnitScript>().UnitDamage(10);
        }
        //if (enemyX > tileX && enemyY > tileY)
        //{
        //    print(enemyX-- & enemyY);
        //    print(enemyX + enemyY--);
        //    if (map.graph[enemyX--, enemyY].unit != null) { map.graph[enemyX--, enemyY].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //    if (map.graph[enemyX, enemyY--].unit != null) { map.graph[enemyX, enemyY--].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //}
        //else if(enemyX > tileX && enemyY < tileY)
        //{
        //    print("int x" + enemyX-- + " int y" + enemyY);
        //    print("int x" + enemyX + " int y" + enemyY++);
        //    if (map.graph[enemyX--, enemyY].unit != null) { map.graph[enemyX--, enemyY].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //    if (map.graph[enemyX, enemyY++].unit != null) { map.graph[enemyX, enemyY++].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //}
        //else if (enemyX > tileX && enemyY == tileY)
        //{
        //    print("int x" + enemyX + " int y" + enemyY--);
        //    print("int x" + enemyX + " int y" + enemyY++);
        //    if (map.graph[enemyX, enemyY--].unit != null) { map.graph[enemyX, enemyY--].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //    if (map.graph[enemyX, enemyY++].unit != null) { map.graph[enemyX, enemyY++].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //}
        //else if (enemyX < tileX && enemyY == tileY)
        //{
        //    print(enemyX++ + enemyY--);
        //    print(enemyX++ + enemyY++);
        //    if (map.graph[enemyX++, enemyY--].unit != null) { map.graph[enemyX++, enemyY--].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //    if (map.graph[enemyX++, enemyY++].unit != null) { map.graph[enemyX++, enemyY++].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //}
        //else if (enemyX == tileX && enemyY > tileY)
        //{
        //    print(enemyX++ + enemyY);
        //    print(enemyX-- + enemyY--);
        //    if (map.graph[enemyX++, enemyY].unit != null) { map.graph[enemyX++, enemyY].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //    if (map.graph[enemyX--, enemyY--].unit != null) { map.graph[enemyX--, enemyY--].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //}
        //else if (enemyX == tileX && enemyY < tileY)
        //{
        //    print(enemyX-- + enemyY++);
        //    print(enemyX++ + enemyY);
        //    if (map.graph[enemyX--, enemyY++].unit != null) { map.graph[enemyX--, enemyY++].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //    if (map.graph[enemyX++, enemyY].unit != null) { map.graph[enemyX++, enemyY].unit.GetComponent<UnitScript>().UnitDamage(5); }
        //}
    }

    void Execute(GameObject targetUnit)
    {
        if(targetUnit.GetComponent<UnitScript>().health < targetUnit.GetComponent<UnitScript>().maxhealth /2)
        {
            animator.SetTrigger("attack");
            targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower * 2);
        }
        else {
            int hitChance = Random.Range(0, 100);

            animator.SetTrigger("attack");
            if (hitChance < accuracy - targetUnit.GetComponent<UnitScript>().dodgeRating)

            {
                targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);
            }
        }
    
        
    }

    void LightningStrike(GameObject targetUnit)
    {
        //RaycastHit2D[] unitsHit = Physics2D.LinecastAll(gameObject.transform.position, targetUnit.transform.position);
        RaycastHit2D[] unitsHit = Physics2D.BoxCastAll(gameObject.transform.position, new Vector2(10, 10), 0, new Vector2(gameObject.transform.position.x - targetUnit.transform.position.x+1, gameObject.transform.position.y - targetUnit.transform.position.y+1));
        foreach(RaycastHit2D hit in unitsHit)
        {
            hit.collider.GetComponent<UnitScript>().UnitDamage(100);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(gameObject.transform.position, new Vector3(1, 1, -1));

    }
}
