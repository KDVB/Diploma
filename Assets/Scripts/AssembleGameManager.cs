using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssembleGameManager : MonoBehaviour
{
    public static int currentPlata = 0;
    public GameObject station;
    public Image background;

    public GameObject[] platainfo = new GameObject[4];

    public bool isAssemble = false;

    public Image spriteImage;
    public Sprite assembly;
    public Sprite dissassembly;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void StationManipulation()
    {
        if(!isAssemble)
        {
            DisassembleStation();
        }
        else
        {
            AssembleStation();
        }
    }

    public void DisassembleStation()
    {
        Animation anim = station.GetComponent<Animation>();
        anim["AssemblyStation"].speed = 1;
        anim["AssemblyStation"].time = 0;
        anim.Play("AssemblyStation");

        spriteImage.overrideSprite = dissassembly;

        isAssemble = true;
    }

    public void AssembleStation()
    {
        
        Animation anim = station.GetComponent<Animation>();
        anim["AssemblyStation"].speed = -1;
        anim["AssemblyStation"].time = anim["AssemblyStation"].length;
        anim.Play("AssemblyStation");

        spriteImage.overrideSprite = assembly;

        isAssemble = false;
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
                    platainfo[0].GetComponent<Animation>().Play("TextIn");
                    break;
                case 2:
                    gameObject.GetComponent<Animation>().Play("FocusPlata3");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    platainfo[1].GetComponent<Animation>().Play("TextIn");
                    break;
                case 3:
                    gameObject.GetComponent<Animation>().Play("FocusPlata4");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    platainfo[2].GetComponent<Animation>().Play("TextIn");
                    break;
                case 4:
                    gameObject.GetComponent<Animation>().Play("FocusPlata5");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    break;
                case 5:
                    gameObject.GetComponent<Animation>().Play("FocusPlata6");
                    background.GetComponent<Animation>().Play("FocusPlataBackground");
                    platainfo[3].GetComponent<Animation>().Play("TextIn");
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
                    platainfo[0].GetComponent<Animation>().Play("TextOut");
                    break;
                case 2:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlata3");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    platainfo[1].GetComponent<Animation>().Play("TextOut");
                    break;
                case 3:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlata4");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    platainfo[2].GetComponent<Animation>().Play("TextOut");
                    break;
                case 4:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlata5");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    break;
                case 5:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlata6");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    platainfo[3].GetComponent<Animation>().Play("TextOut");
                    break;
                case 6:
                    gameObject.GetComponent<Animation>().Play("UnfocusPlataZh");
                    background.GetComponent<Animation>().Play("UnfocusPlataBackground");
                    break;
            }
        }
    }

}
