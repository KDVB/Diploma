using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FifthLanConnector : MonoBehaviour
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
                    cable.GetComponent<Animation>().PlayQueued("FifthLanIn");
                    GameManagerScript.isFifthLan = true;
                    isPresed = true;
                }
                else if (isPresed)
                {
                    cable.GetComponent<Animation>().PlayQueued("FifthLanOut");
                    GameManagerScript.isFifthLan = false;
                    isPresed = false;
                }
            }
        }
    }
}
