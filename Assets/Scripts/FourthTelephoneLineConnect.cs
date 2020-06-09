using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthTelephoneLineConnect : MonoBehaviour
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
                    telephoneCables.GetComponent<Animation>().PlayQueued("FourthLineIn");
                    GameManagerScript.isFourthTelephoneLine = true;
                    isPresed = true;
                }
                else if (isPresed)
                {
                    telephoneCables.GetComponent<Animation>().PlayQueued("FourthLineOut");
                    GameManagerScript.isFourthTelephoneLine = false;
                    isPresed = false;
                }
            }
        }
    }
}
