using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nave;
    void Start()
    {
        GeneracionInicial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GeneracionInicial()
    {
        Vector2 position = new Vector2(0,0);
        GameObject nObject = Instantiate(nave, position, Quaternion.identity);
        AsteroidManager aManager = GetComponent<AsteroidManager>();
        aManager.SetPlayer(nObject.GetComponentInChildren<PlayerController>().gameObject);
        aManager.GeneracionInicial();
    }

}
