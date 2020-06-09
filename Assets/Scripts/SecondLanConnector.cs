using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLanConnector : MonoBehaviour
{
    public GameObject cable;
    bool isPresed = false;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManagerScript.isLoad)
            {
                if (!isPresed)
                {
                    cable.GetComponent<Animation>().PlayQueued("SecondLanIn");
                    GameManagerScript.isSecondLan = true;
                    isPresed = true;
                }
                else if (isPresed)
                {
                    cable.GetComponent<Animation>().PlayQueued("SecondLanOut");
                    GameManagerScript.isSecondLan = false;
                    isPresed = false;
                }
            }
        }
    }
}
