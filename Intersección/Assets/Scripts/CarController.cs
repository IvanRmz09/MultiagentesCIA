using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Vector3 newPosition;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(newPosition);
        transform.position = newPosition;
    }
}
