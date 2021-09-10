using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaforo : MonoBehaviour
{
    public GameObject semaforo;
    // Start is called before the first frame update
    void Start()
    {
        semaforo.transform.GetChild(0).gameObject.SetActive(false);
        semaforo.transform.GetChild(1).gameObject.SetActive(false);
        semaforo.transform.GetChild(2).gameObject.SetActive(true);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
