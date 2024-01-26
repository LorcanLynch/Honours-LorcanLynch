using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        tileMapObj = GameObject.Find("Map").GetComponent<ObjectiveScript>();
    }
    public void AfterUpgradeApplied()
    {
        attachedButton.gameObject.SetActive(false);
        upgradesCancelled();
    }

    void Awake()
    {
        tileMapObj = GameObject.Find("Map").GetComponent<ObjectiveScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEnable()
    {
        GenerateChoice();


    }
    public virtual void GenerateChoice()
    {
        List<string> possibleContainers = upgradeContainers;
        int x = Random.Range(0, possibleContainers.Count);
        Invoke(upgradeContainers[x], .001f);
       
    }
    
    public void upgradesCancelled()
    {
        upgradeScreen.SetActive(false);
        tileMapObj.StartNewLevel();
    }
}
