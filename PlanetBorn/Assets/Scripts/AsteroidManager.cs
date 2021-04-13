using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [Header("Numero max de asteroides")]
    public int maxAsteroides;

    public GameObject asteroide;

    [Header("Tamaño del mapa")]
    public Vector2 mapLimit;

    // Start is called before the first frame update
    void Start()
    {
        GeneracionInicial();
        GetComponent<BoxCollider2D>().size = mapLimit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GeneracionInicial()
    {
        for (int i = 0; i < maxAsteroides; i++)
        {
            GeneraNuevoAsteroide();
        }
    }

    void GeneraNuevoAsteroide()
    {
        Vector2 position = new Vector2(Random.Range(-0.5f*mapLimit.x, 0.5f*mapLimit.x), Random.Range(-0.5f*mapLimit.y, 0.5f*mapLimit.y));
        Instantiate(asteroide,  position, Quaternion.identity);
    }

    void OnTriggerExit2D(Collider2D c)
    {
        Debug.Log(c);
        if (c.gameObject.GetComponent<Asteroid>())
        {
            Destroy(c.gameObject);
            GeneraNuevoAsteroide();
        }
    }
        
}
