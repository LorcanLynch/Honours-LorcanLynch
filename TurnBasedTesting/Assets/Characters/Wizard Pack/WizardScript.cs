using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class WizardScript : UnitScript
{
    public bool fireball;
    public bool lifeDrain;
    public bool incantationOfPower;
    public bool greaterInvocation;
    public bool PowerSurge;
    public bool Bombardment;
    public bool soulDrain;
  
    public int drainedTimer;
  
    
   
    public bool fireLord;
    public bool iceLord;
    public bool breathOfCold = false;
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


                attackAvailable = false;
                attacking = false;
            }
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
            
        }
        abilitiesTarget[1] = false;
    }
    

    public override void Ability3(GameObject targetUnit)
    {
        
        if (targetUnit.tag != gameObject.tag && map.GenerateAttackPath(gameObject, targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY, tileY, tileX).Count - 2 < 10 && Bombardment)
        {
            targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower/2);
           
        }

        if (lifeDrain)
        {
            if(gameObject.tag != targetUnit.tag)
            {
                targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);
                gameObject.GetComponent<UnitScript>().HealDamage(attackPower - targetUnit.GetComponent<UnitScript>().damageReduction);
                if(soulDrain)
                {
                    targetUnit.GetComponent<UnitScript>().Burn(4,3) ;
                   
                }
            }
        }
        if(incantationOfPower)
        {   
          
            
            abilitiesCooldown[2] = 4;
            RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D hit in targets)
            {
                if (hit.collider.tag == "team1")
                {
                    hit.collider.GetComponent<UnitScript>().DamageBuff(2, 2);
                    if(greaterInvocation)
                    {
                        hit.collider.GetComponent<UnitScript>().DamageBuff(2, 4);
                    }
                  
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
                RaycastHit2D[] targets = Physics2D.CircleCastAll(targetUnit.transform.position, .8f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D hit in targets)
                {
                    if (hit.collider.gameObject.layer == 6)
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
                RaycastHit2D[] targets = Physics2D.CircleCastAll(targetUnit.transform.position, .8f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D hit in targets)
                {
                    if (hit.collider.gameObject.layer == 6)
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
    public override void TurnOver()
    {
        

       
        base.TurnOver();
    }

}
