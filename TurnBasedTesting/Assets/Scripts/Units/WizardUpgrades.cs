using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class WizardUpgrades : Upgrades
{ 

    // Start is called before the first frame update
    void Start()
    {
        upgradeContainers.Add("SeekingBlastContainer");
        upgradeContainers.Add("FireLordContainer");
        upgradeContainers.Add("IceLordContainer");
        upgradeContainers.Add("BombardmentContainer");
        upgradeContainers.Add("PowerSurgeContainer");
        upgradeContainers.Add("LifeDrainContainer");
        upgradeContainers.Add("SoulDrainContainer");
        upgradeContainers.Add("IncantationContainer");
        upgradeContainers.Add("FireballContainer");
        GenerateChoice();
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void SeekingBlastContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The wizard can no longer miss, but his damage is halved";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { SeekingBlastOnClick(attachedButton); });

    }


    void SeekingBlastOnClick(Button targetButton)
    {
        GameObject.Find("Wizard").GetComponent<WizardScript>().accuracy = 999;
        GameObject.Find("Wizard").GetComponent<WizardScript>().attackPower = Mathf.RoundToInt(GameObject.Find("Wizard").GetComponent<WizardScript>().attackPower / 2);
        upgradeContainers.Remove("SeekingBlastContainer");
       
        AfterUpgradeApplied();
    }

    void FireLordContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The wizard's basic attacks now deal +2 damage and apply a 4 Turn Burn, dealing damage over time \n but accuracy is reduced by 10";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { FireLordOnClick(attachedButton); });

    }
    void FireLordOnClick(Button targetButton)
    {
        GameObject.Find("Wizard").GetComponent<WizardScript>().accuracy -=10;
        GameObject.Find("Wizard").GetComponent<WizardScript>().fireLord= true;
        upgradeContainers.Remove("FirelordContainer");
      
        AfterUpgradeApplied();
    }

    void IceLordContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard now has a chance to freeze enemies with his basic attacks, stunning them";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { IceLordOnClick(attachedButton); });

    }
    void IceLordOnClick(Button targetButton)
    {
        
        GameObject.Find("Wizard").GetComponent<WizardScript>().iceLord = true;
        upgradeContainers.Remove("IceLordContainer");
        AfterUpgradeApplied();
    }

    void BombardmentContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Third Abilty becomes Bombardment, a low damage ability but with extreme range";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { BombardmentOnClick(attachedButton); });

    }
    void BombardmentOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().Bombardment = true;
       
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
        AfterUpgradeApplied();
    }
    void PowerSurgeContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Surge now also grants +4 damage, and 2 additional move speed \n (This exceeds a character's max move speed)";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { PowerSurgeOnClick(attachedButton); });

    }
    void PowerSurgeOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().PowerSurge = true;
        upgradeContainers.Remove("PowerSurgeContainer");
        AfterUpgradeApplied();
    }
    void LifeDrainContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Third Abilty becomes life drain, a standard attack that heals \n the wizard for the damage done";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { LifeDrainOnClick(attachedButton); });

    }
    void LifeDrainOnClick(Button targetButton)
    {
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
        upgradeContainers.Add("SoulDrainContainer");
        GameObject.Find("Wizard").GetComponent<WizardScript>().lifeDrain = true;
       
        AfterUpgradeApplied();
    }

    void SoulDrainContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Life drain applies a damage over time affect, dealing 100% of the abilities damage over 4 turns";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { LifeDrainOnClick(attachedButton); });

    }
    void SoulDrainOnClick(Button targetButton)
    {
        
        upgradeContainers.Remove("SoulDrainContainer");
        GameObject.Find("Wizard").GetComponent<WizardScript>().soulDrain = true;

        AfterUpgradeApplied();
    }


    void IncantationContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Third Abilty becomes Incantation of Power, an AoE buff that increases damage done \n of all nearby allies";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { IncantationOnClick(attachedButton); });

    }
    void IncantationOnClick(Button targetButton)
    {
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
       
        GameObject.Find("Wizard").GetComponent<WizardScript>().incantationOfPower = true;

        AfterUpgradeApplied();
    }

    void FireballContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Fourth Abilty becomes Fireball, a large damage AoE.";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { FireballOnClick(attachedButton); });

    }
    void FireballOnClick(Button targetButton)
    {
        upgradeContainers.Remove("FireballContainer");
        

        GameObject.Find("Wizard").GetComponent<WizardScript>().fireball = true;

        AfterUpgradeApplied();
    }



}

