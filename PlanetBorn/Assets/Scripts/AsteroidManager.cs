using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidManager : MonoBehaviour
{
    [Header("Numero max de asteroides")] public int maxAsteroides;

    public GameObject asteroide;

    [Header("Tamaño del mapa")] public Vector2 mapLimit;

    [Header("Player")] public GameObject player;
    public float playerSpawnDistance;


    // Start is called before the first frame update
    void Start()
    {
        //GeneracionInicial();
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

    public void GeneracionInicial()
    {
        for (int i = 0; i < maxAsteroides; i++)
        {
            GeneraNuevoAsteroide();
        }
    }

    void GeneraNuevoAsteroide()
    {
        Vector2 position = new Vector2(Random.Range(-0.5f * mapLimit.x, 0.5f * mapLimit.x),
            Random.Range(-0.5f * mapLimit.y, 0.5f * mapLimit.y));
        GameObject randomAsteroid = Instantiate(asteroide, position, Quaternion.identity);
        randomAsteroid.GetComponent<Asteroid>().RandomizeAsteroid();
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
            GeneraNuevoAsteroide();
        }
    }
}