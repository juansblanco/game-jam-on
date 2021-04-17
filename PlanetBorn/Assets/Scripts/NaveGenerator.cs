using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nave;

    public GameObject UI;
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
        PlayerController player = nObject.GetComponentInChildren<PlayerController>();
        AsteroidManager aManager = GetComponent<AsteroidManager>();
        CameraGenerator cGenerator = GetComponent<CameraGenerator>();
        player.UI = UI;
        cGenerator.SetCameraToFollowPlayer(player);
        aManager.SetPlayer(player.gameObject);
        aManager.Start();
    }

}
