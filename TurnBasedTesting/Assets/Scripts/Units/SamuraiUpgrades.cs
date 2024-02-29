using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
public class SamuraiUpgrades : Upgrades
{
    
    // Start is called before the first frame update
    void Start()
    {
        upgradeContainers.Add("AlertExecutionContainer");
        upgradeContainers.Add("TheThrillContainer");
        upgradeContainers.Add("EndlessWindContainer");
        upgradeContainers.Add("FocusContainer");
        upgradeContainers.Add("ExploitWeaknessContainer");
        upgradeContainers.Add("ShurikenContainer");
        upgradeContainers.Add("BloodthirstContainer");
        upgradeContainers.Add("ClearMindContainer");
        //upgradeContainers.Add("TrueStrikeContainer");
        upgradeContainers.Add("EndlessWindContainer");
        GenerateChoice();
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void AlertExecutionContainer()
    {
        
        GameObject enemy = GenerateEnemy();
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's execute now grants +40 dodge for one turn if it kills it's target, additionally he gains +5 dodge";
      
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
      
        attachedButton.onClick.AddListener(delegate { AlertExecutionOnClick(attachedButton); });

    }

    void AlertExecutionOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().agileX = true;
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().dodgeRating +=5;
        upgradeContainers.Remove("AlertExecutionerContainer");

        AfterUpgradeApplied();
    }

    public void TrueStrikeContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Samurai's first attack each turn always hits, Additionally he gains +1 attack power";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
      
