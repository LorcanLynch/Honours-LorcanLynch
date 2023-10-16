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
    /// <summary>
    /// Delcares all useful variables I use within the Script, 
    /// </summary>
    public int tileY; //Due to Djikstra using Y where i would use X I just flipped the value here, so this is actually the X value of the unit, same for the variable Below
    public int tileX;
    public bool selected = false;
    public enum heroClass{knight,wizard,ranger,samurai };//This should be better done later, each should probably use their own script instead of all being in one.
    public heroClass unitClass;// Sets the unit class
    public bool attackAvailable = true;    
    public TileMap map;//Link to the tilemap which essentially acts as a game manager, this allows me to reduce the ammount of looking for objects
    public List<Node> currentPath = null;
    public bool move = false;
    public Vector3 target;
    public int targetX;
    public int targetY;
    
    
    public bool[] abilitiesTarget = new bool[1] { false } ;
    public bool attacking;
    public Animator animator;


    //**UNIT STATS**//

    public float maxMoveSpeed = 2;
    public float moveSpeed;
    public float maxhealth = 10;
    public float health;
    public float attackPower = 1;
    public float attackRange = 3;
    public Collider2D claveHb;
    public GameObject lightningBolt;

    public float accuracy;
    public int visibility;
    public float dodgeRating;
    public int damageReduction;
    
    LineRenderer lineRenderer;
    
    private void Start()
    {
        //This sets all the various initial values needed for a unit
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
            //This looks really ugly and needs refacrtored. but it checks first if a unit is trying to attack this one, then if it is in range,
            //next if that unit can attack and finally if the unit is on the same team
        {
            //Again a mess but it simply checks what ability the unit should use(1-4), then what ability that corelates to on the selected Unit's class and calls the
            //related function, this should definitly be its own script and **Should be** refactored later
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
                        map.selectedUnit.GetComponent<UnitScript>().PiercingShot(gameObject);
                        break;
                }
                
            }
            else
             // if the unit can attack but isnt trying to use an ability then we simply attack with their basic ability
            map.selectedUnit.GetComponent<UnitScript>().attack(gameObject);
        }
        else
            //If the selected unit isnt trying to attack or any of the paramaters arent met like range or team then we simply select this unit instead
        map.UnitSelected(gameObject);
        
    }

    public void attack(GameObject target)
    {
        ///<summary>
        ///Really simple attack function, first it generates a random number to hit, kind of like a lot of Table top systems might use
        ///Then we see if the number we rolled is below the units accuracy minus the target's dodge chance
        ///finally if that is true we cause the unit to take damage which is shown later
        ///</summary>
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
        /// <summary>
        /// Causes the unit to take damage
        /// Starts by reducing it's health by the damage dealt factoring in the Unit's damage reduction, and clamping it so it cant be lower than one
        /// Then if it's health is below 0 it kills the unit by destroying it
        /// otherwise we simply play the hit animation.
        /// </summary>
        health -= Mathf.Clamp(AP-damageReduction,1,100);
        if(health <= 0)
        {
            animator.SetTrigger("death");
            map.GetComponent<TileMap>().RemoveUnit(gameObject);
            Destroy(gameObject, 1.2f);
            
        }
        else
        {
            
            animator.SetTrigger("damage");
        }
        
    }
    void Update()
    {
        //First 2 if statements here are testers for final controls, start the attack and set ability one to be active.
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



        //

        //Rudimentary version of the movement script, this is really innefecient but it does the job for now, preferably this  should be moved to a coroutine
        if (currentPath != null)// Checks if there is a path to follow 
        {
            if (gameObject.transform.position == target)
            {

                if (currentPath != null && moveSpeed >= map.costToEnter(currentPath[1].x, currentPath[1].y))// We check if we have enough movement speed to move into the next tile
                {
                    moveSpeed -= map.costToEnter(currentPath[1].x, currentPath[1].y);//if we do we reduce our movement speed by the cost to enter the next tile

                    target = map.TileCoordToWorldCoord(currentPath[1].x, currentPath[1].y);// We set our target to the next tile

                    //  map.UnitMoving(gameObject);
                    tileX = currentPath[1].x;//once were moving we set our current tile to the target tile
                    tileY = currentPath[1].y;

                    currentPath.RemoveAt(0);//we remove the current tile from the path

                    if (currentPath.Count == 1)//Once the path is finished we set the path to null and stop moving
                    {

                        move = false;
                        currentPath = null;


                    }



                }
                else// if we dont have enough movement or the path is empty we clear out the relevant data
                {
                    //map.UnitStopped(gameObject);
                    target = map.TileCoordToWorldCoord(tileX, tileY);//set our target to the current tile
                    tileX = currentPath[0].x;//we set our current tile to this tile
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
            animator.SetBool("moving", false);// simply stops the moving animation once we stop

        }
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target, 2 * Time.deltaTime);//We are constantly moving towards the target, this is why this should be in a coroutine


    }

    
    

    public void EnterCourse(int x, int y,List<Node> possiblePath)///<summary>
        ///Used for both the linerender to show where were moving to, and to set the movement for the update function
        /// ///</summary>
        
    {
        print("poss" +possiblePath.Count);
        if (!move)
        {
            float moveRemaining = moveSpeed;//Sets how much movement we have for the linerenderer
            if (targetX == x && targetY == y)//this is a janky way to check if this is the 2nd time we clikced on this tile 
            {
                
                currentPath = possiblePath;//Sets the path for the update
                lineRenderer.positionCount = possiblePath.Count;//thos gets rid of the linerenderer once we start moving
                for (int i = 0; i < possiblePath.Count; i++)
                {
                    lineRenderer.SetPosition(i, gameObject.transform.position);
                }
                animator.SetBool("moving", true);
                print("yep");

            }
            else if (currentPath == null)//We check if this is the first time we clicked this tile
            {
                
                    Vector3[] Points = new Vector3[possiblePath.Count];//Sets the points to equal the path
                print(Points.Length);
                    int currNode = 0;//used to iterate the nodes 
                    lineRenderer.positionCount = possiblePath.Count;//sets the length of the renderer 
                    Vector3 start = map.TileCoordToWorldCoord(possiblePath[currNode].x, possiblePath[currNode].y) + new Vector3(0, 0, -2);//sets the start point to begin at the unit
                while (currNode < possiblePath.Count - 1 && moveRemaining >= map.costToEnter(possiblePath[1].x, possiblePath[1].y))//checks if we have movement and path left 
                {

                    moveRemaining -= map.costToEnter(possiblePath[1].x, possiblePath[1].y);//Reduces the renderer movement
                    Vector3 end = map.TileCoordToWorldCoord(possiblePath[currNode + 1].x, possiblePath[currNode + 1].y) + new Vector3(0, 0, -2);//sets the next point, end here is simply the next point in the ine


                    Points[currNode] = end;//adds it to the vector

                    currNode++;
                }
                lineRenderer.positionCount = currNode + 1;
                print(currNode);
                    lineRenderer.SetPosition(0, start);
                    for (int i = 0; i < currNode; i++)//simply iterates and draws a line to each point in the array we made
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
        ///<summary>
        ///Really simple function that checks if the range we need to attack across is within the unit's range
        ///</summary>

        List<Node> possiblePath = map.GenerateAttackPath(gameObject,gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, y, x);
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
        attackAvailable = true;//just resets our turn based actions
    }

   
    public void Cleave(GameObject targetUnit)
    {
        ///<summary>
        ///Allows the knight to use it's cleave abiltiy
        /// </summary>
        
        targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);//simple attack
        RaycastHit2D[] wack = Physics2D.CircleCastAll(gameObject.transform.position, 2, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
        foreach(RaycastHit2D wacked in wack)
        {
            wacked.collider.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower /3));
        }

        //int enemyX = targetUnit.GetComponent<UnitScript>().tileX;
        //int enemyY = targetUnit.GetComponent<UnitScript>().tileY;
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
        ///<summary>
        ///Allows the Samurai to use execute
        ///</summary>
        if(targetUnit.GetComponent<UnitScript>().health < targetUnit.GetComponent<UnitScript>().maxhealth /2)
        {
            int hitChance = Random.Range(0, 100);//Does a normal attack, then if the target's health is lower than half it's max then it does extra damage, otherwise it does normal damage
            animator.SetTrigger("attack");
            if (hitChance < accuracy - targetUnit.GetComponent<UnitScript>().dodgeRating)

            {
                targetUnit.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower * 1.5f));
            }
           
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
        ///<summary>
        ///This is a little out of system but checking each hex in a line seemed like it would take to much processing work so i simple instantiate an object with damages each unit it touches
        /// </summary>
        //RaycastHit2D[] unitsHit = Physics2D.LinecastAll(gameObject.transform.position, targetUnit.transform.position);
        GameObject bolt = Instantiate(lightningBolt,  Vector3.MoveTowards( gameObject.transform.position,targetUnit.transform.position,.4f), Quaternion.FromToRotation(transform.position, -targetUnit.transform.position));
        bolt.GetComponent<LightningBoltScript>().target = targetUnit.transform.position;
        
    }

    void PiercingShot(GameObject targetUnit)
    {
        ///<summary>
        ///Simple attack that simply ignores a portion of the target unit's armour
        /// </summary>
        int hitChance = Random.Range(0, 100);

        animator.SetTrigger("attack");
        if (hitChance < accuracy - targetUnit.GetComponent<UnitScript>().dodgeRating)

        {
            targetUnit.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower + (targetUnit.GetComponent<UnitScript>().damageReduction* .75f)));
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(gameObject.transform.position, new Vector3(1, 1, -1));

    }
}
