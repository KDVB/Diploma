using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTelephoneLineConnect : MonoBehaviour
{
    public GameObject telephoneCables;
    bool isPresed = false;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManagerScript.isLoad)
            {
                if (!isPresed)
                {
                    telephoneCables.GetComponent<Animation>().PlayQueued("FirstLineIn");
                    GameManagerScript.isFirstTelephoneLine = true;
                    isPresed = true;
                }
                else if (isPresed)
                {
                    telephoneCables.GetComponent<Animation>().PlayQueued("FirstLineOut");
                    GameManagerScript.isFirstTelephoneLine = false;
                    isPresed = false;
                }
            }
        }
    }
}
