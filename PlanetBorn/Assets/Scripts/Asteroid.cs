using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidColor
    {
        GREEN,
        RED,
        YELLOW
    }

    public enum AsteroidSize
    {
        VERY_SMALL,
        SMALL,
        MEDIUM,
        BIG,
        VERY_BIG,
        ULTRA_BIG,
        ULTRA_MEGA_BIG
    }

    [Header("Movement config")]
    public float mForce;
    public float mTorque;

    public AsteroidColor aColor;

    // Private
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        InitialForce();
        SetColorBasedOnType();
        SetAsteroidSize();
    }

    private void SetColorBasedOnType()
    {
        aColor = (AsteroidColor)UnityEngine.Random.Range(0, 3);
        switch (aColor)
        {
            case AsteroidColor.GREEN:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case AsteroidColor.RED:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case AsteroidColor.YELLOW:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
        }
    }

    void SetAsteroidSize()
    {
        Vector3 scale;
        switch ((AsteroidSize)Random.Range(0, 7))
        {
            case AsteroidSize.VERY_SMALL:
                scale = new Vector3(0.5f, 0.5f, transform.localScale.z);
                break;
            case AsteroidSize.SMALL:
                scale = new Vector3(0.75f, 0.75f, transform.localScale.z);
                break;
            case AsteroidSize.MEDIUM:
                scale = new Vector3(1, 1, transform.localScale.z);
                break;
            case AsteroidSize.BIG:
                scale = new Vector3(1.5f, 1.5f, transform.localScale.z);
                break;
            case AsteroidSize.VERY_BIG:
                scale = new Vector3(2, 2, transform.localScale.z);
                break;
            case AsteroidSize.ULTRA_BIG:
                scale = new Vector3(4, 4, transform.localScale.z);
                break;
            case AsteroidSize.ULTRA_MEGA_BIG:
                scale = new Vector3(10, 10, transform.localScale.z);
                break;
            default:
                scale = new Vector3(0, 0, 0);
                break;
        }

        transform.localScale = scale;
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
            collision.gameObject.GetComponent<Asteroid>().aColor == aColor)
        {
            Debug.Log("puff y se unen");
        }
    }
}
