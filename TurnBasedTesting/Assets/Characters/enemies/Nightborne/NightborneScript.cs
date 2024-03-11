using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightborneScript : UnitScript
{
    public override void UnitDamage(float AP)
    {
        /// <summary>
        /// Causes the unit to take damage
        /// Starts by reducing it's health by the damage dealt factoring in the Unit's damage reduction, and clamping it so it cant be lower than one
        /// Then if it's health is below 0 it kills the unit by destroying it
        /// otherwise we simply play the hit animation.
        /// </summary>

        health -= Mathf.Clamp(AP - damageReduction, 1, 100);


        text.GetComponent<DamageTextScript>().UpdateText(Mathf.RoundToInt((AP - damageReduction) * -1).ToString());
        if (health <= 0)
        {
            animator.SetTrigger("death");
            map.GetComponent<TileMap>().RemoveUnit(gameObject);
           
                RaycastHit2D[] targets = Physics2D.CircleCastAll(gameObject.transform.position, .8f * attackRange, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
                foreach (RaycastHit2D hit in targets)
                {
                    if (hit.collider.tag == "team1")
                    {
                        hit.collider.gameObject.GetComponent<UnitScript>().UnitDamage(Mathf.Round(4));

                    }
                }
            
            Destroy(gameObject, 1.2f);

        }
        else
        {

            animator.SetTrigger("damage");
        }
    }
}
