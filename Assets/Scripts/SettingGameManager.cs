using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingGameManager : MonoBehaviour
{

    private string path = Environment.CurrentDirectory + "\\ipAddress.txt";
    private string currentIP = string.Empty;

    public TMP_InputField address;

    // Start is called before the first frame update
    void Start()
    {
        GetIPAddress();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void GetIPAddress()
    {
        if(File.Exists(path))
        {
            currentIP = System.IO.File.ReadAllLines(path)[0];
            address.text = currentIP;
        }
        else
        {
            File.Create(path);
            address.text = currentIP;
        }
    }

    public void SetIPAddress()
    {
        currentIP = address.text;
        File.WriteAllText(path, currentIP);
    }
}
