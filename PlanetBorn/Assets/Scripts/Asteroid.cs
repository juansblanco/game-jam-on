using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = UnityEngine.Random;


public class Asteroid : MonoBehaviour
{
    public enum AsteroidColor
    {
        GREEN,
        RED,
        YELLOW,
        GREY
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

    [Header("Movement config")] public float mForce;
    public float mTorque;

    public AsteroidColor aColor;

    public AsteroidSize aSize;

    // Private
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        InitialForce();
    }

    public void RandomizeAsteroidColor()
    {
        aColor = (AsteroidColor) UnityEngine.Random.Range(0, 3);
        SetColorBasedOnType();
    }

    public void RandomizeAsteroidSize()
    {
        aSize = (AsteroidSize) Random.Range(0, 4);
        SetAsteroidSize();
    }

    public void SetColorBasedOnType()
    {
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
            case AsteroidColor.GREY:
                GetComponent<SpriteRenderer>().color = Color.grey;
                break;
        }
    }

    void SetAsteroidSize()
    {
        Vector3 scale;
        switch (aSize)
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
                scale = new Vector3(6, 6, transform.localScale.z);
                break;
            default:
                Debug.Log("Asteroid creation failed");
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

    public void AddForce(Vector2 dir, float force)
    {
        body.AddForce(dir * force);
    } // AddForce

    private void InitialForce()
    {
        Vector2 v = new Vector2(UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f));
        //Debug.Log("v " + v);
        body.AddForce(
            v * mForce);
        body.AddTorque(UnityEngine.Random.Range(-1f, 1f) * mTorque);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (asteroid &&
            asteroid.aColor == aColor)
            AsteroidFusion(asteroid);
    }

    //Fusiona los asteroides del mismo color que chocan
    void AsteroidFusion(Asteroid asteroid)
    {
        Debug.Log("puff y se unen: " + asteroid.aSize + " con " + this.aSize);
        if (asteroid.aSize > this.aSize)
        {
            if (asteroid.aSize < AsteroidSize.ULTRA_MEGA_BIG)
            {
                asteroid.aSize++;
                asteroid.SetAsteroidSize();
                Destroy(this);
            }
            else
            {
                asteroid.RandomizeAsteroidSize();
            }
        }
        else
        {
            if (this.aSize < AsteroidSize.ULTRA_MEGA_BIG)
            {
                this.aSize++;
                SetAsteroidSize();
                Destroy(asteroid.gameObject);
            }
            else
            {
                RandomizeAsteroidSize();
            }
        }
    }
}