using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntressScript : UnitScript
{
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
            }
        }
        abilitiesTarget[1] = false;
    }
}
