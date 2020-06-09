using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine.UI;

public class BrowserLoad : MonoBehaviour
{
    public string path = Environment.CurrentDirectory + "\\ipAddress.txt";
    private int port = 11000; // порт сервера
    public static string address = "192.168.88.200";

    public GameObject mainScreen;
    public GameObject mainMenuScreen;
    public GameObject controlPanelScreen;
    public GameObject[] networkWindows = new GameObject[3];
    public GameObject ipAddressWindow;
    public GameObject browser;
    public GameObject browserError;

    public TMP_InputField[] IpAddress = new TMP_InputField[4];
    public TMP_InputField[] MaskAddress = new TMP_InputField[4];

    public TMP_InputField searchString;
    public TMP_InputField searchStringError;

    public GameObject EnigmaAuthorization;

    public GameObject[] enigmaSettings = new GameObject[5];

    public TMP_InputField loginField;
    public TMP_InputField passwordField;

    public GameObject EnigmaTelephones;
    public GameObject EnigmaTelephoneAdd;

    public TMP_Dropdown ports;
    public TMP_InputField number;

    public TMP_InputField newTKNumber;
    public GameObject confirmNewNumbButton;

    public TextMeshProUGUI[] newValues = new TextMeshProUGUI[4];

    public static string newTKNumb = string.Empty;

    public bool isHaveNumber = false;

    string[] numbsOnServer = { "1234" };

    // Start is called before the first frame update
    void Start()
    {
        //address = System.IO.File.ReadAllLines(path)[0];
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    public void AddAll()
    {
        GameManagerScript.currentHint = -1;
        for (int i = 0; i < GameManagerScript.phonesNumbers.Length; i++)
        {
            AddTelephoneToServer(GameManagerScript.phonesNumbers[i]);
        }
    }

    public void CheckNewNumb()
    {
        for(int i = 0; i < numbsOnServer.Length; i++)
        {
            if(numbsOnServer[i].Equals(newTKNumber.text))
            {
                isHaveNumber = true;
                break;
            }
            else
            {
                isHaveNumber = false;
            }
        }

        if(!isHaveNumber)
        {
            confirmNewNumbButton.SetActive(true);
            ShowNewValues();
            GameManagerScript.currentHint = 18;
        }
    }

    public void ShowNewValues()
    {
        if (newTKNumber.text.Equals("0000"))
        {
            newValues[0].text = "10.0.0.2";
            newValues[1].text = "10.0.0.4";
            newValues[2].text = "OK";
            newValues[3].text = "OK";
        }
        else
        {
            newValues[0].text = $"185.{newTKNumber.text[0]}{newTKNumber.text[1]}.{newTKNumber.text[2]}{newTKNumber.text[3]}.2";
            newValues[1].text = $"185.{newTKNumber.text[0]}{newTKNumber.text[1]}.{newTKNumber.text[2]}{newTKNumber.text[3]}.4";
            newValues[2].text = "OK";
            newValues[3].text = "OK";
        }
    }

    public void EnigmaTelephoneSettingWindow()
    {
        GameManagerScript.currentHint = 22;
        GameManagerScript.tknumb = newTKNumb;
        AddNewStationToServer(GameManagerScript.tknumb);
        DisableAll();
        EnigmaTelephones.SetActive(true);
    }

    public void AddNewStationToServer(string numb)
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "station, " + numb;
            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);

