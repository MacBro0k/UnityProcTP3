using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private float mZCoord;
    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        // Get the distance between the camera and object
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // Keep track of the mouse position
        Vector3 mousePos = GetMouseAsWorldPoint();
        // Move object to mouse position
        gameObject.transform.position = mousePos;

    }
}
