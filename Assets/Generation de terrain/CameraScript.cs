using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public float RotateSpeed;
    public float MoveSpeed;
    private const float MinOffset = 1f;
    void Update()
    {
        // Camera Orbit
        if (Input.GetKey(KeyCode.D))
        {
            if(Input.GetKey(KeyCode.LeftShift))
                target.transform.Rotate(new Vector3(0, -1, 0), RotateSpeed, Space.Self);
            else
                target.transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.Q)) {
            if (Input.GetKey(KeyCode.LeftShift))
                target.transform.Rotate(new Vector3(0, 1, 0), RotateSpeed, Space.Self);
            else
                target.transform.Translate(Vector3.left * Time.deltaTime * MoveSpeed);
        }

        // Camera Zoom & movement
        if (Input.GetKey(KeyCode.Z))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                transform.Translate(Vector3.forward * Time.deltaTime * RotateSpeed);
            else if (Vector3.Distance(gameObject.transform.position, target.transform.position) > MinOffset)
                target.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                transform.Translate(Vector3.back * Time.deltaTime * RotateSpeed);
            else
                target.transform.Translate(Vector3.back * Time.deltaTime * MoveSpeed);
        }
        transform.LookAt(target);
    }
}