            // получаем ответ
            data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket.Available > 0);

            // закрываем сокет
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void EnigmaFifthSettingWindow()
    {
        GameManagerScript.currentHint = 21;
        DisableAll();
        enigmaSettings[4].SetActive(true);
    }

    public void EnigmaFourthSettingWindow()
    {
        GameManagerScript.currentHint = 20;
        DisableAll();
        enigmaSettings[3].SetActive(true);
    }

    public void EnigmaThirdSettingWindow()
    {
        GameManagerScript.currentHint = 19;
        newTKNumb = newTKNumber.text;
        DisableAll();
        enigmaSettings[2].SetActive(true);
    }

    public void EnigmaSecondSettingOpen()
    {
        GameManagerScript.currentHint = 17;
        DisableAll();
        enigmaSettings[1].SetActive(true);
    }

    public void AddTelephoneToServer(string numb)
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "add, " + GameManagerScript.tknumb + ", " + numb;
            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);

            // получаем ответ
            data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket.Available > 0);

            // закрываем сокет
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    public void OpenTelephoneAdd()
    {
        GameManagerScript.currentHint = 23;
        DisableAll();
        EnigmaTelephoneAdd.SetActive(true);
    }

    public void TelephoneCancel()
    {
        number.text = "";
        DisableAll();
        EnigmaTelephones.SetActive(true);
    }

    public void TelephoneAdd()
    {
        GameManagerScript.currentHint = 22;
        GameManagerScript.phonesNumbers[GameManagerScript.currentPhones] = number.text;
        switch(ports.value)
        {
            case 0:
                GameManagerScript.firstGrandstreamPort = true;
                GameManagerScript.phonesPorts[GameManagerScript.currentPhones] = "1(Grandstream HT704)";
                break;
            case 1:
                GameManagerScript.secondGrandstreamPort = true;
                GameManagerScript.phonesPorts[GameManagerScript.currentPhones] = "2(Grandstream HT704)";
                break;
            case 2:
                GameManagerScript.thirdGrandstreamPort = true;
                GameManagerScript.phonesPorts[GameManagerScript.currentPhones] = "3(Grandstream HT704)";
                break;
            case 3:
                GameManagerScript.fourthGrandstreamPort = true;
                GameManagerScript.phonesPorts[GameManagerScript.currentPhones] = "4(Grandstream HT704)";
                break;
        }
        GameManagerScript.currentPhones++;
        DisableAll();
        EnigmaTelephones.SetActive(true);
    }

    public void OpenMainMenu()
    {
        GameManagerScript.currentHint = 7;
        DisableAll();
        mainMenuScreen.SetActive(true);
    }

    public void CloseMainMenu()
    {
        DisableAll();
        mainScreen.SetActive(true);
    }

    public void OpenControlPanel()
    {
        GameManagerScript.currentHint = 8;
        DisableAll();
        controlPanelScreen.SetActive(true);
    }

    public void OpenNetworkSetting()
    {
        GameManagerScript.currentHint = 9;
        DisableAll();
        networkWindows[0].SetActive(true);
    }

    public void OpenSecondNetworkWindow()
    {
        GameManagerScript.currentHint = 10;
        DisableAll();
        networkWindows[1].SetActive(true);
    }

    public void OpenAdapterSetting()
    {
        GameManagerScript.currentHint = 11;
        DisableAll();
        networkWindows[2].SetActive(true);
    }

    public void OpenIPSetting()
    {
        GameManagerScript.currentHint = 12;
        DisableAll();
        ipAddressWindow.SetActive(true);
    }

    public void ConfirmAddressSetting()
    {
        GameManagerScript.currentHint = 13;

        GameManagerScript.ipLaptopAddress = IpAddress[0].text + "." + IpAddress[1].text + "." +
                                            IpAddress[2].text + "." + IpAddress[3].text;
        GameManagerScript.ipLaptopMask = MaskAddress[0].text + "." + MaskAddress[1].text + "." +
                                         MaskAddress[2].text + "." + MaskAddress[3].text;

        Debug.Log(GameManagerScript.ipLaptopAddress);
        Debug.Log(GameManagerScript.ipLaptopMask);

        DisableAll();
        networkWindows[2].SetActive(true);
    }

    public void BrowserOpen()
    {
        GameManagerScript.currentHint = 14;
        DisableAll();
        browser.SetActive(true);
    }
    
    public void EnigmaOpen()
    {
        if(GameManagerScript.tknumb == "0000")
        {
            if(searchString.text.Equals("10.0.0.3"))
            {
                DisableAll();
                EnigmaAuthorization.SetActive(true);
                GameManagerScript.currentHint = 15;
            }
            else
            {
                DisableAll();
                browserError.SetActive(true);
            }
        }
        else
        {
            if (searchString.text.Equals($"185.{GameManagerScript.tknumb[0]}{GameManagerScript.tknumb[1]}.{GameManagerScript.tknumb[2]}{GameManagerScript.tknumb[3]}.3"))
            {
                DisableAll();
                EnigmaAuthorization.SetActive(true);
                GameManagerScript.currentHint = 15;
            }
            else
            {
                DisableAll();
                browserError.SetActive(true);
            }
        }
    }

    public void EnigmaErrorOpen()
    {
        if (GameManagerScript.tknumb == "0000")
        {
            if (searchStringError.text.Equals("10.0.0.3"))
            {
                DisableAll();
                EnigmaAuthorization.SetActive(true);
                GameManagerScript.currentHint = 15;
            }
            else
            {
                DisableAll();
                browserError.SetActive(true);
            }
        }
        else
        {
            if (searchStringError.text.Equals($"185.{GameManagerScript.tknumb[0]}{GameManagerScript.tknumb[1]}.{GameManagerScript.tknumb[2]}{GameManagerScript.tknumb[3]}.3"))
            {
                DisableAll();
                EnigmaAuthorization.SetActive(true);
                GameManagerScript.currentHint = 15;
            }
            else
            {
                DisableAll();
                browserError.SetActive(true);
            }
        }
    }

    public void EnigmaLogin()
    {
        if(loginField.text.Equals(GameManagerScript.loginEverest) && passwordField.text.Equals(GameManagerScript.passEverest))
        {
            DisableAll();
            enigmaSettings[0].SetActive(true);
            GameManagerScript.currentHint = 16;
        }
    }

    public void DisableAll()
    {
        mainScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        controlPanelScreen.SetActive(false);

        for(int i = 0; i < networkWindows.Length; i++)
        {
            networkWindows[i].SetActive(false);
        }

        ipAddressWindow.SetActive(false);
        browser.SetActive(false);
        browserError.SetActive(false);

        EnigmaAuthorization.SetActive(false);

        EnigmaTelephones.SetActive(false);
        EnigmaTelephoneAdd.SetActive(false);

        for(int i = 0; i < enigmaSettings.Length; i++)
        {
            enigmaSettings[i].SetActive(false);
        }
    }

}
