using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedHound : UnitScript
{
    public override void attack(GameObject target)
    {
        attackAvailable = false;
        int hitChance = Random.Range(0, 100);

        animator.SetTrigger("attack");
        if (hitChance < accuracy - target.GetComponent<UnitScript>().dodgeRating)

        {
            target.GetComponent<UnitScript>().UnitDamage(attackPower);
            target.GetComponent<UnitScript>().Burn(2,1);
        }
        else
        {
            text.GetComponent<DamageTextScript>().UpdateText("Miss");
        }
        attacking = false;

    }
}

