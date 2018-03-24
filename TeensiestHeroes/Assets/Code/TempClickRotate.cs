using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempClickRotate : MonoBehaviour
{
    public float speed;
    public float lerpSpeed;
    private float xDeg;
    private float yDeg;
    private Quaternion fromRotation;
    private Quaternion toRotation;
 
    public void Update()
    {
        fromRotation = transform.rotation;
        if (Input.GetMouseButton(0))
        {

            xDeg -= Input.GetAxis("Mouse X") * speed;
            yDeg += Input.GetAxis("Mouse Y") * speed;
            
            toRotation = Quaternion.Euler(yDeg, xDeg, 0);
        }
        transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);
    }
}
