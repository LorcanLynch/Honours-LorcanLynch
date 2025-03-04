
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool bloodThirst;
    public bool bloodThirstA;

    public bool clearMind;

    public bool shuriken;
    public bool shreddingShuriken;
    public int agileA;

    public bool trueStrike;
    public bool trueStrikeR;
    public AudioClip buffEffect;
    public bool swiftExe;
    public bool swiftExeA;

    public bool harmony;

    public bool endlessWind;
    public bool[] abilityCombos = new bool[3];
    public bool[] abilityCombosA = new bool[3];

    //Do the loops to break combos
    public override void attack(GameObject target)
    {

        if (attackAvailable)
        {
            if (!trueStrikeR)
            {


                int hitChance = Random.Range(0, 100);

                animator.SetTrigger("attack");
                if (hitChance < accuracy - target.GetComponent<UnitScript>().dodgeRating)

                {
                    aSource.PlayOneShot(hitEffect);
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
                    aSource.PlayOneShot(missEffect);
                }
            }
            else
            {
                trueStrikeR = false;
                if (exploitA)
                {
                    target.GetComponent<UnitScript>().UnitDamage(attackPower + target.GetComponent<UnitScript>().damageReduction);
                }
                else
                {
                    target.GetComponent<UnitScript>().UnitDamage(attackPower);
                }
            }

            attacking = false;

            attackAvailable = false;

            if (target.GetComponent<UnitScript>().health <= 0)
            {
                if (bloodThirstA)
                {
                    attackAvailable = true;
                }
            }
            swiftExeA = false;
            for (int i = 0; i < 3; i++)
            {
                abilityCombosA[i] = false;
            }
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
                aSource.PlayOneShot(hitEffect);
                int hitChance = Random.Range(0, 100);//Does a normal attack, then if the target's health is lower than half it's max then it does extra damage, otherwise it does normal damage
                animator.SetTrigger("attack");
                if (hitChance < accuracy - targetUnit.GetComponent<UnitScript>().dodgeRating)

                {
                    targetUnit.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower * 1.5f));
                }
                else
                {
                    text.GetComponent<DamageTextScript>().UpdateText("Miss");
                    aSource.PlayOneShot(missEffect);
                }
                if (targetUnit.GetComponent<UnitScript>().health <= 0)
                {
                    if (thrill || bloodThirstA)
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
                else
                {
                    text.GetComponent<DamageTextScript>().UpdateText("Miss");
                    aSource.PlayOneShot(missEffect);
                }
                if (targetUnit.GetComponent<UnitScript>().health <= 0)
                {
                    if (thrill || bloodThirstA)
                    {
                        attackAvailable = true;
                    }
                    if (swiftExeA)
                    {
                        HealDamage(5);
                    }
                }
            }
        }
        if (!swiftExeA)
        {
            swiftExeA = true;
        }
        else
        {
            if (swiftExe)
            {
                swiftExeA = false;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            abilityCombosA[i] = false;
        }

        abilitiesTarget[0] = false;
        abilitiesCooldown[0] = 3;
    }

    public override void Ability2(GameObject targetUnit)
    {
        abilitiesTarget[1] = false;
        moveSpeed += 4;
        abilitiesCooldown[1] = 3;
        aSource.PlayOneShot(buffEffect);
        if (!swiftExeA)
        {
            swiftExeA = true;
        }
        else
        {
            health += 2;
            swiftExeA = false;
        }

        if (abilityCombosA[0])
        {
            moveSpeed += 2;
            abilityCombosA[0] = false;
        }
        else
        {
            if (abilityCombos[0])
            {
                abilityCombosA[0] = true;
            }
        }

        if (abilityCombosA[1])
        {
            moveSpeed += 2;
            abilityCombosA[1] = false;
        }
        else
        {
            if (abilityCombos[1])
            {
                abilityCombosA[1] = true;
            }
        }

        if (endlessWind)
        {
            DodgeBuff(2, 40);
        }
    }

    public override void Ability3(GameObject targetUnit)
    {
        if (focus)
        {
            aSource.PlayOneShot(buffEffect);
            if (harmony)
            {
                health = maxhealth;
                attackAvailable = false;
            }

            DamageBuff(3, 2);
            DRBuff(3, 1);
            DodgeBuff(3, 10);


            
            abilitiesCooldown[2] = 5;

            if (abilityCombosA[0])
            {
                DamageBuff(4, 2);
                DRBuff(4, 1);
                DodgeBuff(4, 10);
                focusT++;
                abilityCombosA[0] = false;
            }
            else
            {
                if (abilityCombos[0])
                {
                    abilityCombosA[0] = true;
                }
            }
            swiftExeA = false;

        }
        if (exploit)
        {
            aSource.PlayOneShot(buffEffect);
            exploitA = true;
            exploitT = 2;
            abilitiesCooldown[2] = 4;
            if (abilityCombosA[1])
            {
                focusT++;
                abilityCombosA[1] = false;
            }
            else
            {
                if (abilityCombos[1])
                {
                    abilityCombosA[1] = true;
                }
            }
            swiftExeA = false;
        }
        if (shuriken)
        {
            if (targetUnit.tag != gameObject.tag)
            {
                if (CheckAttackDistance(targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY, 3))
                {


                    int hitChance = Random.Range(0, 100);
                    animator.SetTrigger("attack");
                    if (hitChance < accuracy - targetUnit.GetComponent<UnitScript>().dodgeRating)

                    {
                        aSource.PlayOneShot(hitEffect);
                        targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);
                        if (abilityCombosA[2])
                        {
                            targetUnit.GetComponent<UnitScript>().UnitDamage(4 + targetUnit.GetComponent<UnitScript>().damageReduction);
                        }
                        if (shreddingShuriken)
                        {
                            targetUnit.GetComponent<UnitScript>().Burn(2, 2);
                        }
                    }
                    else
                    {
                        text.GetComponent<DamageTextScript>().UpdateText("Miss");
                        aSource.PlayOneShot(missEffect);
                    }

                    abilitiesCooldown[2] = 4;
                }
                

                if (abilityCombosA[2])
                {

                    abilityCombosA[2] = false;
                }
                else
                {
                    if (abilityCombos[2])
                    {
                        abilityCombosA[2] = true;
                    }
                }
                swiftExeA = false;
            }
        }
    }

    public override void Ability4(GameObject targetUnit)
    {
        if (bloodThirst)
        {
            aSource.PlayOneShot(buffEffect);
            bloodThirstA = true;
            abilitiesCooldown[3] = 6;
        }
        if (clearMind)
        {
            aSource.PlayOneShot(buffEffect);
            for (int i = 0; i < 3; i++)
            {
                abilitiesCooldown[i] = 0;

            }
            abilitiesCooldown[3] = 7;
        }
        for (int i = 0; i < 3; i++)
        {
            abilityCombosA[i] = false;
        }

    }

    public override void TurnOver()
    {
        if (trueStrike)
        {
            trueStrikeR = true;
        }
        if (bloodThirstA)
            bloodThirstA = false;

        exploitT--;
        if (exploitT == 0)
        {
            exploitA = false;
        }

        agileA--;
        if (agileA == 0)
        {
            dodgeRating -= 40;
        }

        focusT--;
        if (focusT == 0)
        {
            accuracy -= 10;
            attackPower -= 2;
            damageReduction -= 1;
            dodgeRating -= 10;


        }
        base.TurnOver();
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
        if (shuriken && abilityIndex == 2)
        {
            range = 3;
        }
        if (abilityIndex == 1)
        {
            range = 0;
        }
        attacking = false;


        targetting(range);
        map.UpdateIconSelection(abilityIndex);

        if (abilitiesTarget[abilityIndex] == false)
        {
            CancelTarggeting();
        }

    }
}

  
