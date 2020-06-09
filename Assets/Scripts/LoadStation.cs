using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStation : MonoBehaviour
{
    public GameObject[] loadScreens = new GameObject[7];
    public GameObject screen;
    public GameObject loadTK;

    private float startTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        screen.SetActive(false);
        loadScreens[0].SetActive(true);
        GameManagerScript.currentHint = 2;
    }

    private void FixedUpdate()
    {
        if(Time.time - startTime > 2f && Time.time - startTime < 4f)
        {
            loadScreens[0].SetActive(false);
            loadScreens[1].SetActive(true);
        }
        else if (Time.time - startTime < 5f)
        {
            loadScreens[1].SetActive(false);
            loadScreens[2].SetActive(true);
        }
        else if (Time.time - startTime < 7f)
        {
            loadScreens[2].SetActive(false);
            loadScreens[3].SetActive(true);
        }
        else if (Time.time - startTime < 9f)
        {
            loadScreens[3].SetActive(false);
            loadScreens[4].SetActive(true);
        }
        else if (Time.time - startTime < 11f)
        {
            loadScreens[4].SetActive(false);
            loadScreens[5].SetActive(true);
        }
        else if (Time.time - startTime < 13f)
        {
            loadScreens[5].SetActive(false);
            loadScreens[6].SetActive(true);
        }
        else if (Time.time - startTime < 15f)
        {
            loadScreens[6].SetActive(false);
            loadTK.SetActive(true);
            gameObject.SetActive(false);

        }
    }
}
