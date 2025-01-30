using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MissionButtonHolder : MonoBehaviour
{

     public enum missionLocation
    {
        Desert,
        Plains,
        Mountain,
        Underhold,
        Forest,
        Tundra
    }

    public enum ObjectiveType
    {
        Eliminate,
        Hold,
        Secure,
        Assassinate,
        Besiege,
        Rescue
    }
    public enum Difficulty
    {
        Trivial,
        Normal,
        Hard,
        Brutal,
        Doom
    }

    



    public missionLocation locale;
    public ObjectiveType oType;
    public Difficulty missionDifficulty;
    public GameObject textView;
    public GameObject missionDetails;



    


    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI txtObj = textView.GetComponent<TextMeshProUGUI>();
        txtObj.text = "Enviroment: "  + locale.ToString() + "\n"+ "Objective: " + oType.ToString()+ "\n" + "Difficulty: " + missionDifficulty.ToString();
        

    }

    public void StartMission()
    {
        SceneManager.LoadScene(1);
    }
    
    private void OnMouseEnter()
    {
        missionDetails.SetActive(true);
    }
   
    private void OnMouseExit()
    {
        missionDetails.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
