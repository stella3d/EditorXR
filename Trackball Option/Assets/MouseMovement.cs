using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public Transform trackball;
    public Transform ball;
    public GameObject instructions;
    public float movementMultiplier = .1f;

    // Use this for initialization
    void Start()
    {

    }

    private void ToggleInstructions()
    {
        instructions.SetActive(!instructions.activeSelf);
    }

    int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1) ||
            Input.GetMouseButton(0) && Input.GetMouseButtonDown(1)||
            Input.GetMouseButtonDown(0) && Input.GetMouseButton(1))
        {
            ToggleInstructions();
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 forward = Vector3.Scale(transform.forward, new Vector3(1f, 0f, 1f)).normalized;
            Vector3 right = Vector3.Scale(transform.right, new Vector3(1f, 0f, 1f)).normalized;

            transform.position += (Input.GetAxis("Mouse X") * right + Input.GetAxis("Mouse Y") * forward) * movementMultiplier;

            //Update trackball
            ball.rotation = Quaternion.AngleAxis(- Input.GetAxis("Mouse X") * movementMultiplier * 10f, trackball.forward)
                * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * movementMultiplier * 10f, trackball.right) * ball.rotation;
        }

        transform.position += Input.mouseScrollDelta.y * Vector3.up * 0.1f;
        //    if (selected == null)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        //Try to select
        //        RaycastHit hit;
        //        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        //        {
        //            obj.position = hit.point;
        //            selected = hit.transform;
        //            mode = 0;
        //        }
        //    }
        //}
        //else
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        //Deselect
        //        selected = null;
        //        return;
        //    }

        //    if (Input.mouseScrollDelta.y > 0f)
        //    {
        //        mode = Mod(mode + 1, 3);
        //    }
        //    if (Input.mouseScrollDelta.y < 0f)
        //    {
        //        mode = Mod(mode - 1, 3);
        //    }

        //    switch (mode)
        //    {
        //        case 0:
        //            if (!Input.GetMouseButton(1))
        //            {
        //                selected.position += Input.GetAxis("Mouse X") * transform.TransformDirection(Vector3.right) + Input.GetAxis("Mouse Y") * transform.TransformDirection(Vector3.forward);
        //            }
        //            else
        //            {
        //                selected.position += Input.GetAxis("Mouse Y") * transform.TransformDirection(Vector3.up);
        //            }
        //            break;
        //        case 1:
        //            if (!Input.GetMouseButton(1))
        //            {
        //                selected.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X"), 0f);
        //            }
        //            else
        //            {
        //                selected.rotation *= Quaternion.Euler(Input.GetAxis("Mouse X"), 0f, Input.GetAxis("Mouse Y"));
        //            }
        //            break;
        //        case 2:
        //            selected.localScale += Input.GetAxis("Mouse X") * Vector3.one;
        //            break;

        //    }

        //}
    }
}
