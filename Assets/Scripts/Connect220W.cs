using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect220W : MonoBehaviour
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
            if (GameManagerScript.is220WConnect)
            {
                cable.GetComponent<Animation>().Play("PowerCableOut");
                GameManagerScript.currentHint = 0;
                GameManagerScript.is220WConnect = false;
            }
            else
            {
                cable.GetComponent<Animation>().Play("PowerCableIn");
                GameManagerScript.currentHint = 1;
                GameManagerScript.is220WConnect = true;
            }
        }
    }
}
