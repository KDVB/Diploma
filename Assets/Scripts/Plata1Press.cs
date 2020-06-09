using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plata1Press : MonoBehaviour
{
    bool isPresed = false;
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!isPresed)
            {
                AssembleGameManager.currentPlata = 0;
                GameObject.Find("AssembleGameManager").GetComponent<AssembleGameManager>().FocusPlata(gameObject);
                isPresed = true;
            }
            else if (isPresed)
            {
                AssembleGameManager.currentPlata = 0;
                GameObject.Find("AssembleGameManager").GetComponent<AssembleGameManager>().UnfocusPlata(gameObject);
                isPresed = false;
            }
        }
    }
}
