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


    // Start is called before the first frame update
    public void Start()
    {
        GeneracionInicial(numNormales, Asteroid.AsteroidColor.GREY);
        GeneracionInicial(numGreen, Asteroid.AsteroidColor.GREEN);
        GeneracionInicial(numRed, Asteroid.AsteroidColor.RED);
        GeneracionInicial(numYellow, Asteroid.AsteroidColor.YELLOW);
        GetComponent<BoxCollider2D>().size = mapLimit;
    }

    internal void SetPlayer(GameObject nObject)
    {
        player = nObject;
    }

    // Update is called once per frame
    void Update()
    {
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
        Vector2 position = new Vector2(Random.Range(-0.5f * mapLimit.x, 0.5f * mapLimit.x),
            Random.Range(-0.5f * mapLimit.y, 0.5f * mapLimit.y));
        GameObject randomAsteroid = Instantiate(asteroide, position, Quaternion.identity);
        randomAsteroid.GetComponent<Asteroid>().aColor = color;
        randomAsteroid.GetComponent<Asteroid>().SetColorBasedOnType();
        randomAsteroid.GetComponent<Asteroid>().RandomizeAsteroidSize();
    }

    void GeneraNuevoAsteroideRandom()
    {
        Vector2 position = new Vector2(Random.Range(-0.5f * mapLimit.x, 0.5f * mapLimit.x),
            Random.Range(-0.5f * mapLimit.y, 0.5f * mapLimit.y));
        GameObject randomAsteroid = Instantiate(asteroide, position, Quaternion.identity);
        randomAsteroid.GetComponent<Asteroid>().RandomizeAsteroidColor();
        randomAsteroid.GetComponent<Asteroid>().RandomizeAsteroidSize();
    }

    private Vector2 PosicionRandomFueraJugador()
    {
        Vector2 position = new Vector2(Random.Range(-0.5f * mapLimit.x, 0.5f * mapLimit.x),
            Random.Range(-0.5f * mapLimit.y, 0.5f * mapLimit.y));
        while (Vector2.Distance(position, player.transform.position) <= playerSpawnDistance)
        {
            position = new Vector2(Random.Range(-0.5f * mapLimit.x, 0.5f * mapLimit.x),
                Random.Range(-0.5f * mapLimit.y, 0.5f * mapLimit.y));
        }
        return position;
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<Asteroid>())
        {
            Debug.Log("Nuevo asteroide");
            Destroy(c.gameObject);
            GeneraNuevoAsteroideColor(Asteroid.AsteroidColor.GREY);
        }
    }
}