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

    [Header("Configure sprites")] public Sprite[] sprites;

    [Header("Movement config")] 
    public float mForce;
    public float mTorque;
    public bool initialForce = true;

    [Header("Visual config")] public AsteroidColor aColor;
    public AsteroidSize aSize;

    [Header("Gravity")] 
    public GameObject gravity;

    private bool isPlanet;

    // Private
    private Rigidbody2D body;
    private CircleCollider2D gravityCollider;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        gravityCollider = gravity.GetComponent<CircleCollider2D>();
        if (initialForce)
        {
            InitialForce();
        }
        isPlanet = false;
    }
    
    void Update()
    {
        if (this.aSize == AsteroidSize.ULTRA_MEGA_BIG && this.aColor != AsteroidColor.GREY && !isPlanet)
        {
            Debug.Log("Planet complete");
            this.isPlanet = true;
            RigidbodyConstraints2D constraints2D = RigidbodyConstraints2D.FreezeAll;
            this.GetComponent<Rigidbody2D>().constraints = constraints2D;
        }
        /*if (aSize == AsteroidSize.ULTRA_MEGA_BIG && aColor != AsteroidColor.GREY)
        {
            Collider2D c = gravity.GetComponent<Collider2D>();
            Debug.Log("Collider enabled: " + c.enabled);
            if (!c.enabled)
            {
                c.enabled = !c.enabled;
                Debug.Log("Gravity added" + c.gameObject);
            }
        }*/

        // body.AddForce(new Vector2(1, -1) * mForce);
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
                //GetComponent<SpriteRenderer>().color = Color.green;
                GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;
            case AsteroidColor.RED:
                //GetComponent<SpriteRenderer>().color = Color.red;
                GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;
            case AsteroidColor.YELLOW:
                //GetComponent<SpriteRenderer>().color = Color.yellow;
                GetComponent<SpriteRenderer>().sprite = sprites[3];
                break;
            case AsteroidColor.GREY:
                //GetComponent<SpriteRenderer>().color = Color.grey;
                GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
        }
    }

    public void SetAsteroidSize()
    {
        Vector3 scale;
        if(body == null) body = GetComponent<Rigidbody2D>();
        switch (aSize)
        {
            case AsteroidSize.VERY_SMALL:
                scale = new Vector3(1, 1, transform.localScale.z);
                body.mass = 0.2f;
                break;
            case AsteroidSize.SMALL:
                scale = new Vector3(1.5f, 1.5f, transform.localScale.z);
                body.mass = 0.6f;
                break;
            case AsteroidSize.MEDIUM:
                scale = new Vector3(2, 2, transform.localScale.z);
                body.mass = 1f;
                break;
            case AsteroidSize.BIG:
                scale = new Vector3(3, 3, transform.localScale.z);
                body.mass = 1.4f;
                break;
            case AsteroidSize.VERY_BIG:
                scale = new Vector3(4, 4, transform.localScale.z);
                body.mass = 1.7f;
                break;
            case AsteroidSize.ULTRA_BIG:
                scale = new Vector3(5, 5, transform.localScale.z);
                body.mass = 2f;
                break;
            case AsteroidSize.ULTRA_MEGA_BIG:
                scale = new Vector3(8, 8, transform.localScale.z);
                body.mass = 2.5f;
                break;
            default:
                Debug.Log("Asteroid creation failed");
                scale = Vector3.zero;
                break;
        }

        transform.localScale = scale;
        ManageGravity();
    }

    private void ManageGravity()
    {
        if(aColor != AsteroidColor.GREY)
        {
            if (gravityCollider == null)
            {
                gravityCollider = gravity.GetComponent<CircleCollider2D>();
            }

            if (aSize == AsteroidSize.ULTRA_MEGA_BIG)
            {
                Debug.Log("Collider enabled: " + gravityCollider.enabled);
                gravityCollider.enabled = true;
            }
            else
            {
                gravityCollider.enabled = false;
            }
        }
    }

    // Update is called once per frame
   

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
        //Debug.Log("puff y se unen: " + asteroid.aSize + " con " + this.aSize);
        if (asteroid.aSize > aSize)
        {
            if (asteroid.aSize < AsteroidSize.ULTRA_MEGA_BIG)
            {
                asteroid.aSize++;
                asteroid.SetAsteroidSize();
                Destroy(gameObject);
            }
            else if (asteroid.aColor == AsteroidColor.GREY)
            {
                asteroid.RandomizeAsteroidSize();
            }
        }
        else
        {
            if (aSize < AsteroidSize.ULTRA_MEGA_BIG)
            {
                aSize++;
                SetAsteroidSize();
                Destroy(asteroid.gameObject);
            }
            else if (asteroid.aColor == AsteroidColor.GREY)
            {
                RandomizeAsteroidSize();
            }
        }
    }
}