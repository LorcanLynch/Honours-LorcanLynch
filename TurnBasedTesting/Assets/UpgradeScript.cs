
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeScript : MonoBehaviour
{
    public List<Button> upgradeButtons;
    public ObjectiveScript tileMapObj;
    public Sprite[] abilitySprites = new Sprite[20];
    public List<string> upgradeContainers;
    public int upgradesAvailable = 2;
    public GameObject upgradeScreen;
    // Start is called before the first frame update
    void Start()
    {
        tileMapObj = GameObject.Find("Map").GetComponent<ObjectiveScript>();
        upgradeContainers.Add("HalberdContainer");
        upgradeContainers.Add("ParryStrikebackContainer");
        upgradeContainers.Add("ParryDodgeChanceContainer");
        upgradeContainers.Add("FrenziedContainer");
        upgradeContainers.Add("GloryContainer");
        upgradeContainers.Add("UnstopableContainer");

        //Wizard Upgrades
        upgradeContainers.Add("SeekingBlastContainer");
        upgradeContainers.Add("PowerSurgeContainer");
        upgradeContainers.Add("BombardmentContainer");
        upgradeContainers.Add("IceKingContainer");
        upgradeContainers.Add("FireLordContainer");
        upgradeContainers.Add("FireBallContainer");
        
        upgradeContainers.Add("LifeDrainContainer");
        upgradeContainers.Add("IncantationContainer");
        upgradeScreen = GameObject.Find("VictoryScreen");


        //Huntress Upgrades
        upgradeContainers.Add("LastingInfluenceContainer");


        //Sam Upgrades
        
        upgradeScreen.SetActive(false);
        upgradesAllowed();


        
    }
    void printit()
    {
        print("Worked");
    }

    public void upgradesAllowed()
    {
        upgradeScreen.SetActive(true);
        upgradeButtons = new List<Button>();
        upgradeButtons.Add(GameObject.Find("upgrade1").GetComponent<Button>());
        upgradeButtons.Add(GameObject.Find("upgrade2").GetComponent<Button>());
        upgradeButtons.Add(GameObject.Find("upgrade3").GetComponent<Button>());
        upgradeButtons.Add(GameObject.Find("upgrade4").GetComponent<Button>());
        List<string> possibleContainers = upgradeContainers;
        int x = Random.Range(0, possibleContainers.Count);
        upgradesAvailable = 2;
        for(int y = 0; y <4; y++)
        {
            
            x = Random.Range(0, possibleContainers.Count);
            Invoke(upgradeContainers[x], 0.1f * y);
            possibleContainers.RemoveAt(x);
            print(x);
        }    

      

    }
    void UpgradesCancelled()
    {
        upgradeScreen.SetActive(false);
        tileMapObj.StartNewLevel();
    }
    /// <summary>
    /// Knight Upgrades
    /// </summary>
    void HalberdContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[0];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "+1 range, and cleave now sweeps \n all in range, stunning them for one turn";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { HalberdOnClick(target); });
        upgradeContainers.Remove("HalberdContainer");

    }

    void HalberdOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().domumHalberd = true;
        GameObject.Find("Knight").GetComponent<KnightScript>().attackRange++;
       
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if(upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
        
    }

    void ParryStrikebackContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[1];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "After Activating Parry, the first time the knight is struck they take no damage and strike all enemies in range";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { ParryStrikeOnClick(target); });
        upgradeContainers.Remove("ParryStikebackContainer");

    }
    void ParryStrikeOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().parryDamage = true;
      
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }

    void ParryDodgeChanceContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[1];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "Parry now increases dodge by 4x(Previously 2x), and its cooldown is one turn shorter";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { ParryHoldfastOnClick(target); });
        upgradeContainers.Remove("ParryDodgeChanceContainer");

    }

    void ParryHoldfastOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().parryHoldfast = true;
      
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }

    void FrenziedContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 3rd ability becomes 'Frenzied Strike', which attacks the same target 3 times in quick succession";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { FrenziedOnClick(target); });
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
    }

    void FrenziedOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().frenziedStrike = true;
    
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }

    void GloryContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 3rd ability becomes 'Glory or Death', Granting +2 Damage reduction to all friendly units within 2 tiles";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { GloryOnClick(target); });
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
    }
    void GloryOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().righteousGlory = true;
       
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }

    void UnstopableContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "Knight's 3rd ability becomes 'Unstopable Force', Causing them to take no damage until their next turn";
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { UnstopableOnClick(target); });
    }
    void UnstopableOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().unstopableForce = true;
       
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }






    /// <summary>
    /// Wizard Abilities
    /// </summary>
    void SeekingBlastContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The wizard can no longer miss, but his damage is halfed";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { SeekingBlastOnClick(target); });
    }

    void SeekingBlastOnClick(Button targetButton)
    {
        GameObject.Find("Wizard").GetComponent<WizardScript>().accuracy= 999;
        GameObject.Find("Wizard").GetComponent<WizardScript>().attackPower =Mathf.RoundToInt(GameObject.Find("Wizard").GetComponent<WizardScript>().attackPower/2);
        upgradeContainers.Remove("SeekingBlastContainer");
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }

    void FireLordContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[0];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The wizard's basic attacks now deal +2 damage and apply a 4 Turn Burn, dealing damage over time \n but accuracy is reduced by 10";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { FireLordOnClick(target); });
        upgradeContainers.Remove("FireLordContainer");
        upgradeContainers.Remove("SeekingBlastContainer");
        upgradeContainers.Remove("IceKingContainer");

    }

    void FireLordOnClick(Button targetButton)
    {
        GameObject.Find("Wizard").GetComponent<WizardScript>().fireLord = true;
       
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }

    }

    void IceKingContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[0];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard now has a chance to freeze enemies with his basic attacks, stunning them";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { FireLordOnClick(target); });
        upgradeContainers.Remove("IceKingContainer");
        upgradeContainers.Remove("FireLordContainer");

    }

    void IceKingOnClick(Button targetButton)
    {
        GameObject.Find("Wizard").GetComponent<WizardScript>().fireLord = true;
        
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }

    }
    void PowerSurgeContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "Surge now also grants +4 damage, and 2 additional move speed \n (This exceeds a character's max move speed)";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { PowerSurgeOnClick(target); });
        upgradeContainers.Remove("PowerSurgeContainer");
    }

    void PowerSurgeOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().PowerSurge = true;
       
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }   

    void BombardmentContainer()
    {
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Third Abilty becomes Bombardment, a low damage ability but with extreme range";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { BombardmentOnClick(target); });
       
    }
    void BombardmentOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().Bombardment = true;
       
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }
    void LifeDrainContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Third Abilty becomes life drain, a standard attack that heals \n the wizard for the damage done";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { LifeDrainOnClick(target); });
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
    }
    void LifeDrainOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().lifeDrain = true;
        upgradeContainers.Add("SoulDrainContainer");
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }

    void SoulDrainContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "Life drain applies a damage over time affect, dealing 100% of the abilities damage over 4 turns";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { SoulDrainOnClick(target); });
        upgradeContainers.Remove("SoulDrainContainer");
        
    }
    void SoulDrainOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().soulDrain = true;
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }
    void IncantationContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Third Abilty becomes Incantation of Power, an AoE buff that increases damage done \n of all nearby allies";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { IncantationOnClick(target); });
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
    }
    void IncantationOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().incantationOfPower = true;
       
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }
    void FireBallContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Fourth Abilty becomes Fireball, a large damage AoE.";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { FireballOnClick(target); });
        upgradeContainers.Remove("FireballContainer");
    }
    void FireballOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().fireball = true;
       
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }









    /// <summary>
    /// Huntress Abilities
    /// </summary>
    /// 

    void LastingInfluenceContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' soothe ability now grants an extra two health each turn for 3 turns";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { LastingInfluenceOnClick(target); });
        upgradeContainers.Remove("LastingInfluenceContainer");
    }
    void LastingInfluenceOnClick(Button targetButton)
    {

        GameObject.Find("Huntress").GetComponent<HuntressScript>().lastingInfluence = true;
       
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {   
            UpgradesCancelled();
        }
    }



    void PickYourPreyContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 3rd ability becomes mark prey, which marks an enemy unit causing it to take +3 damage for three turns(Does not cost their action)";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { PickYourPreyOnClick(target); });
        upgradeContainers.Remove("FieldMedicineContainer");
        upgradeContainers.Remove("PickYourPreyContainer");
        upgradeContainers.Remove("SnipeContainer");
    }
    void PickYourPreyOnClick(Button targetButton)
    {

        GameObject.Find("Huntress").GetComponent<HuntressScript>().markPrey = true;
       
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }

    void FieldMedicineContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 3rd ability becomes mark prey, which marks an enemy unit causing it to take +3 damage for three turns(Does not cost their action)";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { PickYourPreyOnClick(target); });
        upgradeContainers.Remove("FieldMedicineContainer");
        upgradeContainers.Remove("PickYourPreyContainer");
        upgradeContainers.Remove("SnipeContainer");
    }
    void FieldMedicineOnClick(Button targetButton)
    {

        GameObject.Find("Huntress").GetComponent<HuntressScript>().markPrey = true;
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }

    void SnipeContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 3rd ability becomes mark prey, which marks an enemy unit causing it to take +3 damage for three turns(Does not cost their action)";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { PickYourPreyOnClick(target); });
        upgradeContainers.Remove("FieldMedicineContainer");
        upgradeContainers.Remove("PickYourPreyContainer");
        upgradeContainers.Remove("SnipeContainer");
    }
    void SnipeOnClick(Button targetButton)
    {

        GameObject.Find("Huntress").GetComponent<HuntressScript>().markPrey = true;
        
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }









    // Update is called once per frame
    void Update()
    {
        
    }
}
