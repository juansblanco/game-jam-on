using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Planet : MonoBehaviour
{
    public enum PlanetSize
    {
        SMALL,
        MEDIUM,
        LARGE
    }

    [Header("Configure sprites")]
    public Sprite[] sprites;

    [Header("Visual config")]
    public PlanetSize pSize;

    [Header("Gravity")]
    public GameObject gravity;

    [Header("Movement")]
    public float angularVelocity;

    [Header("Stats")]
    public float healthBySizeFactor;

    // Private
    private Rigidbody2D body;
    private CircleCollider2D gravityCollider;
    private float planetHealth;
    private int asteroidsToNextLevel;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.angularVelocity = angularVelocity;
        gravityCollider = gravity.GetComponent<CircleCollider2D>();
        planetHealth = ((int)pSize + 1) * healthBySizeFactor;
        asteroidsToNextLevel = 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (asteroid)
        {
            CheckAsteroidToMakeBiggerPlanet(asteroid);
            CheckAsteroidToTakeDamage(asteroid, collision);
            Destroy(asteroid.gameObject);
        }
    }

    private void CheckAsteroidToTakeDamage(Asteroid asteroid, Collision2D collision)
    {
        if(asteroid.aColor == Asteroid.AsteroidColor.BROWN 
            || asteroid.aColor == Asteroid.AsteroidColor.RED
            || asteroid.aSize != Asteroid.AsteroidSize.ULTRA_MEGA_BIG)
        {
            float collisionValue = Math.Abs(collision.relativeVelocity.x) + Math.Abs(collision.relativeVelocity.y);
            collisionValue *= collision.rigidbody.mass;
            if (collision.gameObject.GetComponent<Asteroid>().aColor == Asteroid.AsteroidColor.RED)
            {
                collisionValue *= 2;
            }
            TakeDamage(collisionValue);
        }
    }

    private void TakeDamage(float damage)
    {
        planetHealth = Mathf.Max(0, planetHealth - damage);
        if(planetHealth == 0)
        {
            // Planet is destroyed, it just destroys itself for now, 
            // will be divided in several asteroids in the future
            Destroy(gameObject);
        }
    }

    private void CheckAsteroidToMakeBiggerPlanet(Asteroid asteroid)
    {
        if (asteroid.aSize == Asteroid.AsteroidSize.ULTRA_MEGA_BIG
                        && asteroid.aColor != Asteroid.AsteroidColor.BROWN
                        && asteroid.aColor != Asteroid.AsteroidColor.RED)
        {
            asteroidsToNextLevel--;
            if(asteroidsToNextLevel <= 0)
            {
                SetPlanetSize(pSize + 1);
            }
        }
    }

    private void SetPlanetSize(PlanetSize planetSize)
    {
        planetSize = (PlanetSize) Mathf.Min((int)planetSize, 2);
        switch (planetSize)
        {
            case PlanetSize.SMALL:
                pSize = PlanetSize.SMALL;
                transform.localScale = new Vector3(10, 10, transform.localScale.z);
                asteroidsToNextLevel = 2;
                break;
            case PlanetSize.MEDIUM:
                pSize = PlanetSize.MEDIUM;
                transform.localScale = new Vector3(12, 12, transform.localScale.z);
                asteroidsToNextLevel = 3;
                break;
            case PlanetSize.LARGE:
                pSize = PlanetSize.LARGE;
                transform.localScale = new Vector3(14, 14, transform.localScale.z);
                WinGame();
                break;
            default:
                Debug.Log("Incorrect planet size: " + planetSize);
                break;
        }
        SetPlanetSprite(pSize);

    }

    private void WinGame()
    {
        CinemachineVirtualCamera playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
        playerCamera.Follow = transform;
        StartCoroutine(GameObject.FindWithTag("Player").GetComponent<PlayerController>().WinGame());
    }

    private void SetPlanetSprite(PlanetSize pSize)
    {
        switch (pSize)
        {
            case PlanetSize.SMALL:
            case PlanetSize.MEDIUM:
                GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
            case PlanetSize.LARGE:
                GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;
        }
    }
}
