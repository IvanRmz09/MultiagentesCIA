using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void OnCollisionEnter(Collision enemy)
    {
        if (enemy.gameObject.tag == "Car")
        {
            Destroy(enemy.gameObject);
        }
    }
}
