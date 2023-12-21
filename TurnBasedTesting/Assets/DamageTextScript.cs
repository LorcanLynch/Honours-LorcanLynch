using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DamageTextScript : MonoBehaviour
{
    float timer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 1f;

    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        if(timer > .8f)
        {
            
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.transform.localPosition += new Vector3(0, .2f);
        }
    }

    public void UpdateText(string text)
    {
        transform.localPosition = new Vector3(0, 20, 0);
        gameObject.SetActive(true);
        gameObject.GetComponent<TextMeshProUGUI>().text = text;
        timer = 0f;
    }
    
    
}
