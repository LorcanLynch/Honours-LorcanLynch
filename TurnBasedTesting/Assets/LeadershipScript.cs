using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadershipScript : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
       
    }

    private void OnTriggernExit2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "team1")
        {
            if (!collision.gameObject.GetComponent<UnitScript>().beingLead)
                {
                if (gameObject.GetComponentInParent<KnightScript>().leader)
                {
                    collision.GetComponent<UnitScript>().beingLead = true;
                    collision.GetComponent<UnitScript>().attackPower++;
                    collision.GetComponent<UnitScript>().damageReduction++;
                }
                if(gameObject.GetComponentInParent<KnightScript>().lord)
                {
                    collision.GetComponent<UnitScript>().attackPower++;
                    collision.GetComponent<UnitScript>().damageReduction++;
                    collision.GetComponent<UnitScript>().leadByLord = true;
                }
                if (gameObject.GetComponentInParent<KnightScript>().adaptiveLord)
                {
                    if(gameObject.GetComponentInParent<KnightScript>().health < gameObject.GetComponentInParent<KnightScript>().maxhealth/2)
                    {
                        collision.GetComponent<UnitScript>().damageReduction++;
                    }
                    else
                    {
                        collision.GetComponent<UnitScript>().attackPower++;
                    }
                   
                }
            }

        }
        else if(collision.gameObject.tag == "Enemy")
        {
            if (gameObject.GetComponentInParent<KnightScript>().fearedLord)
            {
                if (!collision.gameObject.GetComponent<UnitScript>().fearedByLord)
                {
                    collision.GetComponent<UnitScript>().fearedByLord = true;
                    collision.GetComponent<UnitScript>().attackPower--;
                    collision.GetComponent<UnitScript>().damageReduction--;
                   
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "team1")
        {
            if (collision.gameObject.GetComponent<UnitScript>().beingLead)
            {
                if (gameObject.GetComponentInParent<KnightScript>().leader)
                {
                    collision.GetComponent<UnitScript>().beingLead = false;
                    collision.GetComponent<UnitScript>().attackPower--;
                    collision.GetComponent<UnitScript>().damageReduction--;
                }
                if (gameObject.GetComponentInParent<KnightScript>().lord)
                {
                    if (collision.GetComponent<UnitScript>().leadByLord)
                    {
                        collision.GetComponent<UnitScript>().attackPower--;
                        collision.GetComponent<UnitScript>().damageReduction--;
                    }
                   
                }
                if (gameObject.GetComponentInParent<KnightScript>().adaptiveLord)
                {
                    if (gameObject.GetComponentInParent<KnightScript>().health < gameObject.GetComponentInParent<KnightScript>().maxhealth / 2)
                    {
                        collision.GetComponent<UnitScript>().damageReduction--;
                    }
                    else
                    {
                        collision.GetComponent<UnitScript>().attackPower--;
                    }

                }
                collision.GetComponent<UnitScript>().leadByLord = false;
            }
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (gameObject.GetComponentInParent<KnightScript>().fearedLord)
            {
                if (collision.gameObject.GetComponent<UnitScript>().fearedByLord)
                {
                    collision.GetComponent<UnitScript>().fearedByLord = false;
                    collision.GetComponent<UnitScript>().attackPower++;
                    collision.GetComponent<UnitScript>().damageReduction++;

                }

               
            }
        }

    }
}
