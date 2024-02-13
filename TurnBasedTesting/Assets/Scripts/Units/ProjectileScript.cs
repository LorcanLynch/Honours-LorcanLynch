using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject target;
    float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Wizard");
    }

    // Update is called once per frame
    void Update()
    {
       
        if(timer > 10 || gameObject.transform.position == target.transform.position)
        {
            Destroy(gameObject);
        }
    }
}
