using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect30W : MonoBehaviour
{
    public GameObject cable;

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
            if (GameManagerScript.is30WConnect)
            {
                cable.GetComponent<Animation>().Play("PowerCable30WOut");
                GameManagerScript.is30WConnect = false;
            }
            else
            {
                cable.GetComponent<Animation>().Play("PowerCable30WIn");
                GameManagerScript.is30WConnect = true;
            }
        }
    }
}
