using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{

    public Transform obj;
    private Transform selected;

    private int mode = 0;

    // Use this for initialization
    void Start()
    {

    }

    int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Try to select
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
                {
                    obj.position = hit.point;
                    selected = hit.transform;
                    mode = 0;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Deselect
                selected = null;
                return;
            }

            if (Input.mouseScrollDelta.y > 0f)
            {
                mode = Mod(mode + 1, 3);
            }
            if (Input.mouseScrollDelta.y < 0f)
            {
                mode = Mod(mode - 1, 3);
            }

            switch (mode)
            {
                case 0:
                    if (!Input.GetMouseButton(1))
                    {
                        selected.position += Input.GetAxis("Mouse X") * transform.TransformDirection(Vector3.right) + Input.GetAxis("Mouse Y") * transform.TransformDirection(Vector3.forward);
                    }
                    else
                    {
                        selected.position += Input.GetAxis("Mouse Y") * transform.TransformDirection(Vector3.up);
                    }
                    break;
                case 1:
                    if (!Input.GetMouseButton(1))
                    {
                        selected.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X"), 0f);
                    }
                    else
                    {
                        selected.rotation *= Quaternion.Euler(Input.GetAxis("Mouse X"), 0f, Input.GetAxis("Mouse Y"));
                    }
                    break;
                case 2:
                    selected.localScale += Input.GetAxis("Mouse X") * Vector3.one;
                    break;

            }

        }
    }
}
