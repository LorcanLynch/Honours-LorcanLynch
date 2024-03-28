using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{

    public List<string> tutorialText;
    public List<Sprite> currSprites;

    public Sprite[] allSprites;

    public TextMeshProUGUI tutText;
    public TextMeshProUGUI tutTitle;
    public  Image tutSprite;
    public int currPage;
    public int maxPage;
    AudioSource aS;
    // Start is called before the first frame update
    void Start()
    {
        AbilitiesPrefab();
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
      
    }
    public void AttackingPrefab()
    {
        currSprites = new List<Sprite> { allSprites[0], allSprites[1], allSprites[2] };
        tutorialText = new List<string> { "Press Space to ready a basic attack, red squares are your valid targets", "Click on an enemy when targetting to attack them, you can only attack once per turn", "The chance to hit is the unit's accuracy subtracting the target's evasion" };
        tutSprite.sprite = currSprites[0];
        tutText.text = tutorialText[0];
        tutTitle.text = "Attacking";
        maxPage = 2;
        currPage = 0;
    }

    public void AbilitiesPrefab()
    {
        currSprites = new List<Sprite> { allSprites[3], allSprites[4], allSprites[5] };
        tutorialText = new List<string> { "Click on an ability to prepare it, you can also hit the relevant key to do the same (1,2,3,4)", "Some abilties require enemy target's others require allies or none at all, click on the relevant unit to activate your ability", "Abilites also have cooldowns based on the strength of the ability, some are also free costing no action!" };
        tutSprite.sprite = currSprites[0];
        tutText.text = tutorialText[0];
        tutTitle.text = "Abilities";
        maxPage = 2;
        currPage = 0;
    }

    public void BasicPrefab()
    {
        currSprites = new List<Sprite> { allSprites[3], allSprites[4], allSprites[5] };
        tutorialText = new List<string> { "You can move the camera using WASD or middle mouse", "Click on a friendly unit to select them, you can move them by clicking on another tile", "You can access the tutorials at any time by pressing ESC, or on the main menu" };
        tutSprite.sprite = currSprites[0];
        tutText.text = tutorialText[0];
        tutTitle.text = "General Movement and controls";
        maxPage = 2;
        currPage = 0;
    }


    public void Next()
    {
        currPage++;
        if(currPage > maxPage)
        {
            gameObject.SetActive(false);
        }
        tutText.text = tutorialText[currPage];
        tutSprite.sprite = currSprites[currPage];
    }

    public void Back()
    {
        if (currPage != 0)
        {
            currPage--;
        }
         
        tutText.text = tutorialText[currPage];
        tutSprite.sprite = currSprites[currPage];
    }
}
