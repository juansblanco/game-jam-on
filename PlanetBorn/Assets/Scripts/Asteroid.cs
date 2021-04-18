using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = UnityEngine.Random;
using Cinemachine;


public class Asteroid : MonoBehaviour
{
    public enum AsteroidColor
    {
        GREEN,
        RED,
        BLUE,
        BROWN
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

    [Header("Configure sprites")] 
    public Sprite[] sprites;

    [Header("Movement config")] 
    public float mForce;
    public float mTorque;
    public bool initialForce = true;
    public float maxAngularVelocity;
    public float maxRotation;

    [Header("Visual config")] 
    public AsteroidColor aColor;
    public AsteroidSize aSize;

    [Header("Gravity")] 
    public GameObject gravity;

    [Header("Prefabs")]
    public Planet planetPrefab;

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
    }
    
    void FixedUpdate()
    {
        CheckSpeed();
    }

    private void CheckSpeed()
    {
        body.angularVelocity = Mathf.Clamp(body.angularVelocity, -maxAngularVelocity, maxAngularVelocity);
        //body.velocity = Mathf.Max(body.velocity, maxVelocity); need to think about this
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
        Debug.Log("Tamaño nuevo " + aSize);
    }

    public void SetColorBasedOnType()
    {
        switch (aColor)
        {
            case AsteroidColor.GREEN:
                //GetComponent<SpriteRenderer>().color = Color.green;
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor];
                break;
            case AsteroidColor.RED:
                //GetComponent<SpriteRenderer>().color = Color.red;
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor];
                break;
            case AsteroidColor.BLUE:
                //GetComponent<SpriteRenderer>().color = Color.yellow;
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor];
                break;
            case AsteroidColor.BROWN:
                //GetComponent<SpriteRenderer>().color = Color.grey;
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor];
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
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor];
                body.mass = 1f;
                break;
            case AsteroidSize.SMALL:
                scale = new Vector3(1.5f, 1.5f, transform.localScale.z);
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor];
                body.mass = 1.3f;
                break;
            case AsteroidSize.MEDIUM:
                scale = new Vector3(2, 2, transform.localScale.z);
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor];
                body.mass = 1.6f;
                break;
            case AsteroidSize.BIG:
                scale = new Vector3(3, 3, transform.localScale.z);
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor + 4];
                body.mass = 2f;
                break;
            case AsteroidSize.VERY_BIG:
                scale = new Vector3(4, 4, transform.localScale.z);
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor + 4];
                body.mass = 2.3f;
                break;
            case AsteroidSize.ULTRA_BIG:
                scale = new Vector3(5, 5, transform.localScale.z);
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor + 4];
                body.mass = 2.6f;
                break;
            case AsteroidSize.ULTRA_MEGA_BIG:
                scale = new Vector3(8, 8, transform.localScale.z);
                GetComponent<SpriteRenderer>().sprite = sprites[(int)aColor + 4];
                body.mass = 3f;
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
        if(aColor != AsteroidColor.BROWN)
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
        //No funciona
        else if (asteroid && asteroid.aColor == AsteroidColor.RED && (aColor == AsteroidColor.GREEN || aColor==AsteroidColor.BLUE))
        {
            Debug.Log("Explode1");
            if (aSize > AsteroidSize.VERY_SMALL)
            {
                this.aSize--;
                SetAsteroidSize();
                Debug.Log("Tamaño nuevo " + aSize);
            }
            if (asteroid.aSize > AsteroidSize.BIG)
            {
                asteroid.RandomizeAsteroidSize();
                RigidbodyConstraints2D constraints2D = RigidbodyConstraints2D.None;
                asteroid.GetComponent<Rigidbody2D>().constraints = constraints2D;
            }
            else
            {
                Destroy(asteroid.gameObject);  
            }
        }
        else if(asteroid && asteroid.aSize == AsteroidSize.ULTRA_MEGA_BIG
            && aSize == AsteroidSize.ULTRA_MEGA_BIG && asteroid.aColor != aColor
            && (aColor == AsteroidColor.BLUE || aColor == AsteroidColor.GREEN)
            && (asteroid.aColor == AsteroidColor.BLUE || asteroid.aColor == AsteroidColor.GREEN)){
            if(aColor == AsteroidColor.BLUE)
            {
                Debug.Log("destroying blue");
                Destroy(gameObject);
            }
            else
            {
                UpgradeToPlanet();
            }
        }
        else if (collision.gameObject.GetComponent<PlayerController>() &&
                 collision.otherCollider.GetComponent<Asteroid>())
        {
            Debug.Log("impulse");
            GetComponent<CinemachineImpulseSource>().GenerateImpulse(3f * body.mass);
        }
    }
    void UpgradeToPlanet()
    {
        Debug.Log("upgrading to planet");
        Planet planet = Instantiate(planetPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
                Debug.Log("Tamaño nuevo " + asteroid.aSize);
                Destroy(gameObject);
            }
            else if (asteroid.aColor == AsteroidColor.BROWN)
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
                Debug.Log("Tamaño nuevo " + aSize);
                Destroy(asteroid.gameObject);
            }
            else if (asteroid.aColor == AsteroidColor.BROWN)
            {
                RandomizeAsteroidSize();
            }
        }
    }

    
}