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
        upgradeContainers.Add("RawGritContainer");
        upgradeContainers.Add("HardenedWarriorContainer");
        upgradeContainers.Add("ParryStrikebackContainer");
        upgradeContainers.Add("ParryDodgeChanceContainer");
        upgradeContainers.Add("FrenziedContainer");
        upgradeContainers.Add("GloryContainer");
        upgradeContainers.Add("UnstopableContainer");
        upgradeContainers.Add("RiposteContainer");
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
   
        attachedButton.onClick.AddListener(delegate { HalberdOnClick(attachedButton); });
       
    }
    

    void HalberdOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().domumHalberd = true;
      //  GameObject.Find("Knight").GetComponent<KnightScript>().attackRange++;
        upgradeContainers.Remove("HalberdContainer");
        AfterUpgradeApplied();
    }

    public void HardenedWarriorContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Knight gains a flat +10 Hp, and can no longer be stunned";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
    
        attachedButton.onClick.AddListener(delegate { HardenedWarriorOnClick(attachedButton); });

    }


    void HardenedWarriorOnClick(Button targetButton)
    {
        
        GameObject.Find("Knight").GetComponent<KnightScript>().maxhealth+=10;
        GameObject.Find("Knight").GetComponent<KnightScript>().hardened = true;
        upgradeContainers.Remove("HardenedWarriorContainer");
        AfterUpgradeApplied();
    }

    public void RawGritContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The first time per map the knight drops below 0hp, he is instead set to 1hp";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        
        attachedButton.onClick.AddListener(delegate { RawGritOnClick(attachedButton); });

    }


    void RawGritOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().rawGrit = true;
        GameObject.Find("Knight").GetComponent<KnightScript>().rawGritUsed = false;
        upgradeContainers.Remove("RawGritContainer");
        AfterUpgradeApplied();
    }


    void ParryStrikebackContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "After Activating Parry, the first time the knight is struck they take no damage and strike all enemies in range";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];

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
     
        attachedButton.onClick.AddListener(delegate { ParryHoldfastOnClick(attachedButton); });
        

    }

    void ParryHoldfastOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().parryHoldfast = true;
        upgradeContainers.Remove("ParryDodgeChanceContainer");
       
        AfterUpgradeApplied();
    }

    void FrenziedContainer()
    {
        
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 3rd ability becomes 'Frenzied Strike', which attacks the same target 3 times in quick succession";
    
        attachedButton.onClick.AddListener(delegate { FrenziedOnClick(attachedButton); });
        
    }

    void FrenziedOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().frenziedStrike = true;
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
        upgradeContainers.Remove("FlamingContainer");
        upgradeContainers.Add("FrenzyComboContainer");
        AfterUpgradeApplied();
    }
    void FlamingContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 3rd ability becomes 'Flaming Blade', which grants +4 damage on the knight's attacks";
    
        attachedButton.onClick.AddListener(delegate { FrenziedOnClick(attachedButton); });

    }

    void FlamingOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().flamingBlade = true;
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
        upgradeContainers.Remove("FlamingContainer");
        upgradeContainers.Add("LingeringFlameContainer");
        upgradeContainers.Add("FlamingComboContainer");
        AfterUpgradeApplied();
    }

    void LingeringFlameContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Flaming blade now applies a burn to the target on hit";
     
        attachedButton.onClick.AddListener(delegate { FrenziedOnClick(attachedButton); });

    }

    void LingeringFlameOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().lingeringFlame = true;
        upgradeContainers.Remove("LingeringFlameContainer");
        
        AfterUpgradeApplied();
    }

    void GloryContainer()
    {
        
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 3rd ability becomes 'Glory or Death', Granting +2 Damage reduction to all friendly units within 2 tiles";
       
        attachedButton.onClick.AddListener(delegate { GloryOnClick(attachedButton); });
        
    }
    void GloryOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().righteousGlory = true;
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
        upgradeContainers.Remove("FlamingContainer");
        upgradeContainers.Add("GloriousWarriorContainer");
        upgradeContainers.Add("RighteousComboContainer");
        AfterUpgradeApplied();
    }

    void GloriousWarriorContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Glory or Death additionally grants +2 damage for the duration";
       
        attachedButton.onClick.AddListener(delegate { GloriousWarriorOnClick(attachedButton); });

    }

    void GloriousWarriorOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().lingeringFlame = true;
        upgradeContainers.Remove("LingeringFlameContainer");

        AfterUpgradeApplied();
    }
    void UnstopableContainer()
    {
        
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 3rd ability becomes 'Unstopable Force', Causing them to take no damage until their next turn";

     
        attachedButton.onClick.AddListener(delegate { UnstopableOnClick(attachedButton); });
    }
    void UnstopableOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().unstopableForce = true;
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
        upgradeContainers.Remove("FlamingContainer");
        upgradeContainers.Add("UnmovableContainer");
        upgradeContainers.Add("UnstopableContainer");
        AfterUpgradeApplied();
    }

    void UnmovableContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "When Unstopable is active, the knight stuns any target they strike, additionally they gain maximum +5 hp";

     
        attachedButton.onClick.AddListener(delegate { UnmovableOnClick(attachedButton); });
    }
    void UnmovableOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().unmovable = true;
        GameObject.Find("Knight").GetComponent<KnightScript>().maxhealth += 5;
        upgradeContainers.Remove("UnmovableContainer");
       
        AfterUpgradeApplied();
    }

    void DeathKnightContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 4th ability becomes the passive ability 'Death Knight', causing them to heal a portion of all the damage they deal";

      
        attachedButton.onClick.AddListener(delegate { DeathKnightOnClick(attachedButton); });
    }
    void DeathKnightOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().deathKnight = true;
        upgradeContainers.Remove("DeathKnightContainer");
        upgradeContainers.Remove("SlayerContainer");
        AfterUpgradeApplied();
    }
    void SlayerContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 4th ability becomes the passive ability 'Slayer's Wrath', granting them +1 damage each time they are struck(To a maximum of 5)";

      
        attachedButton.onClick.AddListener(delegate { SlayerOnClick(attachedButton); });
    }
    void SlayerOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().slayer = true;
        upgradeContainers.Remove("DeathKnightContainer");
        upgradeContainers.Remove("SlayerContainer");
        
        AfterUpgradeApplied();
    }

    ///Combos
    ///

    void RiposteContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Parry can now combo with cleave, reducing the cooldown of both abilities if parry is used first, and granting +3 damage reduction if cleave is used first";

     
        attachedButton.onClick.AddListener(delegate { RiposteOnClick(attachedButton); });
    }
    void RiposteOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().riposte = true;
        upgradeContainers.Remove("RiposteContainer");

        AfterUpgradeApplied();
    }

    void FrenzyComboContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Parry can now combo with Frenzied Strikes, adding and extra attack if parry is used first, and healing for 5hp if frenzied strikes is used first";

   
        attachedButton.onClick.AddListener(delegate { FrenzyComboOnClick(attachedButton); });
    }
    void FrenzyComboOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().combosU[0] = true;
        upgradeContainers.Remove("FrenzyComboContainer");

        AfterUpgradeApplied();
    }

    void UnstopableComboContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Parry can now combo with Unstopable Force, Doubling the duration of the comboed ability";

 
        attachedButton.onClick.AddListener(delegate { UnstopableComboOnClick(attachedButton); });
    }
    void UnstopableComboOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().combosU[1] = true;
        upgradeContainers.Remove("RiposteContainer");

        AfterUpgradeApplied();
    }

    void RighteousComboContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Parry can now combo with Righteous Glory, healing all nearby allies when using the comboed ability";

        
        attachedButton.onClick.AddListener(delegate { RighteousComboOnClick(attachedButton); });
    }
    void RighteousComboOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().combosU[3] = true;
        upgradeContainers.Remove("RighteousComboContainer");

        AfterUpgradeApplied();
    }

    void FlamingComboContainer()
    {

        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Parry can now combo with Flaming blade, granting +1 duration if Parry is used first, and an exploding AoE when struck if flaming blade is used first";

      
        attachedButton.onClick.AddListener(delegate { FlamingComboOnClick(attachedButton); });
    }
    void FlamingComboOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().combosU[2] = true;
        upgradeContainers.Remove("RiposteContainer");

        AfterUpgradeApplied();
    }

}
