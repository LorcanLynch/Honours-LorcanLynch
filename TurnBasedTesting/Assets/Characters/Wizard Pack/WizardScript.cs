using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WizardScript : UnitScript
{
    public bool fireball;
    public bool lifeDrain;
    public bool incantationOfPower;
    public bool greaterInvocation;
    public bool PowerSurge;
    public bool Bombardment;
    public bool massBombardment;
    public bool soulDrain;
  
    public int drainedTimer;

    public bool lightningSurgeCombo;
    public bool lightningSurgeComboA;


    public bool fireLord;
    public bool iceLord;
    public bool breathOfCold = false;

    public bool[] combos = new bool[3];
    public bool[] combosA = new bool[3];
    public override void attack(GameObject target)
    {

        ///<summary>
        ///Really simple attack function, first it generates a random number to hit, kind of like a lot of Table top systems might use
        ///Then we see if the number we rolled is below the units accuracy minus the target's dodge chance
        ///finally if that is true we cause the unit to take damage which is shown later
        ///</summary>
        if (attackAvailable)
        {
            int hitChance = Random.Range(0, 100);

            animator.SetTrigger("attack");
            if (hitChance < accuracy - target.GetComponent<UnitScript>().dodgeRating)
            {
                target.GetComponent<UnitScript>().UnitDamage(attackPower);
                if (fireLord)
                {
                    target.GetComponent<UnitScript>().Burn(4, 2);
                }
                if (iceLord)
                {
                    int stunChance = Random.Range(0, 100);
                    if (stunChance > 25)
                    {
                        target.GetComponent<UnitScript>().stunned = true;
                    }
                }


               
              
            }
            attackAvailable = false;
            attacking = false;
        }

        
    }
    public override void Ability1(GameObject targetUnit)
    {
        ///<summary>
        ///This is a little out of system but checking each hex in a line seemed like it would take to much processing work so i simple instantiate an object with damages each unit it touches
        /// </summary>
        //RaycastHit2D[] unitsHit = Physics2D.LinecastAll(gameObject.transform.position, targetUnit.transform.position);
        GameObject bolt = Instantiate(lightningBolt, Vector3.MoveTowards(gameObject.transform.position, targetUnit.transform.position, .4f), Quaternion.FromToRotation(transform.position, -targetUnit.transform.position));
        bolt.GetComponent<LightningBoltScript>().target = targetUnit.transform.position;
        if(lightningSurgeComboA)
        {
            bolt.GetComponent<LightningBoltScript>().damage += 4;
            lightningSurgeComboA = false;
        }
        else
        {
            if(lightningSurgeCombo)
            {
                lightningSurgeComboA = true;
            }
        }
        abilitiesCooldown[0] = 4;
        attackAvailable = false;

        abilitiesTarget[0] = false;



    }

    public override void Ability2(GameObject targetUnit)
    {
        if(targetUnit.tag == gameObject.tag)
        {
            targetUnit.GetComponent<UnitScript>().attackAvailable = true;
            targetUnit.GetComponent<UnitScript>().moveSpeed = targetUnit.GetComponent<UnitScript>().maxMoveSpeed;
            if(PowerSurge)
            {
                targetUnit.GetComponent<UnitScript>().DamageBuff(1, 4);
                targetUnit.GetComponent<UnitScript>().moveSpeed = targetUnit.GetComponent<UnitScript>().maxMoveSpeed +2;
                
            }
            if (lightningSurgeComboA)
            {
                if(PowerSurge)
                {
                    targetUnit.GetComponent<UnitScript>().DamageBuff(1, 8);

                }
                else
                {
                    targetUnit.GetComponent<UnitScript>().DamageBuff(1, 4);
                }
                lightningSurgeComboA = false;
            }
            else
            {
                if (lightningSurgeCombo)
                {
                    lightningSurgeComboA = true;
                }
            }

            if (combosA[0])
            {
                targetUnit.GetComponent<UnitScript>().moveSpeed = targetUnit.GetComponent<UnitScript>().moveSpeed += 2;
                combosA[0] = false;
            }
            else
            {
                if (combos[0])
                {
                    combosA[0] = true;
                }
            }


            if (combosA[1])
            {
                targetUnit.GetComponent<UnitScript>().HealDamage(5);
                combosA[1] = false;
            }
            else
            {
                if (combos[1])
                {
                    combosA[1] = true;
                }
            }

            if (combosA[2])
            {
                abilitiesCooldown[1]--;
                abilitiesCooldown[2]--;
                combosA[2] = false;
            }
            else
            {
                if (combos[2])
                {
                    combosA[2] = true;
                }
            }

        }
        abilitiesTarget[1] = false;
        abilitiesCooldown[1] = 5;
    }
    

    public override void Ability3(GameObject targetUnit)
    {
        if (Bombardment)
        {
            abilitiesCooldown[2] = 5;
            if (targetUnit.tag != gameObject.tag && CheckAttackDistance(targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY,10))
            {
                targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower / 2); 
                if (massBombardment)
                {
                    RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                    foreach (RaycastHit2D hit in targets)
                    {

                        if (hit.collider.tag == "team1")
                        {
                            targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower / 2);
                            if (combosA[0])
                            {
                                targetUnit.GetComponent<UnitScript>().UnitDamage(5 + targetUnit.GetComponent<UnitScript>().damageReduction);
                            }
                        }
                    }
                }
                if (combosA[0])
                {
                    targetUnit.GetComponent<UnitScript>().UnitDamage(5 +  targetUnit.GetComponent<UnitScript>().damageReduction);
                    combosA[0] = false;
                }
                else
                {
                    if (combos[0])
                    {
                        combosA[0] = true;
                    }
                }
                
            }
            
        }
        if (lifeDrain)
        {
            if(gameObject.tag != targetUnit.tag)
            {
                if (CheckAttackDistance(targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY, attackRange))
                {
                    abilitiesCooldown[2] = 5;
                    targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);
                    gameObject.GetComponent<UnitScript>().HealDamage(attackPower - targetUnit.GetComponent<UnitScript>().damageReduction);
                    if (soulDrain)
                    {
                        targetUnit.GetComponent<UnitScript>().Burn(4, 3);

                    }
                    if (combosA[1])
                    {
                        targetUnit.GetComponent<UnitScript>().UnitDamage(5 + targetUnit.GetComponent<UnitScript>().damageReduction);
                        gameObject.GetComponent<UnitScript>().HealDamage(5);
                        combosA[1] = false;
                    }
                    else
                    {
                        if (combos[1])
                        {
                            combosA[1] = true;
                        }
                    }
                }
            }
        }
        if(incantationOfPower)
        {


            abilitiesCooldown[2] = 7;
            RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, 1f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D hit in targets)
            {
                if (hit.collider.tag == "team1")
                {
                    hit.collider.GetComponent<UnitScript>().DamageBuff(2, 2);
                    if(greaterInvocation)
                    {
                        hit.collider.GetComponent<UnitScript>().attackAvailable = true;
                    }
                  
                }
            }
            if (combosA[2])
            {
                abilitiesCooldown[2]--;
                abilitiesCooldown[1]--;
                combosA[2] = false;
            }
            else
            {
                if (combos[2])
                {
                    combosA[2] = true;
                }
            }
        }
        attackAvailable = false;
        abilitiesTarget[2] = false;
    }

    public override void Ability4(GameObject targetUnit)
    {

        if (fireball)
        {
            if (map.GenerateAttackPath(gameObject, targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY, tileY, tileX).Count - 2 < attackRange)
            {
                animator.SetTrigger("attack");
                abilitiesCooldown[2] = 4;
                attackAvailable = false;
                targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);//simple attack
                RaycastHit2D[] targets = Physics2D.CircleCastAll(targetUnit.transform.position, 1f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D hit in targets)
                {
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        hit.collider.gameObject.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower));
                    }


                }
                abilitiesTarget[3] = false;
                abilitiesCooldown[3] = 6;
            }
        }
        if (breathOfCold)
        {
            if (map.GenerateAttackPath(gameObject, targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY, tileY, tileX).Count - 2 < attackRange)
            {
                animator.SetTrigger("attack");
                abilitiesCooldown[2] = 4;
                attackAvailable = false;
                targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);//simple attack
                RaycastHit2D[] targets = Physics2D.CircleCastAll(targetUnit.transform.position, 1f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D hit in targets)
                {
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        hit.collider.gameObject.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower/2));
                        hit.collider.gameObject.GetComponent<UnitScript>().stunned = true;
                    }


                }
                abilitiesCooldown[3] = 6;
                abilitiesTarget[3] = false;
            }
        }
    }

    public override void Update()
    {
        if (gm.paused == false)
        {
            //First 2 if statements here are testers for final controls, start the attack and set ability one to be active.
            if (Input.GetKeyDown(KeyCode.Space) && map.selectedUnit == gameObject)
            {
                targetting();
                attacking = !attacking;


            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && map.selectedUnit == gameObject)
            {
                abilitiesTarget[0] = !abilitiesTarget[0];
                abilitiesTarget[2] = false;
                abilitiesTarget[1] = false;
                abilitiesTarget[3] = false;
                targetting();
                map.UpdateIconSelection(0);

            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && map.selectedUnit == gameObject)
            {

                abilitiesTarget[0] = false;
                abilitiesTarget[2] = false;
                abilitiesTarget[1] = !abilitiesTarget[1];
                abilitiesTarget[3] = false;
                targetting(1);
                map.UpdateIconSelection(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && map.selectedUnit == gameObject)
            {

                abilitiesTarget[0] = false;
                abilitiesTarget[1] = false;
                abilitiesTarget[2] = !abilitiesTarget[2];
                abilitiesTarget[3] = false;

                if (Bombardment)
                {
                    targetting(10);
                }
                else if(incantationOfPower)
                {
                    targetting(1);
                }
                else
                {
                    targetting();
                }

                map.UpdateIconSelection(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && map.selectedUnit == gameObject)
            {

                abilitiesTarget[0] = false;
                abilitiesTarget[1] = false;
                abilitiesTarget[2] = false;
                abilitiesTarget[3] = !abilitiesTarget[3];
                targetting();
                map.UpdateIconSelection(3);
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
            if (map.selectedUnit == gameObject)
            {
                map.UpdateCooldowns(gameObject);
            }


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

    }
    public override void TurnOver()
    {
        

       
        base.TurnOver();
    }

}
