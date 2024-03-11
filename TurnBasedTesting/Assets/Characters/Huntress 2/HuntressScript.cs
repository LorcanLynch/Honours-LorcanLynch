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
            abilitiesCooldown[2] = 4;
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
