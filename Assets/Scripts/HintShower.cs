using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintShower : MonoBehaviour
{

    public GameObject[] hints = new GameObject[24];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDisable()
    {
        DisableAllHints();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DisableAllHints();
        hints[GameManagerScript.currentHint].SetActive(true);
        if(GameManagerScript.currentHint.Equals(4))
        {
            hints[GameManagerScript.currentHint + 1].SetActive(true);
        }
    }

    public void DisableAllHints()
    {
        for(int i = 0; i < hints.Length; i++)
        {
            hints[i].SetActive(false);
        }
    }
}
