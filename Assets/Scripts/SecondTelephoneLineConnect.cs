using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTelephoneLineConnect : MonoBehaviour
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
                    telephoneCables.GetComponent<Animation>().PlayQueued("SecondLineIn");
                    GameManagerScript.isSecondTelephoneLine = true;
                    isPresed = true;
                }
                else if (isPresed)
                {
                    telephoneCables.GetComponent<Animation>().PlayQueued("SecondLineOut");
                    GameManagerScript.isSecondTelephoneLine = false;
                    isPresed = false;
                }
            }
        }
    }
}
