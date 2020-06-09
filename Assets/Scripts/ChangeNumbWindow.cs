using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeNumbWindow : MonoBehaviour
{

    public TextMeshProUGUI oldTKIP;
    public TextMeshProUGUI oldMarshIP;
    public TextMeshProUGUI oldVoipIP;
    public TextMeshProUGUI newTKIP;
    public TextMeshProUGUI newMarshIP;
    public TextMeshProUGUI newVoipIP;
    public TextMeshProUGUI oldTKNumb;
    public TextMeshProUGUI newTKNumb;

    public Toggle toggle;
    public GameObject nextButton;

    // Start is called before the first frame update
    void Start()
    {
        oldTKNumb.text = GameManagerScript.tknumb;
        newTKNumb.text = BrowserLoad.newTKNumb;
        SetNewValues();
        SetOldValues();
    }

    private void FixedUpdate()
    {
        if(toggle.isOn)
        {
            nextButton.SetActive(true);
        }
    }

    public void SetOldValues()
    {
        if(GameManagerScript.tknumb.Equals("0000"))
        {
            oldTKIP.text = "10.0.0.3";
            oldMarshIP.text = "10.0.0.1";
            oldVoipIP.text = "10.0.0.3";
        }
        else
        {
            oldTKIP.text = $"185.{GameManagerScript.tknumb[0]}{GameManagerScript.tknumb[1]}.{GameManagerScript.tknumb[2]}{GameManagerScript.tknumb[3]}.3";
            oldMarshIP.text = $"185.{GameManagerScript.tknumb[0]}{GameManagerScript.tknumb[1]}.{GameManagerScript.tknumb[2]}{GameManagerScript.tknumb[3]}.1";
            oldVoipIP.text = $"185.{GameManagerScript.tknumb[0]}{GameManagerScript.tknumb[1]}.{GameManagerScript.tknumb[2]}{GameManagerScript.tknumb[3]}.3";
        }
    }

    public void SetNewValues()
    {
        if (BrowserLoad.newTKNumb.Equals("0000"))
        {
            newTKIP.text = "10.0.0.3";
            newMarshIP.text = "10.0.0.1";
            newVoipIP.text = "10.0.0.3";
        }
        else
        {
            newTKIP.text = $"185.{BrowserLoad.newTKNumb[0]}{BrowserLoad.newTKNumb[1]}.{BrowserLoad.newTKNumb[2]}{BrowserLoad.newTKNumb[3]}.3";
            newMarshIP.text = $"185.{BrowserLoad.newTKNumb[0]}{BrowserLoad.newTKNumb[1]}.{BrowserLoad.newTKNumb[2]}{BrowserLoad.newTKNumb[3]}.1";
            newVoipIP.text = $"185.{BrowserLoad.newTKNumb[0]}{BrowserLoad.newTKNumb[1]}.{BrowserLoad.newTKNumb[2]}{BrowserLoad.newTKNumb[3]}.3";
        }
    }
}
