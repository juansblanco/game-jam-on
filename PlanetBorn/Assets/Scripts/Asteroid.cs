using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidType
    {
        GREEN,
        RED,
        YELLOW
    }

    [Header("Movement config")]
    public float mForce;
    public float mTorque;

    public AsteroidType aType;

    // Private
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        InitialForce();
        SetColorBasedOnType();
    }

    private void SetColorBasedOnType()
    {
        aType = (AsteroidType)UnityEngine.Random.Range(0, 3);
        switch (aType)
        {
            case AsteroidType.GREEN:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case AsteroidType.RED:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case AsteroidType.YELLOW:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // body.AddForce(new Vector2(1, -1) * mForce);
        
    }
    private void InitialForce()
    {
        Vector2 v = new Vector2(UnityEngine.Random.Range(-1f, 1f),
                    UnityEngine.Random.Range(-1f, 1f));
        Debug.Log("v " + v);
        body.AddForce(
            v * mForce);
        body.AddTorque(UnityEngine.Random.Range(-1f, 1f) * mTorque);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid" && 
            collision.gameObject.GetComponent<Asteroid>().aType == aType)
        {
            Debug.Log("puff y se unen");
        }
    }
}
