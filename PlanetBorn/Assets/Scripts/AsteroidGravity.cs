using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGravity : MonoBehaviour
{
    public float fuerza;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<Asteroid>())
        {
            Debug.Log("Asteroid entered" + c);
            //Esto no funciona lol
            c.gameObject.GetComponent<Rigidbody2D>().AddForce(fuerza*c.transform.position - this.transform.position);
            //c.transform.position = c.transform.position - asteroid.transform.position;
        }
    }
}
