using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiScript : UnitScript
{

    public override void Ability1(GameObject targetUnit)
    {
        attackAvailable = false;
        ///<summary>
        ///Allows the Samurai to use execute
        ///</summary>
        if (targetUnit.GetComponent<UnitScript>().health < targetUnit.GetComponent<UnitScript>().maxhealth / 2)
        {
            int hitChance = Random.Range(0, 100);//Does a normal attack, then if the target's health is lower than half it's max then it does extra damage, otherwise it does normal damage
            animator.SetTrigger("attack");
            if (hitChance < accuracy - targetUnit.GetComponent<UnitScript>().dodgeRating)

            {
                targetUnit.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower * 1.5f));
            }

        }
        else
        {
            int hitChance = Random.Range(0, 100);

            animator.SetTrigger("attack");
            if (hitChance < accuracy - targetUnit.GetComponent<UnitScript>().dodgeRating)

            {
                targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);
            }
        }
        abilitiesTarget[0] = false;
    }

    public override void Ability2(GameObject targetUnit)
    {
        abilitiesTarget[1] = false;
    }





}
