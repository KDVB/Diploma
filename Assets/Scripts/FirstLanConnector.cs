using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLanConnector : MonoBehaviour
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
                    cable.GetComponent<Animation>().PlayQueued("FirstLanIn");
                    GameManagerScript.isFirstLan = true;
                    isPresed = true;
                }
                else if (isPresed)
                {
                    cable.GetComponent<Animation>().PlayQueued("FirstLanOut");
                    GameManagerScript.isFirstLan = false;
                    isPresed = false;
                }
            }
        }
    }
}
