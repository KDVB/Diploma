using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LoadTK : MonoBehaviour
{

    public TextMeshProUGUI timer;
    public GameObject timerWindow;
    public GameObject firstMenuScreen;

    private float startTime = 0f;
    private int time = 0;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        time = 10;
        timerWindow.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time - startTime < 10f)
        {
            int temp = Convert.ToInt32(Time.time - startTime);
            timer.text = $"Готовність - {time - temp}c.";
        }
        else
        {
            timerWindow.SetActive(false);
            firstMenuScreen.SetActive(true);
            GameManagerScript.isLoad = true;
            gameObject.SetActive(false);
            GameManagerScript.currentHint = 3;
        }
    }
}