        attachedButton.onClick.AddListener(delegate { TrueStrikeOnClick(attachedButton); });

    }

    void TrueStrikeOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().trueStrike = true;
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().attackPower++;
        upgradeContainers.Remove("AlertExecutionerContainer");

        AfterUpgradeApplied();
    }

    public void TheThrillContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's execute refunds its attack if it kills its target.";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        
        attachedButton.onClick.AddListener(delegate { TheThrillOnClick(attachedButton); });

    }

    void TheThrillOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().thrill = true;
        
        upgradeContainers.Remove("TheThrillContainer");

        AfterUpgradeApplied();
    }

    void EndlessWindContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Once the Samurai activates dash, for that turn he gains +40 dodge";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[1];
    
        attachedButton.onClick.AddListener(delegate { EndlessWindOnClick(attachedButton); });

    }
    void EndlessWindOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().thrill = true;

        upgradeContainers.Remove("TheThrillContainer");

        AfterUpgradeApplied();
    }

    public void FocusContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's 3rd ability becomes focus, a free action buff granting a small bonus to all attack stats, and 10 dodge chance";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
      
        attachedButton.onClick.AddListener(delegate { FocusOnClick(attachedButton); });

    }

    void FocusOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().focus = true;
        GameObject.Find("Samurai").GetComponent<UnitScript>().abilityIcons[2] = attachedButton.GetComponent<Image>().sprite;
        upgradeContainers.Remove("FocusContainer");
        upgradeContainers.Add("HarmonyContainer");
        upgradeContainers.Add("FocusDashContainer");
        AfterUpgradeApplied();
    }

    public void HarmonyContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's Focus now heals the samurai to full health, but costs a full action";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
       
        attachedButton.onClick.AddListener(delegate { HarmonyOnClick(attachedButton); });

    }

    void HarmonyOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().harmony = true;

        upgradeContainers.Remove("HarmonyContainer");

        AfterUpgradeApplied();
    }

    public void ExploitWeaknessContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's 3rd ability becomes exploit weakness, granting them the abilty to ignore armor for the next 2 turns";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[3];
      
        attachedButton.onClick.AddListener(delegate { ExploitWeaknessOnClick(attachedButton); });
        
    }

    void ExploitWeaknessOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().exploit = true;
        GameObject.Find("Samurai").GetComponent<UnitScript>().abilityIcons[2] = attachedButton.GetComponent<Image>().sprite;
        upgradeContainers.Remove("ExploitWeaknessContainer");
        upgradeContainers.Add("ExploitDashContainer");
        AfterUpgradeApplied();
    }

    public void ShurikenContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's 3rd ability becomes Shuriken Throw, a basic attack with +2 range.";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[4];
      
        attachedButton.onClick.AddListener(delegate { ShurikenOnClick(attachedButton); });

    }

    void ShurikenOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().shuriken = true;
        GameObject.Find("Samurai").GetComponent<UnitScript>().abilityIcons[2] = attachedButton.GetComponent<Image>().sprite;
        upgradeContainers.Remove("ShurikenContainer");
        upgradeContainers.Add("ShreddingShurikenContainer");
        upgradeContainers.Add("ShurikenDashContainer");
        AfterUpgradeApplied();
    }

    public void ShreddingShurikenContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's Shuriken throw now bleeds the target, dealing 9 additional damage over 3 turns";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[4];
       
        attachedButton.onClick.AddListener(delegate { ShreddingShurikenOnClick(attachedButton); });

    }

    void ShreddingShurikenOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().shreddingShuriken = true;

        upgradeContainers.Remove("ShreeddingShurikenContainer");

        AfterUpgradeApplied();
    }

    public void BloodthirstContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's 4th ability becomes bloodthist, a free action buff that resets his attack on any of his kills";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[5];
     
        attachedButton.onClick.AddListener(delegate { BloodthirstOnClick(attachedButton); });

    }

    void BloodthirstOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().bloodThirst = true;
        GameObject.Find("Samurai").GetComponent<UnitScript>().abilityIcons[3] = attachedButton.GetComponent<Image>().sprite;
        upgradeContainers.Remove("BloodthirstContainer");
        upgradeContainers.Remove("ClearMindContainer");

        AfterUpgradeApplied();
    }

    public void ClearMindContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's 4th ability becomes Clear Mind, a free action that resets all ability cooldowns";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[6];
      
        attachedButton.onClick.AddListener(delegate { ClearMindOnClick(attachedButton); });

    }

    void ClearMindOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().clearMind = true;
        GameObject.Find("Samurai").GetComponent<UnitScript>().abilityIcons[3] = attachedButton.GetComponent<Image>().sprite;
        upgradeContainers.Remove("BloodthirstContainer");
        upgradeContainers.Remove("ClearMindContainer");

        AfterUpgradeApplied();
    }


    public void SwiftExecutionerContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Execute can now combo with dash, Restoring a large amount of health on execute kill, or a smaller amount if dash is used first";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
       
        attachedButton.onClick.AddListener(delegate { SwiftExecutionerOnClick(attachedButton); });

    }

    void SwiftExecutionerOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().swiftExe = true;

        upgradeContainers.Remove("SwiftExecutionerContainer");
       

        AfterUpgradeApplied();
    }

    public void ShurikenDashContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Shuriken Throw can now combo with dash, Granting +4 damage if dash is used first, and +2 movespeed if Shuriken throw is used first ";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[4];
      
        attachedButton.onClick.AddListener(delegate { ShurikenDashOnClick(attachedButton); });

    }

    void ShurikenDashOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().abilityCombos[2] = true;

        upgradeContainers.Remove("ShurikenDashContainer");

        AfterUpgradeApplied();
    }

    public void ExploitDashContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Exploit weakness can now combo with Dash, granting an extra +2 movespeed if exploit weakness is used first, and +1 move speed per turn if dash is used first";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[3];
      
        attachedButton.onClick.AddListener(delegate { ExploitDashOnClick(attachedButton); });

    }

    void ExploitDashOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().abilityCombos[1] = true;

        upgradeContainers.Remove("ExploitDashContainer");
       

        AfterUpgradeApplied();
    }

    public void FocusDashContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Focus can now combo with Dash, doubling its duration if dash is used first, and reducing the cooldown of both abilities by 1 if focus is used first";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
    
        attachedButton.onClick.AddListener(delegate { FocusDashOnClick(attachedButton); });

    }

    void FocusDashOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().abilityCombos[0] = true;

        upgradeContainers.Remove("FocusDashContainer");
        

        AfterUpgradeApplied();
    }

    // Update is called once per frame

}
