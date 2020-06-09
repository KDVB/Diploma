using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdTelephoneLineConnect : MonoBehaviour
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
                    telephoneCables.GetComponent<Animation>().PlayQueued("ThirdLineIn");
                    GameManagerScript.isThirdTelephoneLine = true;
                    isPresed = true;
                }
                else if (isPresed)
                {
                    telephoneCables.GetComponent<Animation>().PlayQueued("ThirdLineOut");
                    GameManagerScript.isThirdTelephoneLine = false;
                    isPresed = false;
                }
            }
        }
    }
}
