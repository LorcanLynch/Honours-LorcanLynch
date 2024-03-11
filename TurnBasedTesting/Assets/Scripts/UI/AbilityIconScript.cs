using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AbilityIconScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panel;
    public TextMeshProUGUI text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cooldownPanel(int cooldown)
    {
        if (cooldown > 0)
        {
            panel.SetActive(true);
            text.text = cooldown.ToString();
        }
        else
        {
            panel.SetActive(false);
            text.text = "";
        }
    }
}
