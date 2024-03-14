using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeTextScript : MonoBehaviour
{
    public List<string> upgrades;
    public UpgradeHolder upgradeScript;
    // Start is called before the first frame update
    
    private void OnEnable()
    {
        upgrades = upgradeScript.upgradesUnlocked;
        TextMeshProUGUI t = GetComponent<TextMeshProUGUI>();
        t.text = "Upgrades Unlocked: ";
        foreach (string upgrade in upgrades)
        {
            t.text = t.text + "\n" + upgrade;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
