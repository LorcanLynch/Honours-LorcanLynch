    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class HuntressUpgrades : Upgrades
{

    
    // Start is called before the first frame update
    void Start()
    {
        upgradeContainers.Add("LastingInfluenceContainer");
        upgradeContainers.Add("BetweenTheEyesContainer");
        upgradeContainers.Add("WordsOfPowerContainer");
        upgradeContainers.Add("ThirdCharmContainer");
        upgradeContainers.Add("MarkPreyContainer");
        upgradeContainers.Add("FieldMedicineContainer");
        upgradeContainers.Add("PiercingSongContainer");
        
        upgradeContainers.Add("ExudingWarmthContainer");
        upgradeContainers.Add("TrueshotContainer");
        GenerateChoice();


    }

    // Update is called once per frame
    
     

    public void LastingInfluenceContainer()
    {
       attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' soothe ability now grants an extra two health each turn for 3 turns";
        
        attachedButton.GetComponent<Image>().sprite = abilitySprites[1];
    
        attachedButton.onClick.AddListener(delegate { LastingInfluenceOnClick(attachedButton); });
       
    }
    

    void LastingInfluenceOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().lastingInfluence = true;
       
        upgradeContainers.Remove("LastingInfluenceContainer");
        AfterUpgradeApplied("The Huntress' soothe ability now grants an extra two health each turn for 3 turns");
    }

    public void BetweenTheEyesContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Grant huntress a 10% chance to critically strike, causing the attack to deal double damage";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
      
        attachedButton.onClick.AddListener(delegate { BetweenTheEyesOnClick(attachedButton); });

    }
    void BetweenTheEyesOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().eyes = true;

        upgradeContainers.Remove("BetweenTheEyesContainer");
        
        AfterUpgradeApplied("Grant huntress a 10% chance to critically strike, causing the attack to deal double damage");
    }

    public void ThirdCharmContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Every 3 Attacks the huntress deals double damage";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
   
        attachedButton.onClick.AddListener(delegate { ThirdCharmOnClick(attachedButton); });

    }


    void ThirdCharmOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().thirdCharm = true;

        upgradeContainers.Remove("ThirdCharmContainer");
        AfterUpgradeApplied("Every 3 Attacks the huntress deals double damage");
    }

    public void WordsOfPowerContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The huntress soothe ability grants allies affected by it a buff, granting +20 dodge for two turns";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[1];
  
        attachedButton.onClick.AddListener(delegate { WordsOfPowerOnClick(attachedButton); });

    }

    void WordsOfPowerOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().warding = true;

        upgradeContainers.Remove("WordsOfPowerContainer");
        AfterUpgradeApplied("The huntress soothe ability grants allies affected by it a buff, granting +20 dodge for two turns");
    }



    public void MarkPreyContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 3rd ability becomes mark prey, which marks an enemy unit causing it to take +3 damage for three turns(Does not cost an action)";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[3];
   
        attachedButton.onClick.AddListener(delegate { MarkPreyOnClick(attachedButton); });

    }


    void MarkPreyOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().markPrey = true;
        GameObject.Find("Huntress").GetComponent<UnitScript>().abilityIcons[2] = attachedButton.GetComponent<Image>().sprite;
        GameObject.Find("Huntress").GetComponent<HuntressScript>().abilityDesc[2] = "Free action ability that marks the chosen target, causing them to take +3 damage from the huntress";
        upgradeContainers.Remove("FieldMedicineContainer");
        upgradeContainers.Remove("MarkPreyContainer");
        upgradeContainers.Remove("SnipeContainer");
        upgradeContainers.Add("TrainedSurgeonContainer");
        upgradeContainers.Add("MarkSootheContainer");
        AfterUpgradeApplied("The Huntress' 3rd ability becomes mark prey, which marks an enemy unit causing it to take +3 damage for three turns(Does not cost an action)");
    }



    public void FieldMedicineContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 3rd ability becomes field medicine, a single target heal with 3 charges, healing 3 health per charge";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[4];
    
        attachedButton.onClick.AddListener(delegate { FieldMedicineOnClick(attachedButton); });

    }


    


    void FieldMedicineOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().fieldMedicine = true;
        GameObject.Find("Huntress").GetComponent<UnitScript>().abilityIcons[2] = attachedButton.GetComponent<Image>().sprite;
        GameObject.Find("Huntress").GetComponent<HuntressScript>().abilityDesc[2] = "Free action heal that has 3 charges, but no cooldown";
        upgradeContainers.Remove("FieldMedicineContainer");
        upgradeContainers.Remove("MarkPreyContainer");
        upgradeContainers.Remove("SnipeContainer");
        upgradeContainers.Add("MedicineSootheContainer");
        upgradeContainers.Add("TrainedSurgeonContainer");
        AfterUpgradeApplied("The Huntress' 3rd ability becomes field medicine, a single target heal with 3 charges, healing 3 health per charge");
    }

    public void TrainedSurgeonContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Field medicine heals for +1 additional health and has one additional charge";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[4];
      
        attachedButton.onClick.AddListener(delegate { TrainedSurgeonOnClick(attachedButton); });

    }
    void TrainedSurgeonOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().trainedSurgeon= true;

        upgradeContainers.Remove("TrainedSurgeonContainer");
        
        AfterUpgradeApplied("Field medicine heals for +1 additional health and has one additional charge");
    }


    public void SnipeContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 3rd ability becomes snipe, a high damage ability that cannot miss";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
 
        attachedButton.onClick.AddListener(delegate { SnipeOnClick(attachedButton); });

    }


    void SnipeOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().snipe = true;
        GameObject.Find("Huntress").GetComponent<UnitScript>().abilityIcons[2] = attachedButton.GetComponent<Image>().sprite;
        GameObject.Find("Huntress").GetComponent<HuntressScript>().abilityDesc[2] = "High damage ability that can't miss";
        upgradeContainers.Remove("FieldMedicineContainer");
        upgradeContainers.Remove("MarkPreyContainer");
        upgradeContainers.Remove("SnipeContainer");
        upgradeContainers.Add("LongshotContainer");
        upgradeContainers.Add("SnipeSootheContainer");
        AfterUpgradeApplied("The Huntress' 3rd ability becomes snipe, a high damage ability that cannot miss");
    }

    public void LongshotContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Grant snipe +3 range, additionally gain +3 damage";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
   
        attachedButton.onClick.AddListener(delegate { LongShotOnClick(attachedButton); });

    }
    void LongShotOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().markPrey = true;
        GameObject.Find("Huntress").GetComponent<HuntressScript>().attackPower += 3;

       upgradeContainers.Remove("LongshotContainer");
        AfterUpgradeApplied("Grant snipe +3 range, additionally gain +3 damage");
    }

    public void ExudingWarmthContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 4th ability becomes Exuding Warmth, a buff lasting 4 rounds, healing those around the huntress at the end of each turn";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[6];
        
        attachedButton.onClick.AddListener(delegate {  ExudingWarmthOnClick(attachedButton); });

    }


    void ExudingWarmthOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().exudeWarmth = true;
        GameObject.Find("Huntress").GetComponent<UnitScript>().abilityIcons[3] = attachedButton.GetComponent<Image>().sprite;
        GameObject.Find("Huntress").GetComponent<HuntressScript>().abilityDesc[3] = "Buff lasting 4 turns that heals allies at the end of each turn";
        upgradeContainers.Remove("ExudingWarmthContainer");
        upgradeContainers.Remove("TrueshotContainer");
        AfterUpgradeApplied("The Huntress' 4th ability becomes Exuding Warmth, a buff lasting 4 rounds, healing those around the huntress at the end of each turn");
    }

    public void TrueshotContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 4th ability becomes True Shot, a buff lasting 4 rounds, granting her +200 Accuracy and +20 dodge chance";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[5];
  
        attachedButton.onClick.AddListener(delegate { TrueShotOnClick(attachedButton); });

    }


    void TrueShotOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().TrueShot = true;
        GameObject.Find("Huntress").GetComponent<UnitScript>().abilityIcons[3] = attachedButton.GetComponent<Image>().sprite;
        GameObject.Find("Huntress").GetComponent<HuntressScript>().abilityDesc[3] = "Buff lasting 4 turns that grants +200 accuracy and 20 dodge chance to the huntress";
        upgradeContainers.Remove("ExudingWarmthContainer");
        upgradeContainers.Remove("TrueshotContainer");
        AfterUpgradeApplied("The Huntress' 4th ability becomes True Shot, a buff lasting 4 rounds, granting her +200 Accuracy and +20 dodge chance");
    }



    public void PiercingSongContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Piercing shot can now combo with soothe, dealing double damage versus low armour targets if soothe is used first, and granting +2 movement to the soothed taraget if piercing shot is used first";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
     
        attachedButton.onClick.AddListener(delegate { PiercingSongOnClick(attachedButton); });

    }
    void PiercingSongOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().songCombo = true;

        upgradeContainers.Remove("PiercingSongContainer");

        AfterUpgradeApplied("Piercing shot can now combo with soothe, dealing double damage versus low armour targets if soothe is used first, and granting +2 movement to the soothed taraget if piercing shot is used first");
    }

    public void MarkSootheContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mark Prey can now combo with soothe, Burning the target for 4 damage over two turns if soothe is used first, and giving the abilty for soothe to overheal if mark prey is used first";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[3];
        
        attachedButton.onClick.AddListener(delegate { MarkSootheOnClick(attachedButton); });

    }
    void MarkSootheOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().abilityCombos[1] = true;

        upgradeContainers.Remove("MarkSootheContainer");

        AfterUpgradeApplied("Mark Prey can now combo with soothe, Burning the target for 4 damage over two turns if soothe is used first, and giving the abilty for soothe to overheal if mark prey is used first");
    }

    public void MedicineSootheContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Field medicine can now combo with soothe, granting both the ability to overheal if comboed";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[4];
        
        attachedButton.onClick.AddListener(delegate { MedicineOnClick(attachedButton); });

    }
    void MedicineOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().abilityCombos[0] = true;

        upgradeContainers.Remove("MedicineSootheContainer");

        AfterUpgradeApplied("Field medicine can now combo with soothe, granting both the ability to overheal if comboed");
    }

    public void SnipeSootheContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Snipe can now combo with soothe, Granting snipe +5 damage if soothe is used first, and granting soothe the ability to overheal";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[2];
        
        attachedButton.onClick.AddListener(delegate { SnipeSootheOnClick(attachedButton); });

    }
     void SnipeSootheOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().abilityCombos[2] = true;

        upgradeContainers.Remove("SnipeSootheContainer");

        AfterUpgradeApplied("Snipe can now combo with soothe, Granting snipe +5 damage if soothe is used first, and granting soothe the ability to overheal");
    }
}
