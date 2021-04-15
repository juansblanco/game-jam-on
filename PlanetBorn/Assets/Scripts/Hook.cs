using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public float maxDistance = 10f;
    public float minDistance = 1.5f;
    public float speed = 10f;
    public float releaseForce = 100f;

    public LineRenderer hookLine;

    private bool isActive;
    private bool returnToShip;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Vector2 startPosition;
    private GameObject player;
    private DistanceJoint2D joint;
    private Asteroid attachedAsteroid;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        returnToShip = false;
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        startPosition = transform.localPosition;
        boxCollider.enabled = false;
        player = GameObject.FindWithTag("Player");
        attachedAsteroid = null;
        //hookLine.enabled = false;
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
                if(Vector2.Distance(transform.localPosition, startPosition) < maxDistance)
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
                if (Vector2.Distance(transform.localPosition, startPosition) > minDistance)
                {
                    ReturnToShip();
                }
                else
                {
                    if(attachedAsteroid == null) transform.localPosition = startPosition;
                    isActive = false;
                    returnToShip = false;
                    boxCollider.enabled = false;
                    //hookLine.enabled = false;
                }
            }
        }
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
                    if (joint.connectedBody != null) joint.connectedBody.AddForce(transform.up * releaseForce);
                    if (attachedAsteroid != null) attachedAsteroid = null;
                    joint.enabled = false;
                    transform.localPosition = startPosition;
                }
            }
        }
    }

    private void UpdateLine()
    {
        hookLine.SetPosition(0, player.transform.position);
        hookLine.SetPosition(1, transform.position);
    }

    private void ReturnToShip()
    {
        Vector3 direction = ((Vector3)startPosition - transform.localPosition).normalized;
        transform.localPosition += direction * speed * Time.deltaTime;
        //transform.Translate(player.transform.position);
        //body.MovePosition(player.transform.position);
        //Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void MoveForward()
    {
        Vector3 direction = transform.up.normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    internal void Activate()
    {
        if (joint.enabled)
        {
            if(joint.connectedBody != null) joint.connectedBody.AddForce(transform.up * releaseForce);
            if (attachedAsteroid != null) attachedAsteroid = null;
            joint.enabled = false;
            transform.localPosition = startPosition;
        }
        else
        {
            isActive = true;
            boxCollider.enabled = true;
            //hookLine.enabled = true;
        }
        //Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            returnToShip = true;
            if(asteroid.aSize == Asteroid.AsteroidSize.VERY_SMALL ||
                asteroid.aSize == Asteroid.AsteroidSize.SMALL ||
                asteroid.aSize == Asteroid.AsteroidSize.MEDIUM)
            {
                joint.enabled = true;
                joint.connectedBody = collision.rigidbody;
                attachedAsteroid = asteroid;
            }

        }
    }
}
