using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidManager : MonoBehaviour
{
    [Header("Asteroides colores")] public int numGreen;
    public int numRed;
    public int numYellow;
    [Header("Asteroides normales")] public int numNormales;

    public GameObject asteroide;

    [Header("Tamaño del mapa")] public Vector2 mapLimit;

    [Header("Player")] public GameObject player;
    public float playerSpawnDistance;

    public float spawnTime = 30;
    private float timer;
    private bool alternate;


    // Start is called before the first frame update
    public void Start()
    {
        GeneracionInicial(numNormales, Asteroid.AsteroidColor.BROWN);
        GeneracionInicial(numGreen, Asteroid.AsteroidColor.GREEN);
        GeneracionInicial(numRed, Asteroid.AsteroidColor.RED);
        GeneracionInicial(numYellow, Asteroid.AsteroidColor.BLUE);
        GetComponent<BoxCollider2D>().size = mapLimit;
        timer = spawnTime;
    }

    void Update()
    {
        timer = Mathf.Max(0, timer - Time.deltaTime);
        Spawner();
    }
    internal void SetPlayer(GameObject nObject)
    {
        player = nObject;
    }    

    public void GeneracionInicial(int num, Asteroid.AsteroidColor color)
    {
        for (int i = 0; i < num; i++)
        {
            GeneraNuevoAsteroideColor(color);
        }
        
    }

    void GeneraNuevoAsteroideColor(Asteroid.AsteroidColor color)
    {
        Vector2 position = PosicionRandomFueraJugador();
        GameObject randomAsteroid = Instantiate(asteroide, position, Quaternion.identity);
        randomAsteroid.GetComponent<Asteroid>().aColor = color;
        randomAsteroid.GetComponent<Asteroid>().SetColorBasedOnType();
        randomAsteroid.GetComponent<Asteroid>().RandomizeAsteroidSize();
    }

    void GeneraNuevoAsteroideColorSize(Asteroid.AsteroidColor color, Asteroid.AsteroidSize size)
    {
        Vector2 position = PosicionRandomFueraJugador();
        GameObject randomAsteroid = Instantiate(asteroide, position, Quaternion.identity);
        randomAsteroid.GetComponent<Asteroid>().aColor = color;
        randomAsteroid.GetComponent<Asteroid>().SetColorBasedOnType();
        randomAsteroid.GetComponent<Asteroid>().aSize = size;
        randomAsteroid.GetComponent<Asteroid>().SetAsteroidSize();
    }

    void GeneraNuevoAsteroideColorPosicion(Asteroid.AsteroidColor color, Vector2 position)
    {
        GameObject randomAsteroid = Instantiate(asteroide, position, Quaternion.identity);
        randomAsteroid.GetComponent<Asteroid>().aColor = color;
        randomAsteroid.GetComponent<Asteroid>().SetColorBasedOnType();
        randomAsteroid.GetComponent<Asteroid>().RandomizeAsteroidSize();
    }

    private Vector2 PosicionRandomFueraJugador()
    {
        Vector2 position;
        do
        {
            position = new Vector2(Random.Range(-0.5f * mapLimit.x, 0.5f * mapLimit.x),
                Random.Range(-0.5f * mapLimit.y, 0.5f * mapLimit.y));
        } while (Vector2.Distance(position, player.transform.position) <= playerSpawnDistance);
        return position;
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<Asteroid>())
        {
            //Debug.Log("Nuevo asteroide");
            Asteroid.AsteroidColor color = c.gameObject.GetComponent<Asteroid>().aColor;
            Asteroid.AsteroidSize size = c.gameObject.GetComponent<Asteroid>().aSize;
            Destroy(c.gameObject);
            if (c.gameObject.GetComponent<Asteroid>().aColor != Asteroid.AsteroidColor.BROWN)
            {
                GeneraNuevoAsteroideColorSize(color, size);
                //Debug.Log("Tamaño nuevo " + size);
            }
            else
            {
                GeneraNuevoAsteroideColor(color);
            }
        }
        if (c.gameObject.GetComponent<PlayerController>())
        {
            Vector2 v = Vector2.zero - (Vector2)c.gameObject.transform.position;
            Debug.Log(v);
            c.gameObject.GetComponent<Rigidbody2D>().AddForce(v*0.5f, ForceMode2D.Impulse);
            c.gameObject.GetComponent<PlayerController>().TakeDamage(4);
            //c.gameObject.GetComponent<Transform>().position = Vector3.zero;
        }
    }

    void Spawner()
    {
        
        if (timer == 0)
        {
            Debug.Log("New red");
            GeneraNuevoAsteroideColor(Asteroid.AsteroidColor.RED);
            timer = spawnTime;
            if (alternate)
            {
                alternate = false;
                GeneraNuevoAsteroideColor(Asteroid.AsteroidColor.BLUE);
            }
            else
            {
                alternate = true;
                GeneraNuevoAsteroideColor(Asteroid.AsteroidColor.GREEN);
            }
        }
        
    }
    
    //Buen intento pero no sirve :(
    public void DestroyAsteroid(Asteroid asteroid)
    {
        Asteroid.AsteroidColor color = asteroid.aColor;
        Vector2 position = asteroid.transform.position;
        Asteroid.AsteroidSize size = asteroid.aSize;
        Destroy(asteroid.gameObject);
        position = new Vector2(position.x + 1, position.y + 1);
        GeneraNuevoAsteroideColorPosicion(color, position);
        if (size > Asteroid.AsteroidSize.BIG)
        {
            Debug.Log("Its big");
            position = new Vector2(position.x - 1, position.y - 1);
            GeneraNuevoAsteroideColorPosicion(color, position);
        }
    }
}