using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Pivot;
    public float RotateSpeed;
    public float MoveSpeed;
    private const float MinOffset = 1f;
    private Vector3 TargetPos;
    public float Altitude = 5f;
    void Update()
    {
        // MoveSpeed change selon la distance entre la position désirée et la position actuelle
        MoveSpeed = 1f * Vector3.Distance(gameObject.transform.position, Pivot.transform.position);

        RaycastHit hit;
        Debug.DrawRay(Pivot.transform.position, transform.TransformDirection(Vector3.down) * Mathf.Infinity, Color.yellow);
        if (Physics.Raycast(Pivot.transform.position+Pivot.transform.position.y*Vector3.up*Mathf.Infinity,Pivot.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            if (Pivot.transform.position.y != hit.transform.position.y)
            {
                Pivot.transform.position = new Vector3(Pivot.transform.position.x, hit.transform.position.y, Pivot.transform.position.z);
            }

        }

        if (Vector3.Distance(Pivot.transform.position,TargetPos) >= 1f)
            Pivot.transform.position = Vector3.MoveTowards(Pivot.transform.position, TargetPos, (MoveSpeed * Vector3.Distance(Pivot.transform.position, TargetPos)) * Time.deltaTime);
        else
        {
            // Camera orbit & movement
            if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    Pivot.transform.Rotate(new Vector3(0, -1, 0), RotateSpeed, Space.Self);
                else
                    Pivot.transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    Pivot.transform.Rotate(new Vector3(0, 1, 0), RotateSpeed, Space.Self);
                else
                    Pivot.transform.Translate(Vector3.left * Time.deltaTime * MoveSpeed);
            }


            if (Input.GetKey(KeyCode.Z))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);

                else if (Vector3.Distance(gameObject.transform.position, Pivot.transform.position) > MinOffset)
                    Pivot.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    transform.Translate(Vector3.back * Time.deltaTime * MoveSpeed);

                else
                    Pivot.transform.Translate(Vector3.back * Time.deltaTime * MoveSpeed);
            }

            TargetPos = Pivot.transform.position;

            //Camera drag
            if (Input.GetMouseButtonDown(1))
            {
                TargetPos = GetMouseAsWorldPoint();
            }
        }

        transform.LookAt(Pivot);
    }


    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        Vector3 mouse3dPos;
        Ray castPoint = Camera.main.ScreenPointToRay(mousePoint);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            mouse3dPos= hit.point;
        }
        else
        {
            mouse3dPos= Pivot.transform.position;
        }
        return mouse3dPos;
    }
}
