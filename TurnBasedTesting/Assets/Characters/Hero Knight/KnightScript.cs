using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KnightScript : UnitScript
{
    // Start is called before the first frame update
    public bool parryStrikeback;
    public bool parryDamage;
    public bool parryHoldfast;

    public bool lordlyGreatsword;
    public bool domumHalberd;

    public bool frenziedStrike;
    public bool flamingBlade;
    public bool righteousGlory;
    public bool unstopableForce;

    public int litBlade;
    public int gloryActivated;
    public int unstopableWarrior;
    public int parryActive;
    public List<GameObject> gloried = new List<GameObject>();
    
    
    public override void UnitDamage(float AP)
    {
        if (unstopableWarrior < 1)
        {
            if (parryStrikeback)
            {

                RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D target in targets)
                {
                    if (target.collider.tag == "Enemy")
                    {
                        target.collider.gameObject.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower));
                    }
                }
                animator.SetTrigger("attack");
                parryStrikeback = false;
            }
            else
            {
                health -= Mathf.Clamp(AP - damageReduction, 1, 100);
                
                text.GetComponent<DamageTextScript>().UpdateText(Mathf.RoundToInt((AP - damageReduction) * -1).ToString());
                if (health <= 0)
                {
                    animator.SetTrigger("death");
                    map.GetComponent<TileMap>().RemoveUnit(gameObject);
                    Destroy(gameObject, 1.2f);

                }
                else
                {

                    animator.SetTrigger("damage");
                }
            }
        }
    }

    public override void attack(GameObject target)
    {

        if(lordlyGreatsword)
        {
            RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D hit in targets)
            {
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.gameObject.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower / 4));
                }
            }
        }
        int hitChance = Random.Range(0, 100);

        animator.SetTrigger("attack");
        if (hitChance < accuracy - target.GetComponent<UnitScript>().dodgeRating)

        {
            target.GetComponent<UnitScript>().UnitDamage(attackPower);
            if(litBlade > 0)
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

    public override void Ability1(GameObject targetUnit)
    {
        ///<summary>
        ///Allows the knight to use it's cleave abiltiy
        /// </summary>
        animator.SetTrigger("attack");
        abilitiesCooldown[0] = 2;
        attackAvailable = false;
        targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);//simple attack
        RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
        foreach (RaycastHit2D hit in targets)
        {
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower / 2));
                if (domumHalberd)
                {
                    hit.collider.gameObject.GetComponent<UnitScript>().stunned = true;
                }
            }
        }
        abilitiesTarget[0] = false;

    }

    public override void Ability2(GameObject targetUnit)
    {
        map.UpdateCooldowns(gameObject);
        parryActive = 2;
        attackAvailable = false;
        baseDodge = dodgeRating;
        baseReduction = damageReduction;
        abilitiesCooldown[1] = 4;
        damageReduction = 20;
        dodgeRating = dodgeRating * 2;
        if (parryDamage)
        {
            parryStrikeback = true;
        }
        if (parryHoldfast)
        {
            dodgeRating *= 2;
            abilitiesCooldown[1] = 3;
        }
        baseDodge = dodgeRating - baseDodge;
        baseReduction = damageReduction - baseReduction;
        abilitiesTarget[1] = false;
    }

    public override void Ability3(GameObject targetUnit)
    {
        
        
        if(frenziedStrike)
        {
            if (CheckAttackDistance(targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY) && targetUnit.tag == "Enemy")
            {
                base.attack(targetUnit);
                base.attack(targetUnit);
                base.attack(targetUnit);
                abilitiesCooldown[2] = 4;
                attackAvailable = false;
            }
        }    

        if(unstopableForce)
        {
            unstopableWarrior = 2;
            health += Random.Range(1, maxhealth);
            if(health > maxhealth)
                health = maxhealth;
            abilitiesCooldown[2] = 4;
            attackAvailable = false;
        }
        if(flamingBlade)
        {
            litBlade = 3;
            abilitiesCooldown[2] = 4;
        }
        if(righteousGlory)
        {
            gloried.Clear();
            gloryActivated = 2;
            abilitiesCooldown[2] = 4;
            RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, 1.6f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D hit in targets)
            {
                if (hit.collider.tag == "team1")
                {
                    hit.collider.GetComponent<UnitScript>().damageReduction += 2;
                    gloried.Add(hit.collider.gameObject);
                }
            }
            attackAvailable = false;

        }
        map.UpdateCooldowns(gameObject);
        abilitiesTarget[2] = false;
    }

    public override void TurnOver()
    {

        parryActive--;
        gloryActivated--;
        if(gloryActivated ==0)
        {
            for(int i = 0; i < gloried.Count; i++)
            {
                gloried[i].GetComponent<UnitScript>().damageReduction -= 2;
            }    
        }
        if (parryActive==0)
        {
            dodgeRating -= baseDodge;
            damageReduction -= baseReduction;
        }
        unstopableWarrior--;
        litBlade--;
        base.TurnOver();
    }


    // Update is called once per frame

}
