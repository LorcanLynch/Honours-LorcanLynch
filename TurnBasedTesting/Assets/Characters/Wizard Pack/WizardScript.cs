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

    public override void AbilityTarget(int abilityIndex)
    {
        int range = attackRange;
        for (int i = 0; i < 4; i++)
        {
            if (i == abilityIndex)
            {
                abilitiesTarget[abilityIndex] = !abilitiesTarget[abilityIndex];
            }
            else
            {
                abilitiesTarget[i] = false;
            }
        }
        if (Bombardment && abilityIndex == 2)
        {
            range = 10;
        }
        if (incantationOfPower && abilityIndex == 2)
        {
            range = 1;
        }
        if (abilityIndex == 1)
        {
            range = 1;
        }
        
        attacking = false;


        targetting(range);
        map.UpdateIconSelection(abilityIndex);

        if (abilitiesTarget[abilityIndex] == false)
        {
            CancelTarggeting();
        }

    }

    
    public override void TurnOver()
    {
        

       
        base.TurnOver();
    }

}
