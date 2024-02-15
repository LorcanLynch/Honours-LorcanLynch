
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

public class KnightScript : UnitScript
{
    // Start is called before the first frame update
    public bool parryStrikeback;
    public bool parryDamage;
    public bool parryHoldfast;
    public bool parryStrikebackF;

    public bool lordlyGreatsword;
    public bool domumHalberd;

    public bool frenziedStrike;
    public bool flamingBlade;
    public bool righteousGlory;
    public bool unstopableForce;

    public bool deathKnight;
    public bool slayer;

    public int slayerDamage;

    public int litBlade;
    public bool lingeringFlame;

    public bool gloriousWarrior;

    public int gloryActivated;
    public int unstopableWarrior;
    public int parryActive;
    public List<GameObject> gloried = new List<GameObject>();

    public bool riposte;
    public bool riposteCombo;

    public bool rawGrit;
    public bool rawGritUsed;

    public bool adrenalineRush;

    public bool[] combos = new bool[4] ;
    public bool[] combosU = new bool[4];
    public bool hardened;

    public bool unmovable;
    public bool unmovableA;

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
                        target.collider.gameObject.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower +slayerDamage));
                        if (unmovableA)
                        {
                            target.collider.gameObject.GetComponent<UnitScript>().stunned = true;
                        }
                    }

                }
                animator.SetTrigger("attack");
                parryStrikeback = false;
            }
            else
            {
                if (parryStrikebackF)
                {

                    RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                    foreach (RaycastHit2D target in targets)
                    {
                        if (target.collider.tag == "Enemy")
                        {
                            target.collider.gameObject.GetComponent<UnitScript>().Burn(3,2);
                            target.collider.gameObject.GetComponent<UnitScript>().stunned = true;
                        }

                    }
                    animator.SetTrigger("attack");
                    parryStrikebackF = false;
                }
                health -= Mathf.Clamp(AP - damageReduction, 1, 100);
                if (slayer)
                {
                    if (slayerDamage < 5)
                    {
                        slayerDamage++;
                    }
                }
                text.GetComponent<DamageTextScript>().UpdateText(Mathf.RoundToInt((AP - damageReduction) * -1).ToString());
                if (health <= 0)
                {
                    if(rawGrit)
                    {
                        if(rawGritUsed)
                        {
                            animator.SetTrigger("death");
                            map.GetComponent<TileMap>().RemoveUnit(gameObject);
                            Destroy(gameObject, 1.2f);
                        }
                        else
                        {
                            rawGritUsed = true;
                            health = 1;
                            animator.SetTrigger("damage");
                        }
                    }
                    else
                    {
                        animator.SetTrigger("death");
                        map.GetComponent<TileMap>().RemoveUnit(gameObject);
                        Destroy(gameObject, 1.2f);
                    }
                       
                   
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
        if (attackAvailable)
        {
            if (lordlyGreatsword)
            {
                RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D hit in targets)
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        hit.collider.gameObject.GetComponent<UnitScript>().UnitDamage(Mathf.Round(attackPower / 4));
                        if(deathKnight)
                        {
                            HealDamage(1);
                        }
                    }
                }
            }
            int hitChance = Random.Range(0, 100);

            animator.SetTrigger("attack");
            if (hitChance < accuracy - target.GetComponent<UnitScript>().dodgeRating)

            {
                target.GetComponent<UnitScript>().UnitDamage(attackPower +slayerDamage);
                if (litBlade > 0)
                {
                    target.GetComponent<UnitScript>().UnitDamage(attackPower/2);
                    if(lingeringFlame)
                    {
                        target.GetComponent<UnitScript>().Burn(2, 3);
                    }
                }
                if(adrenalineRush && health <= maxhealth/2)
                {
                    target.GetComponent<UnitScript>().UnitDamage(1);
                    target.GetComponent<UnitScript>().UnitDamage(1);
                    target.GetComponent<UnitScript>().UnitDamage(1);
                  
                }
                if(deathKnight)
                {
                    HealDamage((attackPower - target.GetComponent<UnitScript>().damageReduction)/2);
                }
                if(unmovableA)
                {
                    target.GetComponent<UnitScript>().stunned = true;
                }
            }
            else
            {
                text.GetComponent<DamageTextScript>().UpdateText("Miss");
            }

            for (int i = 0; i < combos.Length; i++)
            {
                combos[i] = false;
            }
            attacking = false;
            riposteCombo = false;
            attackAvailable = false;
        }
    }

    public override void Ability1(GameObject targetUnit)
    {
        ///<summary>
        ///Allows the knight to use it's cleave abiltiy
        /// </summary>
        animator.SetTrigger("attack");
        abilitiesCooldown[0] = 4;
        attackAvailable = false;
        targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);//simple attack
        RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
        if (riposte)
        {
            if (!riposteCombo)
            {
                riposteCombo = true;
            }
            else
            {
                riposteCombo = false;
                abilitiesCooldown[0] = 3;
                abilitiesCooldown[0]--;
            }

        }
        foreach (RaycastHit2D hit in targets)
        {
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<UnitScript>().UnitDamage(Mathf.Round((attackPower + slayerDamage) / 2));
                if (unmovableA)
                {
                    hit.collider.gameObject.GetComponent<UnitScript>().stunned = true;
                }
                if (domumHalberd)
                {
                    hit.collider.gameObject.GetComponent<UnitScript>().stunned = true;
                }
                if(deathKnight)
                {
                    HealDamage((attackPower - targetUnit.GetComponent<UnitScript>().damageReduction) / 2);
                }

            }
        }
        for (int i = 0; i < combos.Length; i++)
        {
            combos[i] = false;
        }
        map.UpdateCooldowns(gameObject);
        abilitiesTarget[0] = false;

    }

    public override void Ability2(GameObject targetUnit)
    {
        
        parryActive = 2;
        attackAvailable = false;
        baseDodge = dodgeRating;
        baseReduction = damageReduction;
        abilitiesCooldown[1] = 4;
        damageReduction += 3;
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
        if (riposte)
        {
            if (!riposteCombo)
            {
                riposteCombo = true;
            }
            else
            {
                riposteCombo = false;
                damageReduction += 3;
            }
        }
        if (combosU[3])
        {
            if (combos[3])
            {
                RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D hit in targets)
                {
                    if (hit.collider.tag == "Team1")
                    {
                        hit.collider.gameObject.GetComponent<UnitScript>().HealDamage(5);

                    }
                }
                combos[3] = false;
            }
            else
            {
                combos[3] = true;
            }
        }
        if (combosU[0])
        {
            if (combos[0])
            {
                RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D hit in targets)
                {
                    if (hit.collider.tag == "Team1")
                    {
                        hit.collider.gameObject.GetComponent<UnitScript>().HealDamage(5);

                    }
                }
                combos[0] = false;
            }
            else
            {
                combos[0] = true;
            }
        }

        if (combosU[1])
        {
            if (combos[1])
            {
                parryActive++;
                combos[1] = false;
            }
            else
            {
                combos[1] = true;
            }
        }


        if (combosU[2])
        {
            if (combos[2])
            {
                parryStrikebackF = true;
                combos[2] = false;
            }
            else
            {
                combos[2] = true;
            }
        }


        
        
        baseDodge = dodgeRating - baseDodge;
        baseReduction = damageReduction - baseReduction;
        map.UpdateCooldowns(gameObject);
        abilitiesTarget[1] = false;
    }

    public override void Ability3(GameObject targetUnit)
    {
        
        
        if(frenziedStrike)
        {
            if (CheckAttackDistance(targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY) && targetUnit.tag == "Enemy")
            {
                attack(targetUnit);
                attack(targetUnit);
                attack(targetUnit);
                abilitiesCooldown[2] = 4;
                attackAvailable = false;
                riposteCombo = false;
                if (combosU[0])
                {
                    if (!combos[0])
                    {
                        combos[0] = true;
                    }
                    else
                    {
                        combos[0] = false;
                        attack(targetUnit);
                    }

                }
               
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
            riposteCombo = false;


            if (combosU[1])
            {
                if (!combos[1])
                {
                    combos[1] = true;
                }
                else
                {
                    combos[1] = false;
                    unstopableWarrior++;
                }

            }

            if(unmovable)
            {
                unmovableA = true;
            }
            
            
        }

        ///FlamingBlade
        if(flamingBlade)
        {
            litBlade = 3;
            abilitiesCooldown[2] = 4;
            riposteCombo = false;
            

            if (combosU[2])
            {
                if (!combos[2])
                {
                    combos[2] = true;
                }
                else
                {
                    combos[2] = false;
                    litBlade++;
                }

            }
          
        }


        ///Righteous
        if(righteousGlory)
        {
            
            abilitiesCooldown[2] = 4;
            RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, 1.6f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D hit in targets)
            {
                if (hit.collider.tag == "team1")
                {
                    DRBuff(2, 2);
                }
            }
            attackAvailable = false;
            riposteCombo = false;
            if(gloriousWarrior)
            {
                foreach (RaycastHit2D hit in targets)
                {
                    if (hit.collider.tag == "team1")
                    {
                        hit.collider.GetComponent<UnitScript>().DamageBuff(2,2);

                    }
                }
            }
            if (combosU[3])
            {
                if (!combos[3])
                {
                    combos[3] = true;
                }
                else
                {
                    combos[3]= false;
                    foreach (RaycastHit2D hit in targets)
                    {
                        if (hit.collider.tag == "team1")
                        {
                            hit.collider.GetComponent<UnitScript>().HealDamage(5);
                            
                        }
                    }
                }

            }

         
                
        }

        map.UpdateCooldowns(gameObject);
        abilitiesTarget[2] = false;
    }

    public override void TurnOver()
    {
        

        if(stunned )
        {
            if (!hardened)
            {
                attackAvailable = false;
            }
            else
            {
                attackAvailable = true;
            }

        }
        else
        {
            attackAvailable = true;
        }
        parryActive--;
        
        if (parryActive==0)
        {
            dodgeRating -= baseDodge;
            damageReduction -= baseReduction;
        }
        unstopableWarrior--;
        if(unstopableWarrior==0)
        {
            unmovableA = false;
        }

        litBlade--;
        moveSpeed = maxMoveSpeed;
        for (int i = 0; i < abilitiesCooldown.Length; i++)
        {
            if (abilitiesCooldown[i] > 0)
            {
                abilitiesCooldown[i]--;
            }

        }
        {
            RaycastHit2D[] tilesAffected = Physics2D.CircleCastAll(gameObject.transform.position, (.8f * attackRange), new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D tile in tilesAffected)
            {
                if (tile.collider.gameObject.layer == 8)
                {
                    tile.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }

        if (burning)
        {
            UnitDamage(burnDamage + damageReduction);
            burnTimer--;
            if (burnTimer == 0)
            {
                burning = false;
            }
        }

        if (healing)
        {
            HealDamage(heal);
            healTimer--;
            if (healTimer <= 0)
            {
                healing = false;
            }
        }

        if (dodgeBuffed)
        {


            if (dodgeBuffT <= 0)
            {
                dodgeBuffed = false;
                dodgeRating -= dodgeBuffN;

            }
            dodgeBuffT--;
        }
        if (damageBuffed)
        {


            if (damageBuffT <= 0)
            {
                damageBuffed = false;
                attackPower -= dodgeBuffN;

            }
            damageBuffT--;
        }

        if (damageBuffed)
        {


            if (damageBuffT <= 0)
            {
                damageBuffed = false;
                attackPower -= damageBuffN;
                damageBuffN = 0;
            }
            damageBuffT--;
        }

        if (buffed)
        {


            if (buffTimer <= 0)
            {
                buffed = false;
                damageReduction -= dRbuff;
            }
            buffTimer--;
        }

        if (burning)
        {
            UnitDamage(burnDamage + damageReduction);
            burnTimer--;
            if (burnTimer == 0)
            {
                burning = false;
            }
        }
    }


    // Update is called once per frame

}
