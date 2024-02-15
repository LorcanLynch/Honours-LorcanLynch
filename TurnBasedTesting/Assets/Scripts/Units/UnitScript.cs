using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;


using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;


public class UnitScript : MonoBehaviour
{
    /// <summary>
    /// Delcares all useful variables I use within the Script, 
    /// </summary>
    public int tileY; //Due to Djikstra using Y where i would use X I just flipped the value here, so this is actually the X value of the unit, same for the variable Below
    public int tileX;
    public bool selected = false;
    public enum heroClass { knight, wizard, ranger, samurai };//This should be better done later, each should probably use their own script instead of all being in one.
    public heroClass unitClass;// Sets the unit class
    public bool attackAvailable = true;
    public TileMap map;//Link to the tilemap which essentially acts as a game manager, this allows me to reduce the ammount of looking for objects
    public List<Node> currentPath = null;
    public bool move = false;
    public Vector3 target;
    public int targetX;
    public int targetY;
    public float baseDodge;
    public int baseReduction;
    public Image[] abilityIcons = { null, null, null, null };
    public bool[] abilitiesTarget = new bool[4] { false, false, false, false };
    public int[] abilitiesCooldown = new int[4] { 0, 0, 0, 0 };
    public bool attacking;
    public Animator animator;
    public string UnitName;
    public bool stunned;
    public bool burning;
    public int burnTimer;
    public int burnDamage = 0;
    //**UNIT STATS**//

    public float maxMoveSpeed = 2;
    public float moveSpeed;
    public float maxhealth = 10;
    public float health;
    public float attackPower = 1;
    public int attackRange = 3;
    public Collider2D claveHb;
    public GameObject lightningBolt;

    public float accuracy;
    public int visibility;
    public float dodgeRating;
    public int damageReduction;
    public TextMeshProUGUI text;
    public LineRenderer lineRenderer;
    
    public float speedFloatVal = 2;

    public bool healing = false;
    public int healTimer = 0;
    public int heal = 0;

    public int buffTimer = 0   ;
    public bool buffed;
    public int dRbuff;

    public int dodgeBuffT = 0;
    public bool dodgeBuffed;
    public int dodgeBuffN= 0;

    public int damageBuffT = 0;
    public bool damageBuffed;
    public int damageBuffN = 0;
    public void Start()
    {
        //This sets all the various initial values needed for a unit
        health = maxhealth;
        map = GameObject.Find("Map").GetComponent<TileMap>();
        map.AddUnit(gameObject);
        target = map.TileCoordToWorldCoord(tileX, tileY);
        
        moveSpeed = maxMoveSpeed;
        if (animator == null) { animator = GetComponent<Animator>(); }
        baseDodge = dodgeRating;
        baseReduction = damageReduction;
        gameObject.transform.position = target;
    }

