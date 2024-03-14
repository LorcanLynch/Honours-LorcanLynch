using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHolder : MonoBehaviour
{
    public int upgraded = 0;
    ObjectiveScript tileMapObj;
    public List<string> upgradesUnlocked;
    // Start is called before the first frame update
    void Start()
    {
        tileMapObj = GameObject.Find("Map").GetComponent<ObjectiveScript>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newLevel()
    {
        
            gameObject.SetActive(false);
           
            tileMapObj.StartNewLevel();
        
    }
}
