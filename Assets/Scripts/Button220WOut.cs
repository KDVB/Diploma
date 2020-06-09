using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button220WOut : MonoBehaviour
{
    public GameObject button;

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
            button.GetComponent<Animation>().Play("220ButtonAnim");
            if (GameManagerScript.isLoad)
            {

            }
        }
    }
}
