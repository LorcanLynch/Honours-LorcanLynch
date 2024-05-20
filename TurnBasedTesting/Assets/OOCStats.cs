using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOCStats : MonoBehaviour
{
    // Start is called before the first frame update

    public int knightPoints;

    public static OOCStats Instance;
    public void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
