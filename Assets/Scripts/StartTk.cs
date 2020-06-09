using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTk : MonoBehaviour
{

    public GameObject startButton;
    public GameObject loadMenu;

    private bool isPressed = false;
    private float timeStart = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isPressed)
        {
            if(Time.time - timeStart >= 4f)
            {
                loadMenu.SetActive(true);
                
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startButton.GetComponent<Animation>().Play("OnOffButtonDownAnim");
            timeStart = Time.time;
            isPressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            startButton.GetComponent<Animation>().Play("OnOffButtonUpAnim");
            timeStart = 0f;
            isPressed = false;
        }
    }

}
