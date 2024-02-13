using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyScript : EnemyScript
{
    public GameObject projectile_;
    // Start is called before the first frame update
    public override void FinishedMove()
    {
        if (unitTarget != null)
        {
            if (GetComponent<UnitScript>().attackAvailable)
            {
                List<Node> possiblePath = map.GenerateAttackPath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileX);
                if (possiblePath.Count - 1 < GetComponent<UnitScript>().attackRange)
                {
                    GetComponent<UnitScript>().attack(unitTarget);
                    GameObject projectile = Instantiate(projectile_, gameObject.transform.position, Quaternion.identity);
                    projectile.GetComponent<Rigidbody2D>().velocity = unitTarget.transform.position - gameObject.transform.position;
                    projectile.GetComponent<ProjectileScript>().target = unitTarget;
                }
            }
        }
    }
}
