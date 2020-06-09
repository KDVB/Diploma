using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStation : MonoBehaviour
{
    public GameObject station;
    public Quaternion originRotation;
    Quaternion currentRotation;
    Vector3 originScale;

    float angle;
    bool press = false;
    bool rotate = true;
    float speed = 60f;

    // Start is called before the first frame update
    void Start()
    {
        originScale = transform.localScale;
        originRotation = station.transform.rotation;
        
    }

    private void FixedUpdate()
    {
        if (rotate)
        {
            angle++;
            station.transform.rotation = originRotation * Quaternion.AngleAxis(angle, Vector3.up);
        }
        if (!press && !rotate)
        {
            var step = speed * Time.deltaTime;
            station.transform.rotation = Quaternion.RotateTowards(station.transform.rotation, originRotation, step);
            if (station.transform.rotation == originRotation)
            {
                rotate = true;
                angle = 0;
            }
        }
    }


    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            press = true;
            rotate = false;
            station.transform.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * 5, Space.World);
            station.transform.Rotate(Vector3.down, Input.GetAxis("Mouse X") * 5, Space.World);
        }
        if (Input.GetMouseButtonDown(0))
        {
            transform.localScale = new Vector3(10, 10, 10);
        }
        if (Input.GetMouseButtonUp(0))
        {
            transform.localScale = originScale;
            press = false;
        }
    }
}
