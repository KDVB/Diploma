using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TelephoneView : MonoBehaviour
{

    public TextMeshProUGUI[] values = new TextMeshProUGUI[8];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i < GameManagerScript.phonesNumbers.Length; i++)
        {
            if(!GameManagerScript.phonesNumbers[i].Equals(string.Empty))
            {
                values[i * 2].text = $"(1) ({GameManagerScript.tknumb}) {GameManagerScript.phonesNumbers[i]}";
                values[i * 2 + 1].text = $"{GameManagerScript.phonesPorts[i]}";
            }
        }
    }
}
