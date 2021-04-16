using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGravity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<Asteroid>())
        {
            Debug.Log("Asteroid entered" + c);
            //Esto no funciona lol
            //c.transform.position = c.transform.position - asteroid.transform.position;
        }
    }
}
