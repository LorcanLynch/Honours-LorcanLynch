using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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

    public void ClearSelection ()
    {

        for (int i = 0; i < baseIcons.Length; i++)
        {
            
            
                baseIcons[i].GetComponent<Image>().color = Color.white;
            }

        



    }
    public void IconSelection(int index )
    {
        for (int i = 0; i < baseIcons.Length; i++)
        {
            if (index != i)
            {
                baseIcons[i].GetComponent<Image>().color = Color.white;
            }
        }

        if (baseIcons[index].GetComponent<Image>().color == Color.white)
        {
            baseIcons[index].GetComponent<Image>().color = Color.red;
        }
        else 
        {
            baseIcons[index].GetComponent<Image>().color = Color.white;
        }

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
