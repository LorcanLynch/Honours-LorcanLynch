using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SkeletonLord : EnemyScript
{

    public int bodiesCD = 0;
    public int bonesCD = 0;
    public int expulsionCD = 0;
    GameObject unitTarget;
    // Start is called before the first frame update
    

    // Update is called once per frame
   
    public override bool turnStart()
    {
        if(bonesCD <= 0)
        {
            BonesToBlades();
        }
        else if(expulsionCD <= 0 )
            {
            NecromanticExpulsion();
        }
        else if(bodiesCD <= 0 && gameObject.GetComponent<UnitScript>().health < gameObject.GetComponent<UnitScript>().maxhealth/2)
        {
            BodiesToBolster();
        }
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("team1");
        float targetDistance = 100;
        foreach (GameObject target in playerUnits)
        {
            if (map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileX) == null)

            {
                targetDistance = 0;
                unitTarget = target;

            }
            else if (map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileX).Count < targetDistance)
            {
                targetDistance = map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileX).Count;
                unitTarget = target;
            }
        }
        List<Node> possiblePath = map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileX);

        if (possiblePath != null && possiblePath.Count - 2 < aggroRange)
        {

            gameObject.GetComponent<UnitScript>().EnterCourse(unitTarget.GetComponent<UnitScript>().tileX, unitTarget.GetComponent<UnitScript>().tileY, possiblePath);
            gameObject.GetComponent<UnitScript>().EnterCourse(unitTarget.GetComponent<UnitScript>().tileX, unitTarget.GetComponent<UnitScript>().tileY, possiblePath);
        }


        return true;
       
    }
    void BonesToBlades()
    {
        for (int i = 0; i < 4; i++)
        {
            map.gm.SpawnUnit(gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY);
        }
        gameObject.GetComponentInChildren<Animator>().SetTrigger("spell");
        bonesCD =3;

    }

    void BodiesToBolster()
    {
       for(int i = 0; i <map.Units.Count;i++)
        {
            if (map.Units[i].tag == "Enemy")
            {
                map.Units[i].GetComponent<UnitScript>().UnitDamage(5 + map.Units[i].GetComponent<UnitScript>().damageReduction);
                gameObject.GetComponent<UnitScript>().health += 5;
            }
        }
        gameObject.GetComponentInChildren<Animator>().SetTrigger("spell");
        bodiesCD = 5;
    }

    public void NecromanticExpulsion()
    {
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("team1");
        float targetDistance = 0;
        foreach (GameObject target in playerUnits)
        {
            if (map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileX) == null)

            {
                targetDistance = 0;
                unitTarget = target;

            }
            else if (map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileX).Count > targetDistance)
            {
                targetDistance = map.GenerateMovePath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileY, target.GetComponent<UnitScript>().tileX).Count;
                unitTarget = target;
            }
        }
        if(map.GenerateAttackPath(gameObject, gameObject.GetComponent<UnitScript>().tileX, gameObject.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileY, unitTarget.GetComponent<UnitScript>().tileX).Count  < 7)
        {
            RaycastHit2D[] blast = Physics2D.CircleCastAll(unitTarget.transform.position, 1.6f, new Vector2(0, 0));//creates a circle around the unit and damages each unit in it
            foreach (RaycastHit2D hits in blast)
            {
                if (hits.collider.tag == "team1")
                {
                    hits.collider.GetComponent<UnitScript>().UnitDamage(10);
                }
            }

        }
        gameObject.GetComponentInChildren<Animator>().SetTrigger("spell");
        expulsionCD = 5;
    }



}
