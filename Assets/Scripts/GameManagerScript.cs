using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public string path = Environment.CurrentDirectory + "\\ipAddress.txt";
    public static int currentPlata = 0;
    public GameObject station;
    public GameObject stationAll;
    public GameObject laptop;
    public Image background;
    public GameObject loadStation;

    public static bool isFirstTelephoneLine = false;
    public static bool isSecondTelephoneLine = false;
    public static bool isThirdTelephoneLine = false;
    public static bool isFourthTelephoneLine = false;

    public static bool isWan = false;
    public static bool isFirstLan = false;
    public static bool isSecondLan = false;
    public static bool isThirdLan = false;
    public static bool isFourthLan = false;
    public static bool isFifthLan = false;

    public static bool isLoad = false;

    public static string currentPhoneUse = string.Empty;

    public GameObject[] tkMenus = new GameObject[4];
    public static int currentMenuItem = 0;

    public static bool isAssemble = false;

    public static bool is220WConnect = false;
    public static bool is30WConnect = false;

    public static string ipLaptopAddress = "";
    public static string ipLaptopMask = "";

    public static string tknumb = "0000";

    public static string loginEverest = "admin";
    public static string passEverest = "everest";

    public static string[] phonesNumbers = new string[4];
    public static string[] phonesPorts = new string[4];
    public static bool firstGrandstreamPort = false;
    public static bool secondGrandstreamPort = false;
    public static bool thirdGrandstreamPort = false;
    public static bool fourthGrandstreamPort = false;

    public static int currentPhones = 0;

    public static bool isStation = true;
    public static bool isLaptop = false;
    public static bool isSpeeker = false;

    public GameObject[] voiceController = new GameObject[4];
    
    public Button stationButton;
    public Button laptopButton;
    public Button speekerButton;

    public static string phoneIP = "";

    public Color32 greenLight = new Color32(136, 196, 0, 255);
    public Color32 blackLight = new Color32(40, 40, 40, 255);

    public GameObject reciever;

    public GameObject callMenu;

    public static int currentHint = 0;

    public Sprite eyeOpen;
    public Sprite eyeCross;

    private Color32 grey = new Color32(130, 130, 130, 255);
    private Color32 lightGreen = new Color32(0, 177, 8, 255);

    public static bool isHint = false;
    public Image spriteImage;

    public GameObject hints;

    public string address;

    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(path))
        {
            address = System.IO.File.ReadAllLines(path)[0];
        }

        Debug.Log(address);
        //station.GetComponent<Animation>().Play("AssemblyStation");
        //fisAssemble = true;
        //loadStation.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void SetCallMenu()
    {
        callMenu.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isStation)
        {
            stationButton.GetComponent<Image>().color = greenLight;
            laptopButton.GetComponent<Image>().color = blackLight;
            speekerButton.GetComponent<Image>().color = blackLight;
        }
        if(isLaptop)
        {
            stationButton.GetComponent<Image>().color = blackLight;
            laptopButton.GetComponent<Image>().color = greenLight;
            speekerButton.GetComponent<Image>().color = blackLight;
        }
        if(isSpeeker)
        {
            stationButton.GetComponent<Image>().color = blackLight;
            laptopButton.GetComponent<Image>().color = blackLight;
            speekerButton.GetComponent<Image>().color = greenLight;
        }
    }

    public void ActivateHints()
    {
        if (!isHint)
        {
            spriteImage.overrideSprite = eyeOpen;
            spriteImage.color = lightGreen;
            hints.SetActive(true);
            isHint = true;
        }
        else
        {
            spriteImage.overrideSprite = eyeCross;
            spriteImage.color = grey;
            hints.SetActive(false);
            isHint = false;
        }
    }

    public void SetPhoneIp()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                phoneIP = ip.ToString();
                break;
            }
        }
    }

    public void EnableVoice()
    {
        for(int i = 0; i < voiceController.Length; i++)
        {
            voiceController[i].SetActive(true);
        }
        reciever.SetActive(true);
    }

    public void DisableVoice()
    {
        for (int i = 0; i < voiceController.Length; i++)
        {
            voiceController[i].SetActive(false);
        }
        reciever.SetActive(false);
    }

    public void ChangeToStation()
    {
        if(isStation)
        {

        }
        else if(isLaptop)
        {
            laptop.GetComponent<Animation>().Play("LaptopOut");
            stationAll.GetComponent<Animation>().Play("TKIn");
            isStation = true;
            isLaptop = false;
            isSpeeker = false;
        }
        else if (isSpeeker)
        {
            DisableVoice();
            stationAll.GetComponent<Animation>().Play("TKIn");
            isStation = true;
            isLaptop = false;
            isSpeeker = false;
        }
    }

    public void ChangeToLaptop()
    {
        currentHint = 6;
        if (isLaptop)
        {

        }
        else if(isStation)
        {
            laptop.GetComponent<Animation>().Play("LaptopIn");
            stationAll.GetComponent<Animation>().Play("TKOut");
            isStation = false;
            isLaptop = true;
            isSpeeker = false;
        }
        else if (isSpeeker)
        {
            laptop.GetComponent<Animation>().Play("LaptopIn");
            DisableVoice();
            isStation = false;
            isLaptop = true;
            isSpeeker = false;
        }
    }

    public void ChangeToSpeeker()
    {
        if (isSpeeker)
        {

        }
        else if(isStation)
        {
            stationAll.GetComponent<Animation>().Play("TKOut");
            EnableVoice();
            isStation = false;
            isLaptop = false;
            isSpeeker = true;
        }
        else if (isLaptop)
        {
            laptop.GetComponent<Animation>().Play("LaptopOut");
            EnableVoice();
            isStation = false;
            isLaptop = false;
            isSpeeker = true;
        }
    }

    public void NextMenuItem()
    {
        if(currentMenuItem != tkMenus.Length - 1)
        {
            tkMenus[currentMenuItem].SetActive(false);
            tkMenus[currentMenuItem + 1].SetActive(true);
            currentMenuItem++;
        }
        else
        {
            tkMenus[currentMenuItem].SetActive(false);
            tkMenus[0].SetActive(true);
            currentMenuItem = 0;
        }
    }

    public void PrevMenuItem()
    {
        if (currentMenuItem != 0)
        {
            tkMenus[currentMenuItem].SetActive(false);
            tkMenus[currentMenuItem - 1].SetActive(true);
            currentMenuItem--;
        }
        else
        {
            tkMenus[currentMenuItem].SetActive(false);
            tkMenus[tkMenus.Length - 1].SetActive(true);
            currentMenuItem = tkMenus.Length - 1;
        }
    }

    public void FocusPlata(GameObject gameObject)
    {
        if (isAssemble)
        {
            switch (currentPlata)
            {
                case 0:
                    gameObject.GetComponent<Animation>().Play("FocusPlata1");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    break;
                case 1:
                    gameObject.GetComponent<Animation>().Play("FocusPlata2");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    break;
                case 2:
                    gameObject.GetComponent<Animation>().Play("FocusPlata3");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    break;
                case 3:
                    gameObject.GetComponent<Animation>().Play("FocusPlata4");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    break;
                case 4:
                    gameObject.GetComponent<Animation>().Play("FocusPlata5");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    break;
                case 5:
                    gameObject.GetComponent<Animation>().Play("FocusPlata6");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    break;
                case 6:
                    gameObject.GetComponent<Animation>().Play("FocusPlataZh");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    break;
            }
        }
    }

    public void UnfocusPlata(GameObject gameObject)
    {
        if (isAssemble)
        {
            switch (currentPlata)
            {
                case 0:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlata1");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    break;
                case 1:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlata2");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    break;
                case 2:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlata3");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    break;
                case 3:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlata4");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    break;
                case 4:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlata5");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    break;
                case 5:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlata6");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    break;
                case 6:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlataZh");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    break;
            }
        }
    }
}
