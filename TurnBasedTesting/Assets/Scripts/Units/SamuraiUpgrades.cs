using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
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

        GenerateChoice();
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void AlertExecutionContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's execute now grants +40 dodge for one turn if it kills it's target, additionally he gains +5 dodge";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { AlertExecutionOnClick(attachedButton); });

    }

    void AlertExecutionOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().agileX = true;
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().dodgeRating +=5;
        upgradeContainers.Remove("AlertExecutionerContainer");

        AfterUpgradeApplied();
    }

    public void TheThrillContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's execute refunds its attack if it kills its target. +2 execute damage";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
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
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Once the Samurai activates dash, for that turn any time he  kills a target he gains an extra 2 movement";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
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
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { FocusOnClick(attachedButton); });

    }

    void FocusOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().focus = true;

        upgradeContainers.Remove("FocusContainer");

        AfterUpgradeApplied();
    }

    public void ExploitWeaknessContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's 3rd ability becomes exploit weakness, granting them the abilty to ignore armor piercing for the next 2 turns";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { ExploitWeaknessOnClick(attachedButton); });

    }

    void ExploitWeaknessOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().exploit = true;

        upgradeContainers.Remove("ExploitWeaknessContainer");

        AfterUpgradeApplied();
    }

    public void ShurikenContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Samurai's 3rd ability becomes Shuriken Throw, a ranged attack that causes the target to bleed for 2 turns";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { ShurikenOnClick(attachedButton); });

    }

    void ShurikenOnClick(Button targetButton)
    {
        GameObject.Find("Samurai").GetComponent<SamuraiScript>().shuriken = true;

        upgradeContainers.Remove("ShurikenContainer");

        AfterUpgradeApplied();
    }


    // Update is called once per frame

}
