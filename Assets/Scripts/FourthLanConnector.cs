using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthLanConnector : MonoBehaviour
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
                    cable.GetComponent<Animation>().PlayQueued("FourthLanIn");
                    GameManagerScript.isFourthLan = true;
                    isPresed = true;
                }
                else if (isPresed)
                {
                    cable.GetComponent<Animation>().PlayQueued("FourthLanOut");
                    GameManagerScript.isFourthLan = false;
                    isPresed = false;
                }
            }
        }
    }
}
