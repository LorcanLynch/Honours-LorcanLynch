using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkHoundScript : UnitScript
{
    int dodges = 1;
    // Start is called before the first frame update
    public override void UnitDamage(float AP)
    {
        if (dodges < 1)
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
        else
        {
            dodges--;
            animator.SetTrigger("damage");
        }
    }

    // Update is called once per frame

}
