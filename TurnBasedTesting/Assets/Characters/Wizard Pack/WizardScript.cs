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
    
    GameObject surged;
    List<GameObject> buffed = new List<GameObject> { };
    public int incatationed = 0;
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
                surged.GetComponent<UnitScript>().attackPower += 4;
                targetUnit.GetComponent<UnitScript>().moveSpeed = targetUnit.GetComponent<UnitScript>().maxMoveSpeed +2;
                surged = gameObject;
            }
            
        }
        abilitiesTarget[1] = false;
    }
    

    public override void Ability3(GameObject targetUnit)
    {
        
        if (targetUnit.tag != gameObject.tag && map.GenerateAttackPath(gameObject, targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY, tileY, tileX).Count - 2 < 10 && Bombardment)
        {
            targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower/2);
            abilitiesTarget[2] = false;
        }

        if (lifeDrain)
        {
            if(gameObject.tag != targetUnit.tag)
            {
                targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);
                gameObject.GetComponent<UnitScript>().HealDamage(attackPower - targetUnit.GetComponent<UnitScript>().damageReduction);

            }
        }
        if(incantationOfPower)
        {   
            buffed.Clear();
            incatationed = 2;
            abilitiesCooldown[2] = 4;
            RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D hit in targets)
            {
                if (hit.collider.tag == "team1")
                {
                    hit.collider.GetComponent<UnitScript>().attackPower += 2;
                    if(greaterInvocation)
                    {
                        hit.collider.GetComponent<UnitScript>().attackPower += 4;
                    }
                    buffed.Add(hit.collider.gameObject);
                }
            }
            attackAvailable = false;
        }
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
                abilitiesTarget[2] = false;
            }
        }
    }
    public override void TurnOver()
    {
        if(surged != null)
        {
            surged.GetComponent<UnitScript>().attackPower -= 4;
            surged = null;
        }

        incatationed--;
        if (incatationed == 0)
        {
            for (int i = 0; i < buffed.Count; i++)
            {
                buffed[i].GetComponent<UnitScript>().attackPower -= 2;
                if (greaterInvocation)
                {
                    buffed[i].GetComponent<UnitScript>().attackPower -= 4;
                }
            }
        }
        
      
        base.TurnOver();
    }

}