    private void OnMouseUp()
    {
        if (map.selectedUnit != null)
        {
            RaycastHit2D[] wack = Physics2D.CircleCastAll(map.selectedUnit.transform.position, (.8f * map.selectedUnit.GetComponent<UnitScript>().attackRange), new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D wacked in wack)
            {
                if (wacked.collider.gameObject.layer == 8)
                {
                    wacked.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }

        if (map.selectedUnit == null)
        {

            map.ClearIconSelection();
            map.UnitSelected(gameObject);
            return;
        }
        

            //Again a mess but it simply checks what ability the unit should use(1-4), then what ability that corelates to on the selected Unit's class and calls the
            //related function, this should definitly be its own script and **Should be** refactored later
            if (map.selectedUnit.GetComponent<UnitScript>().abilitiesTarget[0] && map.selectedUnit.GetComponent<UnitScript>().CheckAttackDistance(tileX, tileY) && map.selectedUnit.GetComponent<UnitScript>().abilitiesCooldown[0] <= 0 && map.selectedUnit.tag != gameObject.tag && map.selectedUnit.GetComponent<UnitScript>().attackAvailable)
            {

                map.selectedUnit.GetComponent<UnitScript>().Ability1(gameObject);

            }
            else if (map.selectedUnit.GetComponent<UnitScript>().abilitiesTarget[1] && map.selectedUnit.GetComponent<UnitScript>().abilitiesCooldown[1] <= 0)
            {

                map.selectedUnit.GetComponent<UnitScript>().Ability2(gameObject);
            }
            else if (map.selectedUnit.GetComponent<UnitScript>().abilitiesTarget[2] && map.selectedUnit.GetComponent<UnitScript>().abilitiesCooldown[2] <= 0)
            {

                map.selectedUnit.GetComponent<UnitScript>().Ability3(gameObject);
            }
            else if (map.selectedUnit.GetComponent<UnitScript>().attacking && gameObject.tag != map.selectedUnit.tag)
            // if the unit can attack but isnt trying to use an ability then we simply attack with their basic ability
            {
            if (map.selectedUnit.GetComponent<UnitScript>().CheckAttackDistance(tileX, tileY))
            {
                
                    map.selectedUnit.GetComponent<UnitScript>().attack(gameObject);
                
            }
            }
            else
            {
                map.UnitSelected(gameObject);
                map.ClearIconSelection();
            }

        
       
        
        
        //If the selected unit isnt trying to attack or any of the paramaters arent met like range or team then we simply select this unit instead


    }

    public virtual void attack(GameObject target)
    {
        ///<summary>
        ///Really simple attack function, first it generates a random number to hit, kind of like a lot of Table top systems might use
        ///Then we see if the number we rolled is below the units accuracy minus the target's dodge chance
        ///finally if that is true we cause the unit to take damage which is shown later
        ///</summary>
        attackAvailable = false;
        int hitChance = Random.Range(0, 100);

        animator.SetTrigger("attack");
        if (hitChance < accuracy - target.GetComponent<UnitScript>().dodgeRating)

        {
            target.GetComponent<UnitScript>().UnitDamage(attackPower);

        }
        else
        {
            text.GetComponent<DamageTextScript>().UpdateText("Miss");
        }
        attacking = false;
        
    }
    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;


    }

    public virtual void UnitDamage(float AP)
    {
        /// <summary>
        /// Causes the unit to take damage
        /// Starts by reducing it's health by the damage dealt factoring in the Unit's damage reduction, and clamping it so it cant be lower than one
        /// Then if it's health is below 0 it kills the unit by destroying it
        /// otherwise we simply play the hit animation.
        /// </summary>
        
        health -= Mathf.Clamp(AP - damageReduction, 1, 100);
        

        text.GetComponent<DamageTextScript>().UpdateText(Mathf.RoundToInt((AP - damageReduction) * -1).ToString());
        if (health <= 0)
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

    
    public void HealDamage(float AP)
    {
        /// <summary>
        /// Causes the unit to take damage
        /// Starts by reducing it's health by the damage dealt factoring in the Unit's damage reduction, and clamping it so it cant be lower than one
        /// Then if it's health is below 0 it kills the unit by destroying it
        /// otherwise we simply play the hit animation.
        /// </summary>
        health += AP;
        if (health > maxhealth)
        {
            health = maxhealth;
        }


    }
    void Update()
    {
        //First 2 if statements here are testers for final controls, start the attack and set ability one to be active.
        if (Input.GetKeyDown(KeyCode.Space) && map.selectedUnit == gameObject)
        {
            if (attacking)
            {
                RaycastHit2D[] wack = Physics2D.CircleCastAll(gameObject.transform.position, (.8f * attackRange), new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D wacked in wack)
                {
                    if (wacked.collider.gameObject.layer == 8)
                    {
                        wacked.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
            if (!attacking)
            {
                RaycastHit2D[] wack = Physics2D.CircleCastAll(map.TileCoordToWorldCoord(tileX, tileY), (.8f * attackRange), new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D wacked in wack)
                {
                    if (wacked.collider.gameObject.layer == 8)
                    {
                        wacked.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
            }
            attacking = !attacking;


        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && map.selectedUnit == gameObject)
        {
            abilitiesTarget[0] = !abilitiesTarget[0];
            abilitiesTarget[2] = false;
            abilitiesTarget[1] = false;

            map.UpdateIconSelection(0);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && map.selectedUnit == gameObject)
        {

            abilitiesTarget[0] = false;
            abilitiesTarget[2] = false;
            abilitiesTarget[1] = !abilitiesTarget[1];

            map.UpdateIconSelection(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && map.selectedUnit == gameObject)
        {

            abilitiesTarget[0] = false;
            abilitiesTarget[1] = false;
            abilitiesTarget[2] = !abilitiesTarget[2];

            map.UpdateIconSelection(2);
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

                    map.UnitMoving(gameObject);
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
                    map.UnitStopped(gameObject);
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
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target, speedFloatVal * Time.deltaTime);//We are constantly moving towards the target, this is why this should be in a coroutine


    }




    public void EnterCourse(int x, int y, List<Node> possiblePath)///<summary>
                                                                  ///Used for both the linerender to show where were moving to, and to set the movement for the update function
                                                                  /// ///</summary>

    {
        print("poss" + possiblePath.Count);
        if (!move)
        {
            float moveRemaining = moveSpeed;//Sets how much movement we have for the linerenderer
            if (targetX == x && targetY == y)//this is a janky way to check if this is the 2nd time we clikced on this tile 
            {

                currentPath = possiblePath;//Sets the path for the update
                lineRenderer.positionCount = possiblePath.Count;//thos gets rid of the linerenderer once we start moving
                for (int i = 0; i < possiblePath.Count; i++)
                {
                    lineRenderer.SetPosition(i, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 1));
                }
                animator.SetBool("moving", true);
                attacking = false;
                print("yep");

                RaycastHit2D[] wack = Physics2D.CircleCastAll(gameObject.transform.position, (.8f * attackRange), new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D wacked in wack)
                {
                    if (wacked.collider.gameObject.layer == 8)
                    {
                        wacked.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }


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
                lineRenderer.SetPosition(0, new Vector3(start.x, start.y, start.z + 2));
                for (int i = 0; i < currNode; i++)//simply iterates and draws a line to each point in the array we made
                {

                    lineRenderer.SetPosition(i + 1, new Vector3(Points[i].x, Points[i].y, Points[i].z + 2));

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

        List<Node> possiblePath = map.GenerateAttackPath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, y, x);
        if(possiblePath == null)
        {
            return true;
        }

        if (possiblePath.Count - 1 <= attackRange )
        {
            print(possiblePath.Count);
            return true;
        }
        else
        {
            print("Miss?");
            return false;
        }
       




    }
    public bool CheckAttackDistance(int x, int y,int rng)
    {
        ///<summary>
        ///Really simple function that checks if the range we need to attack across is within the unit's range
        ///</summary>

        List<Node> possiblePath = map.GenerateAttackPath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, y, x);
        if (possiblePath.Count - 1 <= rng)
        {
            return true;

        }
        else return false;




    }

    public void Burn(int length, int damage)
    {
        burning = true;
        burnTimer = length;
        burnDamage = damage;
    }

    public void HealOverTime(int length, int damage)
    {
        healing = true;
        healTimer = length;
        heal = damage;
    }

    public void DRBuff(int length, int buff)
    {
        buffed = true;
        buffTimer = length;
        damageReduction += buff;
        dRbuff = buff;
    }

    public void DodgeBuff(int length, int dodgeBuff)
    {
        if (dodgeBuff > dodgeBuffN)
        {
            dodgeRating -= dodgeBuffN;
            dodgeBuffed = true;
            dodgeBuffT = length;
            dodgeRating += dodgeBuff;
            dodgeBuffN = dodgeBuff;
        }
    }

    public void DamageBuff(int length, int dmgBuff)
    {

        if (dmgBuff > damageBuffN)
        {
            attackPower -= damageBuffN;
            damageBuffed = true;
            damageBuffT = length;
            attackPower += dmgBuff;
            damageBuffN = dmgBuff;
        }
    }



    public virtual void TurnOver()
    {
        moveSpeed = maxMoveSpeed;
        if(!stunned)
        {
            attackAvailable = true;
        }
        else
        {
            attackAvailable = false;
        }
        for (int i = 0; i < abilitiesCooldown.Length; i++)
        {
            if (abilitiesCooldown[i] > 0)
            {
                abilitiesCooldown[i]--;
            }

        }
        {
            RaycastHit2D[] tilesAffected = Physics2D.CircleCastAll(gameObject.transform.position, (.8f * attackRange), new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D tile in tilesAffected)
            {
                if (tile.collider.gameObject.layer == 8)
                {
                    tile.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
        if (healing)
        {
            HealDamage(heal);
             
            if (healTimer <= 0)
            {
                healing = false;
                heal = 0;
            }
            healTimer--;
        }

        if (dodgeBuffed)
        {
            
            
            if (dodgeBuffT <= 0)
            {
                dodgeBuffed= false;
                dodgeRating -= dodgeBuffN;
                dodgeBuffN = 0;
            }
            dodgeBuffT--;
        }

        if (damageBuffed)
        {


            if (damageBuffT <= 0)
            {
                damageBuffed = false;
                attackPower -= damageBuffN;
                damageBuffN = 0;
            }
            damageBuffT--;
        }

        if (buffed)
        {
           
            
            if (buffTimer <= 0)
            {
                buffed = false;
                damageReduction -= dRbuff ;
                dRbuff = 0;
            }
            buffTimer--;
        }

        if (burning)
        {
            UnitDamage(burnDamage + damageReduction);
            burnTimer--;
            if(burnTimer == 0)
            {
                burning = false;
            }
        }
        

    }

    public virtual void Ability1(GameObject targetUnit)
    {

    }
    public virtual void Ability2(GameObject targetUnit)
    {

    }
    public virtual void Ability3(GameObject targetUnit)
    {

    }
    public virtual void Ability4(GameObject targetUnit)
    {

    }
    public void OnMouseOver()
    {
        map.ShowStats(gameObject);
    }
}


   

   





