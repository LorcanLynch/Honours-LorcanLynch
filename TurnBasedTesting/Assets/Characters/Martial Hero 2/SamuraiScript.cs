
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiScript : UnitScript
{
    public bool thrill;
    public bool agileX;
    public bool exploit;
    public int exploitT;
    public bool exploitA;
    public bool focus;
    public int focusT;


    public bool shuriken;

    public int agileA;
    public override void attack(GameObject target)
    {

        if (attackAvailable)
        {
            int hitChance = Random.Range(0, 100);

            animator.SetTrigger("attack");
            if (hitChance < accuracy - target.GetComponent<UnitScript>().dodgeRating)

            {
                if (exploitA)
                {
                    target.GetComponent<UnitScript>().UnitDamage(attackPower + target.GetComponent<UnitScript>().damageReduction);
                }
                else
                {
                    target.GetComponent<UnitScript>().UnitDamage(attackPower);
                }
            }
            else
            {
                text.GetComponent<DamageTextScript>().UpdateText("Miss");
            }
            attacking = false;
            attackAvailable = false;
        }
    }
    public override void Ability1(GameObject targetUnit)
    {
        if (CheckAttackDistance(targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY))
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
                if (targetUnit.GetComponent<UnitScript>().health <= 0)
                {
                    if (thrill)
                    {
                        attackAvailable = true;
                    }
                    if (agileX)
                    {
                        agileA = 2;
                        dodgeRating += 40;
                    }

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
        }
        abilitiesTarget[0] = false;
    }

    public override void Ability2(GameObject targetUnit)
    {
        abilitiesTarget[1] = false;
        moveSpeed += 4;
        abilitiesCooldown[1] = 3;
    }

    public override void Ability3(GameObject targetUnit)
    {
        if(focus)
        {
            accuracy += 10;
            attackPower += 2;
            damageReduction += 1;
            dodgeRating += 10;
            focusT = 2;
        }
        if(exploit)
        {
            exploitA = true;
            exploitT = 2;
        }
        if(shuriken)
        {
            if (CheckAttackDistance(targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY,3))
            {
                

                    int hitChance = Random.Range(0, 100);
                    animator.SetTrigger("attack");
                    if (hitChance < accuracy - targetUnit.GetComponent<UnitScript>().dodgeRating)

                    {
                        targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);
                    targetUnit.GetComponent<UnitScript>().Burn(2, 2);
                    }
                
                }

        }
    }

    public override void TurnOver()
    {
        exploitT--;
        if(exploitT == 0)
        {
            exploitA = false;   
        }
        agileA--;
        if(agileA == 0)
        {
            dodgeRating -= 40;
        }

        focusT--;
        if(focusT == 0)
        {
            accuracy -= 10;
            attackPower -= 2;
            damageReduction -= 1;
            dodgeRating -= 10;
            

        }
        base.TurnOver();
    }

}
