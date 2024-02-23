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
      
        upgradeContainers.Add("IncantationContainer");
        upgradeContainers.Add("FireballContainer");
        upgradeContainers.Add("LightningSurgeContainer");
       

        GenerateChoice();
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void SeekingBlastContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The wizard can no longer miss, but his damage is halved";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];

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
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The wizard's basic attacks now deal an extra 8 damage over 4 turns, but accuracy is reduced by 10";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[6];
   
        attachedButton.onClick.AddListener(delegate { FireLordOnClick(attachedButton); });

    }
    void FireLordOnClick(Button targetButton)
    {
        GameObject.Find("Wizard").GetComponent<WizardScript>().accuracy -=10;
        GameObject.Find("Wizard").GetComponent<WizardScript>().fireLord= true;
        upgradeContainers.Remove("FireLordContainer");
      
        AfterUpgradeApplied();
    }

    void IceLordContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard now has a chance to freeze enemies with his basic attacks, stunning them";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[5];
   
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
        attachedButton.GetComponent<Image>().sprite = abilitySprites[4];
 
        attachedButton.onClick.AddListener(delegate { BombardmentOnClick(attachedButton); });

    }
    void BombardmentOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().Bombardment = true;
       
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
        upgradeContainers.Add("MassBombardmentContainer");
        upgradeContainers.Add("BombardComboContainer");
     
        AfterUpgradeApplied();
    }

    void MassBombardmentContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Bombardment now hits in a small area around the target, dealing the same damage to all in the area";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[4];

        attachedButton.onClick.AddListener(delegate { BombardmentOnClick(attachedButton); });

    }
    void MassBombardmentOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().massBombardment = true;

        upgradeContainers.Remove("MassBombardmentContainer");
        
        AfterUpgradeApplied();
    }
    void PowerSurgeContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Surge now also grants +4 damage, and 2 additional move speed \n (This exceeds a character's max move speed)";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[1];

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
        attachedButton.GetComponent<Image>().sprite = abilitySprites[3];
 
        attachedButton.onClick.AddListener(delegate { LifeDrainOnClick(attachedButton); });

    }
    void LifeDrainOnClick(Button targetButton)
    {
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
        upgradeContainers.Add("SoulDrainContainer");
        upgradeContainers.Add("LifeDrainComboContainer");
        
        GameObject.Find("Wizard").GetComponent<WizardScript>().lifeDrain = true;
       
        AfterUpgradeApplied();
    }

    void SoulDrainContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Life drain applies a damage over time affect, dealing 100% of the abilities damage over 4 turns";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[3];

        attachedButton.onClick.AddListener(delegate { SoulDrainOnClick(attachedButton); });

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
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
     
        attachedButton.onClick.AddListener(delegate { IncantationOnClick(attachedButton); });

    }
    void IncantationOnClick(Button targetButton)
    {
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
        upgradeContainers.Add("SurgeIncantationContainer");
        upgradeContainers.Add("IncantationSurgeContainer");
        GameObject.Find("Wizard").GetComponent<WizardScript>().incantationOfPower = true;

        AfterUpgradeApplied();
    }

    void SurgeIncantationContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Incantation of Power now resets all allies attacks";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];

        attachedButton.onClick.AddListener(delegate { SurgeIncantationOnClick(attachedButton); });

    }
    void SurgeIncantationOnClick(Button targetButton)
    {
       
        upgradeContainers.Remove("SurgeIncantationContainer");

        GameObject.Find("Wizard").GetComponent<WizardScript>().greaterInvocation = true;

        AfterUpgradeApplied();
    }

    void FireballContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Fourth Abilty becomes Fireball, a large damage AoE.";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[6];
   
        attachedButton.onClick.AddListener(delegate { FireballOnClick(attachedButton); });

    }
    void FireballOnClick(Button targetButton)
    {
        upgradeContainers.Remove("FireballContainer");

        upgradeContainers.Remove("MassFreezeContainer");
        GameObject.Find("Wizard").GetComponent<WizardScript>().fireball = true;

        AfterUpgradeApplied();
    }

    void MassFreezeContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Fourth Abilty becomes Breath of Cold, a damaging AoE that stuns enemies it strikes.";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[5];

        attachedButton.onClick.AddListener(delegate { MassFreezeOnClick(attachedButton); });

    }
    void MassFreezeOnClick(Button targetButton)
    {
        upgradeContainers.Remove("MassFreezeContainer");
        upgradeContainers.Remove("FireballContainer");

        GameObject.Find("Wizard").GetComponent<WizardScript>().breathOfCold = true;

        AfterUpgradeApplied();
    }

    void LightningSurgeContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's lightning strike can now combo with power surge, granting lightning strike + 4 damage if surge is used first, and granting the target +4 damage that turn if lightning strike is used first.";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
       
        attachedButton.onClick.AddListener(delegate { LightningSurgeOnClick(attachedButton); });

    }
    void LightningSurgeOnClick(Button targetButton)
    {
        upgradeContainers.Remove("LightningSurgeContainer");


        GameObject.Find("Wizard").GetComponent<WizardScript>().lightningSurgeCombo = true;

        AfterUpgradeApplied();
    }

    void IncantationSurgeContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Incantation can now combo with power surge, Reducing both cooldowns by 1 when comboed";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
      
        attachedButton.onClick.AddListener(delegate { IncantationSurgeOnClick(attachedButton); });

    }
    void IncantationSurgeOnClick(Button targetButton)
    {
        upgradeContainers.Remove("IncantationSurgeContainer");


        GameObject.Find("Wizard").GetComponent<WizardScript>().combos[2] = true;

        AfterUpgradeApplied();
    }

    void LifeDrainComboContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Life Drain can now combo with power surge, Granting +5 damage to life drain when comboed, and healing the target for 5 when surge is comboed";

        attachedButton.GetComponent<Image>().sprite = abilitySprites[3];
    
        attachedButton.onClick.AddListener(delegate { LifeDrainComboOnClick(attachedButton); });

    }
    void LifeDrainComboOnClick(Button targetButton)
    {
        upgradeContainers.Remove("LifeDrainComboContainer");


        GameObject.Find("Wizard").GetComponent<WizardScript>().combos[1] = true;

        AfterUpgradeApplied();
    }

    void BombardComboContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Life Drain can now combo with Bombardment, Granting +5 damage to Bombardment when comboed, and granting +2 movespeed to the target when surge is comboed";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[4];
     
        attachedButton.onClick.AddListener(delegate { BombardComboOnClick(attachedButton); });

    }
    void BombardComboOnClick(Button targetButton)
    {
        upgradeContainers.Remove("BombardComboContainer");


        GameObject.Find("Wizard").GetComponent<WizardScript>().combos[0] = true;

        AfterUpgradeApplied();
    }

}

