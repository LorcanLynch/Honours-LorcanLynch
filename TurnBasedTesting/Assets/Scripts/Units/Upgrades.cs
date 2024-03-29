using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public List<string> upgradeContainers = new List<string> { };
    // Start is called before the first frame update
    public Button attachedButton;
    public Sprite[] abilitySprites = new Sprite[20];
    public int upgradesAvailable;
    public GameObject upgradeScreen;
    public ObjectiveScript tileMapObj;
    public TextMeshProUGUI enemyText;
    public GameObject[] allUnits;
    public bool upgrading = false;
    
    public void AfterUpgradeApplied(string upgradeText)
    {
        if(upgradeScreen.GetComponent<UpgradeHolder>().upgraded == 1)
        {
            upgradeScreen.GetComponent<UpgradeHolder>().upgradesUnlocked.Add(upgradeText);
            upgradesCancelled();
            upgradeScreen.GetComponent<UpgradeHolder>().upgraded = 0;
        }
        else
        {
            upgradeScreen.GetComponent<UpgradeHolder>().upgraded++;
            upgradeScreen.GetComponent<UpgradeHolder>().upgradesUnlocked.Add(upgradeText);
            attachedButton.gameObject.SetActive(false);

        }
       
      
    }

    void Awake()
    {
        tileMapObj = GameObject.Find("Map").GetComponent<ObjectiveScript>();
        
    }

    public GameObject GenerateEnemy()
    {
        int enemy = Random.Range(0, allUnits.Length);
        return allUnits[enemy];

    }
    public void AddEnemy(GameObject unitToAdd)
    {
        tileMapObj.GetComponent<ObjectiveScript>().spawningPool.Add(unitToAdd);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEnable()
    {
        GenerateChoice();
        upgrading = true;

    }
    public virtual void GenerateChoice()
    {
        attachedButton.onClick.RemoveAllListeners();    
        List<string> possibleContainers = upgradeContainers;
        int x = Random.Range(0, possibleContainers.Count);
        Invoke(upgradeContainers[x], .001f);
        GameObject add =  GenerateEnemy();
        enemyText.text = "Add 1 " + add.name + " to pool";
        attachedButton.onClick.AddListener(delegate { AddEnemy(add); });
    }
    
    public void upgradesCancelled()
    {
        upgradeScreen.SetActive(false);
        upgrading = false;
        tileMapObj.StartNewLevel();
    }
}
