using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBoltScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 target = Vector2.zero;
    void Start()
    {
        //gameObject.transform.rotation = Quaternion.FromToRotation(transform.position, -target);
        transform.right = transform.position - target;
        GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(target - transform.position) * Time.deltaTime * 600 ;
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
       // transform.position = Vector2.MoveTowards(transform.position, target, 5 * Time.deltaTime);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != gameObject)
        {
            collision.gameObject.GetComponent<UnitScript>().UnitDamage(100);
        }
    }
}
