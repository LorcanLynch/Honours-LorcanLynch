using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntressScript : UnitScript
{
    public bool lastingInfluence;
   

    public bool markPrey;
    public GameObject prey;
    public float preyTimer;

    public bool snipe;

    public bool exudeWarmth;
   
    public int warmthTimer;

    public bool fieldMedicine;
    public int fieldMedicineUses;

    public bool eyes;

    public bool thirdCharm;
    public int thirdCharmC;

    public bool warding;

    public bool trainedSurgeon;

    public bool longShot;
    public bool TrueShot;
    public int trueShotTimer = 0;

    public bool songCombo;
    public bool songComboA;

    public bool[] abilityCombos = new bool[4];
    public bool[] abilityCombosA = new bool[4];
    public override void Ability1(GameObject targetUnit)
    {
        ///<summary>
        ///Simple attack that simply ignores a portion of the target unit's armour
        /// </summary>
        int hitChance = Random.Range(0, 100);
        attackAvailable = false;
        animator.SetTrigger("attack");

        if (hitChance < accuracy - targetUnit.GetComponent<UnitScript>().dodgeRating)

        {
            targetUnit.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower + (targetUnit.GetComponent<UnitScript>().damageReduction * .75f)));
            if (eyes)
            {
                int critChance = Random.Range(0, 100);
                if (critChance < 10)
                {
                    targetUnit.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower + (targetUnit.GetComponent<UnitScript>().damageReduction * .75f)));
                }
            }
            if(songComboA)
            {
                if (targetUnit.GetComponent<UnitScript>().damageReduction < 3)
                {
                    targetUnit.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower + (targetUnit.GetComponent<UnitScript>().damageReduction * .75f)));
                }
            }
            if(thirdCharm && (thirdCharmC >= 2))
            {
                targetUnit.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower + (targetUnit.GetComponent<UnitScript>().damageReduction * .75f)));
                
            }
        }
        thirdCharmC++;
        abilitiesTarget[0] = false;
        abilitiesCooldown[0] = 2;
        if (!songCombo)
        {
            songComboA = true;
        }
        else
        {
            if (songCombo)
            {
                songComboA = false;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            abilityCombosA[i] = false;
        }

    }

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
                    if (eyes)
                    {
                        int critChance = Random.Range(0, 100);
                        if (critChance < 10)
                        {
                            target.GetComponent<UnitScript>().UnitDamage(attackPower);
                        }
                    }
                    if (prey != null && prey == target)
                    {
                        target.GetComponent<UnitScript>().UnitDamage(target.GetComponent<UnitScript>().damageReduction + 3);
                    }
                if (thirdCharm && (thirdCharmC >= 2))
                {
                    target.GetComponent<UnitScript>().UnitDamage(attackPower);

                }

            }

            songComboA = false;
            for (int i = 0; i < 3; i++)
            {
                abilityCombosA[i] = false;
            }
            thirdCharmC++;
            attackAvailable = false;
            attacking = false;
        }
    }

    public override void Ability2(GameObject targetUnit)
    {
        if (attackAvailable)
        {
            abilitiesCooldown[1] = 4;
            map.UpdateCooldowns(gameObject);
            attackAvailable = false;
            RaycastHit2D[] wack = Physics2D.CircleCastAll(gameObject.transform.position, 1, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D wacked in wack)
            {
                if (wacked.collider.tag == "team1")
                {
                    int j = 0;
                    for (int i = 0; i < 4; i++)
                    {

                        if (abilityCombos[i])
                        {
                            if (abilityCombosA[i])
                            {
                                wacked.collider.gameObject.GetComponent<UnitScript>().health += 3;
                                j++;
                            }
                        }
                    }
                    if (j == 0)
                    {
                        wacked.collider.gameObject.GetComponent<UnitScript>().HealDamage(3);
                    }

                    if (lastingInfluence)
                    {
                        wacked.collider.gameObject.GetComponent<UnitScript>().HealOverTime(2, 3);


                    }

                    if (warding)
                    {
                        wacked.collider.gameObject.GetComponent<UnitScript>().DRBuff(2, 3);
                    }

                    if (songComboA)
                    {
                        wacked.collider.gameObject.GetComponent<UnitScript>().moveSpeed += 2;
                    }

                }
            }
            abilitiesTarget[1] = false;

            if (!songCombo)
            {
                songComboA = true;
            }
            else
            {
                if (songCombo)
                {
                    songComboA = false;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (abilityCombos[i] == true)
                {
                    if (abilityCombosA[i] == false)
                    {
                        abilityCombosA[i] = true;
                    }
                }
                else
                {
                    abilityCombosA[i] = false;
                }

            }
        }
    }

    public override void Ability3(GameObject targetUnit)
    {
        if (targetUnit.tag != gameObject.tag && markPrey)
        {
            prey = targetUnit;
            preyTimer = 3;
            abilitiesCooldown[2] = 6;
            if (abilityCombos[1] == true)
            {
                if (abilityCombosA[1] == false)
                {
                    abilityCombosA[1] = true;
                }
               else
                {
                    targetUnit.GetComponent<UnitScript>().Burn(2, 2);
                    abilityCombosA[1] = true;
                }
            }
        }


        if (targetUnit.tag == gameObject.tag && fieldMedicine && map.GenerateAttackPath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, targetUnit.GetComponent<UnitScript>().tileY, targetUnit.GetComponent<UnitScript>().tileX).Count < 2)
        {
            if (fieldMedicineUses > 0)
            {
                if (abilityCombosA[0] == true)
                {
                    targetUnit.GetComponent<UnitScript>().health +=3;
                    fieldMedicineUses--;

                    attackAvailable = false;
                    if (trainedSurgeon)
                    {
                        targetUnit.GetComponent<UnitScript>().health+=1;
                    }
                    abilityCombosA[0] = false;
                }
                else
                {
                    targetUnit.GetComponent<UnitScript>().HealDamage(3);
                    fieldMedicineUses--;

                    attackAvailable = false;
                    if (trainedSurgeon)
                    {
                        targetUnit.GetComponent<UnitScript>().HealDamage(1);
                    }
                }

                if (abilityCombos[0] == true)
                {
                    if (abilityCombosA[0] == false)
                    {
                        abilityCombosA[0] = true;
                    }
                }
               
            }
        }
        if (snipe && targetUnit.tag != gameObject.tag)
        {
            abilitiesCooldown[2] = 5;
            if(!longShot)
            {
                
                if (CheckAttackDistance(targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY, attackRange))
                {
                    targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower + 10);
                    if (eyes)
                    {
                        int critChance = Random.Range(0, 100);
                        if (critChance < 10)
                        {
                            targetUnit.GetComponent<UnitScript>().UnitDamage((attackPower + 10));
                        }
                    }
                    if (prey != null && prey == targetUnit)
                    {
                        targetUnit.GetComponent<UnitScript>().UnitDamage(targetUnit.GetComponent<UnitScript>().damageReduction + 3);
                    }
                }
                if (thirdCharm && (thirdCharmC >= 2))
                {
                targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower + 10);
    
                }
                if (abilityCombosA[2] == true)
                {
                    targetUnit.GetComponent<UnitScript>().UnitDamage(targetUnit.GetComponent<UnitScript>().damageReduction + 5);
                    abilityCombosA[2] = false;
                }
            }
            else
            {
                if (CheckAttackDistance(targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY, attackRange+3))
                {
                    targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower + 10);
                    if (eyes)
                    {
                        int critChance = Random.Range(0, 100);
                        if (critChance < 10)
                        {
                            targetUnit.GetComponent<UnitScript>().UnitDamage((attackPower + 10));
                        }
                    }
                    if (prey != null && prey == targetUnit)
                    {
                        targetUnit.GetComponent<UnitScript>().UnitDamage(targetUnit.GetComponent<UnitScript>().damageReduction + 3);
                    }
                    if (thirdCharm && (thirdCharmC >= 2))
                    {
                        targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower + 10);

                    }
                    if (abilityCombosA[2] == true)
                    {
                        targetUnit.GetComponent<UnitScript>().UnitDamage(targetUnit.GetComponent<UnitScript>().damageReduction + 5);
                        abilityCombosA[2] = false;
                    }
                }
            }
            thirdCharmC++;
            if (abilityCombos[2] == true)
            {
                if (abilityCombosA[2] == false)
                {
                    abilityCombosA[2] = true;
                }
               
            }


        }
        abilitiesTarget[2] = false;
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

                if(longShot)
                {
                    targetting(7);
                }
                else if(fieldMedicine)
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
    public override void Ability4(GameObject targetUnit)
    {
        if(exudeWarmth)
        {
           
            warmthTimer = 4;
            abilitiesCooldown[3] = 8;
        }
        if(TrueShot)
        {
            trueShotTimer = 4;
            accuracy += 200;
            dodgeRating += 20;
            abilitiesCooldown[3] = 8;
        }
    }
    public override void TurnOver()
    {
        trueShotTimer--;
        if(trueShotTimer ==0)
        {
            accuracy -= 200;
            dodgeRating -= 20;
        }    
        
        if(prey!= null)
        {
            preyTimer--;
            if(preyTimer == 0)
            {
                prey = null;
            }
        }
        if(warmthTimer > 0)
        {
            warmthTimer--;
            RaycastHit2D[] warmed = Physics2D.CircleCastAll(gameObject.transform.position, 1, new Vector2(0, 0));
            foreach (RaycastHit2D healed in warmed)
            {
                if (healed.collider.tag == "team1")
                {
                    healed.collider.gameObject.GetComponent<UnitScript>().HealDamage(2);
                   
                }
            }
        }
        


        base.TurnOver();
    }

}
