using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnigmaCheckDevices : MonoBehaviour
{
    public TextMeshProUGUI marshIP;
    public TextMeshProUGUI voipIP;

    void Start()
    {
        SetMarshIP();
        SetVoipIP();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMarshIP()
    {
        if(GameManagerScript.tknumb.Equals("0000"))
        {
            marshIP.text = "10.0.0.1";
        }
        else
        {
            marshIP.text = $"185.{GameManagerScript.tknumb[0]}{GameManagerScript.tknumb[1]}.{GameManagerScript.tknumb[2]}{GameManagerScript.tknumb[3]}.1";
        }
    }
    public void SetVoipIP()
    {
        if (GameManagerScript.tknumb.Equals("0000"))
        {
            voipIP.text = "10.0.0.4";
        }
        else
        {
            voipIP.text = $"185.{GameManagerScript.tknumb[0]}{GameManagerScript.tknumb[1]}.{GameManagerScript.tknumb[2]}{GameManagerScript.tknumb[3]}.4";
        }
    }

}
