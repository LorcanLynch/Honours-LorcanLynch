using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
public class BackDropScript : MonoBehaviour
{

    public Image[] baseIcons = { null, null, null, null };
    GameObject attackReadyIcon = null;
    public Image LockIcon;
    public Image[] newIcons = { null, null, null, null };
    // Start is called before the first frame update
    void Start()
    {
        
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newUnit(Image[] newIcons, int[] cooldown)
    {
        
        for(int i = 0; i < baseIcons.Length; i++)
        {
            if (newIcons[i] != null)
            {
                baseIcons[i].GetComponent<Image>().sprite = newIcons[i].sprite;
                baseIcons[i].GetComponent<AbilityIconScript>().cooldownPanel(cooldown[i]);
                
                
            }
            else
            {
                baseIcons[i].GetComponent<Image>().sprite = LockIcon.sprite;
            }
        }

       
        

        
    }
}
