


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class KnightScript : UnitScript
{
    // Start is called before the first frame update
    public bool parryStrikeback;
    public bool parryDamage;
    public bool parryHoldfast;
    public bool parryStrikebackF;

    public bool lordlyGreatsword;
    public bool domumHalberd;
    public AudioClip parrySound;
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

    public int leadershipRange = 1;

    public bool berserk;
    public bool berserkA;

    public bool leader;
    public bool lord;
    public bool fearedLord;
    public bool adaptiveLord;
    public bool deathToFoes;
    public override void Start()
    {

        base.Start();
        abilitiesRange = new List<int> { 1, 2, 3, 5, 5, 5, 5, 5, 5, 5 };
        hotKeysString = new List<string> { "Cleave","Print" };
}


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
                text.GetComponent<DamageTextScript>().UpdateText(Mathf.RoundToInt(Mathf.Clamp(AP - damageReduction, 1, 100) * -1).ToString());
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
                aSource.PlayOneShot(hitEffect);
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

                if(target.GetComponent<UnitScript>().health <= 0)
                {
                    if(berserk)
                    {
                        berserkA = true;
                    }
                }
            }
            else
            {
                text.GetComponent<DamageTextScript>().UpdateText("Miss");
                aSource.PlayOneShot(missEffect);
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
   
    //private void LateUpdate()
    //{
    //    RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, 5f * leadershipRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
    //    foreach (RaycastHit2D hit in targets)
    //    {
    //        if (hit.collider.tag == "team1")
    //        {
    //            if (leadByLord)
    //            {
    //                hit.collider.GetComponent<UnitScript>().leadByLord = false;
    //                hit.collider.GetComponent<UnitScript>().attackPower--;
    //                hit.collider.GetComponent<UnitScript>().damageReduction--;
                    
                   
                
    //            }
    //            if (beingLead)
    //            {
    //                hit.collider.GetComponent<UnitScript>().beingLead = false;
    //                hit.collider.GetComponent<UnitScript>().attackPower--;
    //                hit.collider.GetComponent<UnitScript>().damageReduction--;
                   
    //            }
               
    //        }
    //    }
    //    targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f* leadershipRange , new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
    //    foreach (RaycastHit2D hit in targets)
    //    {
    //        if (hit.collider.tag == "team1")
    //        {
    //            if(!beingLead)
    //            {
    //                hit.collider.GetComponent<UnitScript>().attackPower++;
    //                hit.collider.GetComponent<UnitScript>().damageReduction++;
    //                hit.collider.GetComponent<UnitScript>().beingLead = true;
    //            }
               
    //            if(gloryActivated > 0)
    //            {
    //                if (!leadByLord)
    //                {
    //                    hit.collider.GetComponent<UnitScript>().attackPower++;
    //                    hit.collider.GetComponent<UnitScript>().damageReduction++;
    //                    hit.collider.GetComponent<UnitScript>().leadByLord = true;
    //                }
                   
    //            }
              
    //        }
    //    }

       
    //}
    public override void Ability1(GameObject targetUnit)
    {
        ///<summary>
        ///Allows the knight to use it's cleave abiltiy
        /// </summary>
        aSource.PlayOneShot(hitEffect);
        animator.SetTrigger("attack");
        abilitiesCooldown[0] = 4;
        attackAvailable = false;
        targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower);//simple attack
        RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, 1f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
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
        if (attackAvailable)
        {
            aSource.PlayOneShot(parrySound);
            attackAvailable = false;

            abilitiesCooldown[1] = 5;
            DRBuff(3, 3);
            DodgeBuff(3,(int)dodgeRating);
            if (parryDamage)
            {
                parryStrikeback = true;
            }
            if (parryHoldfast)
            {
                DodgeBuff(3, (int)dodgeRating * 3);
                abilitiesCooldown[1] = 5;
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
                    DRBuff(3,6);
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
                    buffTimer++;
                    dodgeBuffT++;
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




            
            map.UpdateCooldowns(gameObject);
            abilitiesTarget[1] = false;
        }
    }

    public override void Ability3(GameObject targetUnit)
    {
        
        
        if(frenziedStrike)
        {
            if (attackAvailable)
            {
                if (CheckAttackDistance(targetUnit.GetComponent<UnitScript>().tileX, targetUnit.GetComponent<UnitScript>().tileY) && targetUnit.tag == "Enemy")
                {
                    attack(targetUnit);
                    attackAvailable = true;
                    attack(targetUnit);
                    attackAvailable = true;
                    attack(targetUnit);
                    attackAvailable = true;
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
                            attackAvailable = true;
                            attack(targetUnit);
                            
                        }

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
            aSource.PlayOneShot(parrySound);

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
            aSource.PlayOneShot(parrySound);
            litBlade = 3;
            abilitiesCooldown[2] = 7;
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
            aSource.PlayOneShot(parrySound);
            abilitiesCooldown[2] = 6;
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
    //public override void AbilityTarget(int abilityIndex)
    //{
    //    if (abilityIndex == 3)
    //    {
    //        return;
    //    }
    //    int range = attackRange;
    //    for (int i = 0; i < 4; i++)
    //    {
    //        if (i == abilityIndex)
    //        {
               
    //            abilitiesTarget[abilityIndex] = !abilitiesTarget[abilityIndex];
    //        }
    //        else
    //        {
    //            abilitiesTarget[i] = false;
    //        }
    //    }
    //    if (unstopableForce && abilityIndex == 2)
    //    {
    //        range = 0;
    //    }
    //    if (flamingBlade && abilityIndex == 2)
    //    {
    //        range = 0;
    //    }
    //    if (righteousGlory && abilityIndex == 2)
    //    {
    //        range = 2;
    //    }
    //    if ( abilityIndex == 1)
    //    {
    //        range = 0;
    //    }


    //    attacking = false;


    //    targetting(range);
    //    map.UpdateIconSelection(abilityIndex);

    //    if (abilitiesTarget[abilityIndex] == false)
    //    {
    //        CancelTarggeting();
    //    }

    //}
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

        if (gloryActivated > 0)
        {
            
            gloryActivated--;
           
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

    public void Cleave()
    {
        aSource.PlayOneShot(hitEffect);
        animator.SetTrigger("attack");
        
        attackAvailable = false;
        targetUnit.GetComponent<UnitScript>().UnitDamage(attackPower/2);//simple attack
        RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, 1f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
        foreach(RaycastHit2D target in targets)
        {
            if(target.collider.tag != gameObject.tag)
            {
                target.collider.gameObject.GetComponent<UnitScript>().UnitDamage(attackPower);
                if (target.collider.gameObject.GetComponent<UnitScript>().health <= 0)
                {
                    if (berserk)
                    {
                        berserkA = true;
                    }
                }
            }
            
        }
        
        targetUnit = null;
        map.UpdateCooldowns(gameObject);
        for(int i = 0; i<abilitiesTargetting.Count; i++)
        {
            abilitiesTargetting[i] = false;
        }
    }

    public void Taunt()
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, 1f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
        foreach (RaycastHit2D target in targets)
        {
            if (target.collider.tag != gameObject.tag)
            {
                target.collider.gameObject.GetComponent<UnitScript>().taunted = true;
                target.collider.GetComponent<UnitScript>().tauntTarget = gameObject;
            }

        }
        
      
        for (int i = 0; i < abilitiesTargetting.Count; i++)
        {
            abilitiesTargetting[i] = false;
        }
    }

    public void FrenziedStrikes()
    {
                    attack(targetUnit);
                    attackAvailable = true;
                    attack(targetUnit);
                    attackAvailable = true;
                    attack(targetUnit);
                    attackAvailable = true;
                    abilitiesCooldown[2] = 4;
                    attackAvailable = false;
                    riposteCombo = false;
                    targetUnit = null;
    }

    public void PreparedForWar()
    {
        
            aSource.PlayOneShot(parrySound);
            litBlade = 3;
            abilitiesCooldown[2] = 7;

        targetUnit = null;

        for (int i = 0; i < abilitiesTargetting.Count; i++)
        {
            abilitiesTargetting[i] = false;
        }


    }

    public void UnstopableForce()
    {
      
            unstopableWarrior = 2;
            HealDamage(Random.Range(1, 5));
            abilitiesCooldown[2] = 4;
            attackAvailable = false;
            riposteCombo = false;
            aSource.PlayOneShot(parrySound);
            targetUnit = null;

        for (int i = 0; i < abilitiesTargetting.Count; i++)
        {
            abilitiesTargetting[i] = false;
        }


    }

    public void RighteousGlory()
    {
        gloryActivated = 3;
        targetUnit = null;
    }
    // Update is called once per frame

}
