using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderScript : MonoBehaviour
{
    GameObject unit;
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponentInParent<UnitScript>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Slider>().value = (unit.GetComponent<UnitScript>().health/ unit.GetComponent<UnitScript>().maxhealth);
    }
}
