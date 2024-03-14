using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIconScript : MonoBehaviour 
{
    // Start is called before the first frame update
    public GameObject panel;
    public TextMeshProUGUI text;
    public GameObject descPanel;
    public TextMeshProUGUI descText;
    public string abilityText = "Locked Ability, you can unlock this with upgrades after each mission";
    void Start()
    {
        descPanel = GameObject.Find("abilityPanel");
        descText = GameObject.Find("abilityText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cooldownPanel(int cooldown)
    {
        if (cooldown > 0)
        {
            panel.SetActive(true);
            text.text = cooldown.ToString();
        }
        else
        {
            panel.SetActive(false);
            text.text = "";
        }
    }
    private void OnMouseOver()
    {
        descPanel.SetActive(true);
        descText.text = abilityText;
    }
    private void OnMouseExit()
    {
        descPanel.SetActive(false);
        descText.text = "";
    }

}
