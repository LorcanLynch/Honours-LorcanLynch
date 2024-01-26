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
        GenerateChoice();


    }

    // Update is called once per frame
    void Update()
    {
      

    }
     

    public void LastingInfluenceContainer()
    {
       attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' soothe ability now grants an extra two health each turn for 3 turns";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
       attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { LastingInfluenceOnClick(attachedButton); });
       
    }
    

    void LastingInfluenceOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().lastingInfluence = true;
       
        upgradeContainers.Remove("LastingInfluenceContainer");
        AfterUpgradeApplied();
    }

    public void MarkPreyContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 3rd ability becomes mark prey, which marks an enemy unit causing it to take +3 damage for three turns(Does not cost their action)";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { MarkPreyOnClick(attachedButton); });

    }


    void MarkPreyOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().markPrey = true;

        upgradeContainers.Remove("FieldMedicineContainer");
        upgradeContainers.Remove("MarkPreyContainer");
        upgradeContainers.Remove("SnipeContainer");
        AfterUpgradeApplied();
    }

    public void FieldMedicineContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { FieldMedicineOnClick(attachedButton); });

    }


    void FieldMedicineOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().fieldMedicine = true;

        upgradeContainers.Remove("FieldMedicineContainer");
        upgradeContainers.Remove("MarkPreyContainer");
        upgradeContainers.Remove("SnipeContainer");
        AfterUpgradeApplied();
    }
    

    public void SnipeContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 3rd ability becomes snipe, a high damage ability that cannot miss";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate { SnipeOnClick(attachedButton); });

    }


    void SnipeOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().snipe = true;

        upgradeContainers.Remove("FieldMedicineContainer");
        upgradeContainers.Remove("MarkPreyContainer");
        upgradeContainers.Remove("SnipeContainer");
        AfterUpgradeApplied();
    }

    public void ExudingWarmthContainer()
    {
        attachedButton.GetComponentInChildren<TextMeshProUGUI>().text = "The Huntress' 4th ability becomes Exuding Warmth, a buff lasting 4 rounds, healing those around the huntress at the end of each turn";
        attachedButton.GetComponent<Image>().sprite = abilitySprites[0];
        attachedButton.onClick.RemoveAllListeners();
        attachedButton.onClick.AddListener(delegate {  ExudingWarmthOnClick(attachedButton); });

    }


    void ExudingWarmthOnClick(Button targetButton)
    {
        GameObject.Find("Huntress").GetComponent<HuntressScript>().snipe = true;

        upgradeContainers.Remove("ExudingWarmthContainer");
       
        AfterUpgradeApplied();
    }

}
