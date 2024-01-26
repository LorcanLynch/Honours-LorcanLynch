using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class KnightUpgrades : Upgrades
{

    
    // Start is called before the first frame update
    void Start()
    {
        upgradeContainers.Add("HalberdContainer");
        
        upgradeContainers.Add("ParryStrikebackContainer");
        upgradeContainers.Add("ParryDodgeChanceContainer");
        upgradeContainers.Add("FrenziedContainer");
        upgradeContainers.Add("GloryContainer");
        upgradeContainers.Add("UnstopableContainer");
        GenerateChoice();
    }


    // Update is called once per frame
    void Update()
    {
      

    }
     

    public void HalberdContainer()
    {
       attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "+1 range, and cleave now sweeps \n all in range, stunning them for one turn";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
       attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { HalberdOnClick(attachedButton); });
       
    }
    

    void HalberdOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().domumHalberd = true;
        GameObject.Find("Knight").GetComponent<KnightScript>().attackRange++;
        upgradeContainers.Remove("HalberdContainer");
        AfterUpgradeApplied();
    }

    void ParryStrikebackContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "After Activating Parry, the first time the knight is struck they take no damage and strike all enemies in range";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { ParryStrikeOnClick(attachedButton); });

    }
    void ParryStrikeOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().parryDamage = true;

        upgradeContainers.Remove("ParryStrikebackContainer");
        AfterUpgradeApplied();
    }

    void ParryDodgeChanceContainer()
    {
        
        attachedButton.GetComponent<Image>().sprite = abilitySprites[1];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Parry now increases dodge by 4x(Previously 2x), and its cooldown is one turn shorter";
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { ParryHoldfastOnClick(attachedButton); });
        

    }

    void ParryHoldfastOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().parryHoldfast = true;
        upgradeContainers.Remove("ParryDodgeChanceContainer");
        targetButton.gameObject.SetActive(false);
        AfterUpgradeApplied();
    }

    void FrenziedContainer()
    {
        
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 3rd ability becomes 'Frenzied Strike', which attacks the same target 3 times in quick succession";
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { FrenziedOnClick(attachedButton); });
        
    }

    void FrenziedOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().frenziedStrike = true;
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
        AfterUpgradeApplied();
    }

    void GloryContainer()
    {
        
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 3rd ability becomes 'Glory or Death', Granting +2 Damage reduction to all friendly units within 2 tiles";
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { GloryOnClick(attachedButton); });
        
    }
    void GloryOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().righteousGlory = true;
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
        AfterUpgradeApplied();
    }

    void UnstopableContainer()
    {
        
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 3rd ability becomes 'Unstopable Force', Causing them to take no damage until their next turn";

        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { UnstopableOnClick(attachedButton); });
    }
    void UnstopableOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().unstopableForce = true;
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
        AfterUpgradeApplied();
    }
}
