using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGravity : MonoBehaviour
{
    public Rigidbody2D body;

    private List<Asteroid> asteroids;
    private CircleCollider2D gCollider;
    private void FixedUpdate()
    {
        if (asteroids.Count > 0 && gCollider.enabled)
        {
            foreach (Asteroid asteroid in asteroids)
            {
                if (asteroid != null)
                {
                    GravitationalPull(asteroid);
                }
            }
        }
    }

    private void Start()
    {
        asteroids = new List<Asteroid>();
        gCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Asteroid asteroid = collision.GetComponent<Asteroid>();
        if (asteroid && asteroid.aSize != Asteroid.AsteroidSize.ULTRA_MEGA_BIG)
        {
            asteroids.Add(asteroid);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Asteroid asteroid = collision.GetComponent<Asteroid>();
        if (asteroid && asteroid.aSize != Asteroid.AsteroidSize.ULTRA_MEGA_BIG &&
            asteroids.Contains(asteroid))
        {
            asteroids.Remove(asteroid);
        }
    }

    private void GravitationalPull(Asteroid asteroid)
    {
        Rigidbody2D aBody = asteroid.GetComponent<Rigidbody2D>();

        Vector2 direction = transform.position - asteroid.transform.position;
        float distance = direction.magnitude;

        float forceMagnitude = (body.mass * aBody.mass) / Mathf.Pow(distance, 2);
        Vector2 force = direction.normalized * forceMagnitude;

        aBody.AddForce(force);
    }
}
