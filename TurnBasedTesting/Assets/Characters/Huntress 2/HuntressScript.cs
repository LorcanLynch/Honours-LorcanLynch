using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntressScript : UnitScript
{
    public bool lastingInfluence;
    public List<GameObject> influenced = new List<GameObject> { };
    public int influenceTimer;
    public int influenceTimerMax;

    public bool markPrey;
    public GameObject prey;
    public float preyTimer;

    public bool snipe;

    public bool exudeWarmth;
   
    public int warmthTimer;

    public bool fieldMedicine;
    public int fieldMedicineUses;
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
        }

        abilitiesTarget[0] = false;


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
                if (prey != null && prey == target)
                {
                    target.GetComponent<UnitScript>().UnitDamage(attackPower + 3);
                }
                else
                {
                    target.GetComponent<UnitScript>().UnitDamage(attackPower);
                }
            }

            attackAvailable = false;
            attacking = false;
        }
    }

    public override void Ability2(GameObject targetUnit)
    {
        map.UpdateCooldowns(gameObject);
        attackAvailable = false;
        RaycastHit2D[] wack = Physics2D.CircleCastAll(gameObject.transform.position, 1, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
        foreach (RaycastHit2D wacked in wack)
        {
            if (wacked.collider.tag == "team1")
            {
                wacked.collider.gameObject.GetComponent<UnitScript>().HealDamage(3);
                if (lastingInfluence)
                {
                    influenced.Add(wacked.collider.gameObject);
                    influenceTimer = influenceTimerMax;

                }
            }
        }
        abilitiesTarget[1] = false;
    }

    public override void Ability3(GameObject targetUnit)
    {
        if (targetUnit.tag != gameObject.tag && markPrey)
        {
            prey = targetUnit;
            preyTimer = 3;
            abilitiesCooldown[2] = 4;
        }
        if (targetUnit.tag == gameObject.tag && fieldMedicine && map.GenerateAttackPath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, targetUnit.GetComponent<UnitScript>().tileY, targetUnit.GetComponent<UnitScript>().tileX).Count < 2)
        {
            if (fieldMedicineUses > 0)
            {
                targetUnit.GetComponent<UnitScript>().HealDamage(6);
                fieldMedicineUses--;

                attackAvailable = false;
            }
        }
        if (snipe && targetUnit.tag != gameObject.tag)
        {
            if (prey != null && prey == targetUnit)
            {
                targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower + 13);
            }
            else
            {
                targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower + 10);
            }
        }
        abilitiesTarget[2] = false;
    }
    public override void Ability4(GameObject targetUnit)
    {
        if(exudeWarmth)
        {
           
            warmthTimer = 4;
        }
    }
    public override void TurnOver()
    {
        if (lastingInfluence && influenceTimer > 0)
        {
            for (int i = 0; i < influenced.Count; i++)
            {
                influenced[i].GetComponent<UnitScript>().HealDamage(2);

            }
            influenceTimer--;
            if(influenceTimer == 0)
            {
                influenced.Clear();
            }
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
