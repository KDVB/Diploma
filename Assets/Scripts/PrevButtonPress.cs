using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevButtonPress : MonoBehaviour
{
    public GameObject button;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            button.GetComponent<Animation>().Play("PrevButtonAnim");
            if (GameManagerScript.isLoad)
            {
                GameObject.Find("GameManager").GetComponent<GameManagerScript>().PrevMenuItem();
                if (GameManagerScript.currentMenuItem.Equals(2))
                {
                    GameManagerScript.currentHint = 4;
                }
                else
                {
                    GameManagerScript.currentHint = 3;
                }
            }
        }
    }
}
