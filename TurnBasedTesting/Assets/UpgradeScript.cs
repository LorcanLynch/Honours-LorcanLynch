
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
        upgradeContainers.Add("SeekingBlastContainer");
        upgradeContainers.Add("PowerSurgeContainer");
        upgradeContainers.Add("BombardmentContainer");
        upgradeScreen = GameObject.Find("VictoryScreen");
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
            Invoke(upgradeContainers[x], 0.1f);
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
       
    }

    void HalberdOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().domumHalberd = true;
        GameObject.Find("Knight").GetComponent<KnightScript>().attackRange++;
        upgradeContainers.Remove("HalberdContainer");
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

    }
    void ParryStrikeOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().parryDamage = true;
        upgradeContainers.Remove("ParryStikebackContainer");
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

    }

    void ParryHoldfastOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().parryHoldfast = true;
        upgradeContainers.Remove("ParryDodgeChanceContainer");
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
    }

    void FrenziedOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().frenziedStrike = true;
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
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
    }
    void GloryOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().righteousGlory = true;
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
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
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { UnstopableOnClick(target); });
    }
    void UnstopableOnClick(Button targetButton)
    {
        GameObject.Find("Knight").GetComponent<KnightScript>().unstopableForce = true;
        upgradeContainers.Remove("GloryContainer");
        upgradeContainers.Remove("FrenziedContainer");
        upgradeContainers.Remove("UnstopableContainer");
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

    void PowerSurgeContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "Surge now also grants +4 damage, and 2 additional move speed \n (This exceeds a character's max move speed)";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { PowerSurgeOnClick(target); });
    }

    void PowerSurgeOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().PowerSurge = true;
        upgradeContainers.Remove("PowerSurgeContainer");
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }   

    void BombardmentContainer()
    {
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
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
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
    }
    void LifeDrainOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().lifeDrain = true;
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
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
    }
    void IncantationOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().incantationOfPower = true;
        upgradeContainers.Remove("BombardmentContainer");
        upgradeContainers.Remove("LifeDrainContainer");
        upgradeContainers.Remove("IncantationContainer");
        targetButton.gameObject.SetActive(false);
        upgradesAvailable--;
        if (upgradesAvailable == 0)
        {
            UpgradesCancelled();
        }
    }
    void FireballContainer()
    {
        int button = Random.Range(0, upgradeButtons.Count);
        Button target = upgradeButtons[button];
        upgradeButtons.RemoveAt(button);
        target.GetComponent<Image>().sprite = abilitySprites[2];
        target.GetComponentInChildren<TextMeshProUGUI>().text = "The Wizard's Fourth Abilty becomes Fireball, a large damage AoE.";
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(delegate { FireballOnClick(target); });
    }
    void FireballOnClick(Button targetButton)
    {

        GameObject.Find("Wizard").GetComponent<WizardScript>().fireball = true;
        upgradeContainers.Remove("FireballContainer");
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
