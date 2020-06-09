using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmSettings : MonoBehaviour
{
    private float startTime = 0f;
    public GameObject nextButton;
    public TextMeshProUGUI text;

    public int currentload = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentload = 0;
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time - startTime > 2 && currentload == 0)
        {
            text.text = "#" + GameManagerScript.tknumb + "->#" + BrowserLoad.newTKNumb + "\n\nStop management service... - OK\n\n";
            currentload++;
        }
        if (Time.time - startTime > 4 && currentload == 1)
        {
            text.text += "TK ТИП-1\n     Cisco SB RV130 - OK";
            currentload++;
        }
        if (Time.time - startTime > 6 && currentload == 2)
        {
            text.text += "\n     Grandstream HT704 - OK\n\n";
            currentload++;
        }
        if (Time.time - startTime > 8 && currentload == 3)
        {
            if (BrowserLoad.newTKNumb.Equals("0000"))
            {
                text.text += $"Host's ip will be changed to 10.0.0.3... - OK\n";
            }
            else
            {
                text.text += $"Host's ip will be changed to 185.{BrowserLoad.newTKNumb[0]}{BrowserLoad.newTKNumb[1]}.{BrowserLoad.newTKNumb[2]}{BrowserLoad.newTKNumb[3]}.3... - OK\n";
            }
            currentload++;
        }
        if (Time.time - startTime > 10 && currentload == 4)
        {
            text.text += "Apply WAN settings... - OK\n";
            currentload++;
        }
        if (Time.time - startTime > 12 && currentload == 5)
        {
            text.text += "Creating FreeSwitch configuration... - OK\n";
            currentload++;
        }
        if (Time.time - startTime > 14 && currentload == 6)
        {
            text.text += "Waiting for devices reboot... - OK\n";
            currentload++;
        }
        if (Time.time - startTime > 16 && currentload == 7)
        {
            text.text += "Start management service... - OK\n";
            currentload++;
        }
        if (Time.time - startTime > 18 && currentload == 8)
        {
            text.text += "Restarting services\n=============\n";
            currentload++;
        }
        if (Time.time - startTime > 22 && currentload == 9)
        {
            text.text += "Finished - Services are restarted and device is ready";
            nextButton.SetActive(true);
            currentload++;
        }
    }
}
