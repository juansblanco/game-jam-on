using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [Header("Hook Usage Params")]
    public float maxDistance = 10f;
    public float minDistance = 1.5f;
    public float speed = 10f;
    public bool timer = true;
    //public float releaseForce = 100f; Not in use
    
    [Header("Times")]
    public float maxHookedTime = 10f;

    [Header("Needed Objects")]
    public Transform startPosition;
    public LineRenderer hookLine;

    // Private
    private bool isActive;
    private bool returnToShip;
    private float hookTimer;

    private BoxCollider2D boxCollider;
    private GameObject player;
    private DistanceJoint2D joint;
    private Asteroid attachedAsteroid;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        returnToShip = false;
        boxCollider = GetComponent<BoxCollider2D>();
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        boxCollider.enabled = false;
        player = GameObject.FindWithTag("Player");
        attachedAsteroid = null;
        //hookLine.enabled = false;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLine();
        CheckAsteroid();
        if (isActive)
        {
            if (!returnToShip)
            {
                if(Vector2.Distance(transform.position, startPosition.position) < maxDistance)
                {
                    MoveForward();
                }
                else
                {
                    returnToShip = true;
                }
            }
            else
            {
                if (Vector2.Distance(transform.position, player.transform.position) > minDistance)
                {
                    ReturnToShip();
                }
                else
                {
                    if (attachedAsteroid == null)
                    {
                        DisableHook();
                        player.GetComponent<PlayerController>().ResetHookTimer();
                        Debug.Log("reseting");
                    }

                    //hookLine.enabled = false;
                }
            }
        }
    }

    private void DisableHook()
    {
        transform.position = startPosition.position;
        isActive = false;
        returnToShip = false;
        boxCollider.enabled = false;
        gameObject.SetActive(false);
    }

    private void CheckAsteroid()
    {
        if(attachedAsteroid != null)
        {
            if(attachedAsteroid.aSize != Asteroid.AsteroidSize.VERY_SMALL &&
                attachedAsteroid.aSize != Asteroid.AsteroidSize.SMALL &&
                attachedAsteroid.aSize != Asteroid.AsteroidSize.MEDIUM)
            {
                if (joint.enabled)
                {
                    //if (joint.connectedBody != null) joint.connectedBody.AddForce(transform.up * releaseForce);
                    if (attachedAsteroid != null) attachedAsteroid = null;
                    joint.enabled = false;
                    transform.position = startPosition.position;
                }
            }
            else
            {
                if (timer)
                {
                    hookTimer += Time.deltaTime;
                    Debug.Log("hook timer " + hookTimer);
                    if(hookTimer > maxHookedTime)
                    {
                        Activate();
                        player.GetComponent<PlayerController>().SetHookTimer();
                    }
                }
            }
        }
    }
    private void OnEnable()
    {
        Debug.Log("enabled");
        transform.position = startPosition.position;
        hookLine.gameObject.SetActive(true);
        if(player != null)
        {
            transform.up = player.transform.up;
            UpdateLine();
        }
    }

    private void OnDisable()
    {
        Debug.Log("disabled");
        hookLine.gameObject.SetActive(false);
        joint.enabled = false;
    }

    private void UpdateLine()
    {
        hookLine.SetPosition(0, player.transform.position);
        hookLine.SetPosition(1, transform.position);
    }

    private void ReturnToShip()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.localPosition += direction * speed * Time.deltaTime;
    }

    private void MoveForward()
    {
        Vector3 direction = transform.up.normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    internal void Activate()
    {
        if (joint != null && joint.enabled)
        {
            //if(joint.connectedBody != null) joint.connectedBody.AddForce(transform.up * releaseForce);
            if (attachedAsteroid != null) attachedAsteroid = null;
            DisableHook();
        }
        else
        {
            isActive = true;
            boxCollider.enabled = true;
            hookLine.enabled = true;
        }
        //Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (asteroid != null && attachedAsteroid == null)
        {
            returnToShip = true;
            if(asteroid.aSize == Asteroid.AsteroidSize.VERY_SMALL ||
                asteroid.aSize == Asteroid.AsteroidSize.SMALL ||
                asteroid.aSize == Asteroid.AsteroidSize.MEDIUM)
            {
                joint.enabled = true;
                joint.connectedBody = collision.rigidbody;
                attachedAsteroid = asteroid;
                if (timer)
                {
                    hookTimer = 0;
                }
            }

        }
    }
}
