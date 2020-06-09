using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanConnector : MonoBehaviour
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
                    cable.GetComponent<Animation>().PlayQueued("WanIn");
                    GameManagerScript.isWan = true;
                    isPresed = true;
                }
                else if (isPresed)
                {
                    cable.GetComponent<Animation>().PlayQueued("WanOut");
                    GameManagerScript.isWan = false;
                    isPresed = false;
                }
            }
        }
    }
}
