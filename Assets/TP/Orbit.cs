using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * 5);
        }
        if (Input.GetKey(KeyCode.Q)) {
            transform.Translate(Vector3.left * Time.deltaTime * 5);
        }
        transform.LookAt(target);
    }
}
