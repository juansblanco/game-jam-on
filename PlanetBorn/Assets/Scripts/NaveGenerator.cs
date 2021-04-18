using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveGenerator : MonoBehaviour
{
    public GameObject nave;
    public GameObject UI;
    public GameObject levelLoader;

    void Start()
    {
        GeneracionInicial();
    }

    void GeneracionInicial()
    {
        Vector2 position = new Vector2(0,0);
        GameObject nObject = Instantiate(nave, position, Quaternion.identity);
        PlayerController player = nObject.GetComponentInChildren<PlayerController>();
        AsteroidManager aManager = GetComponent<AsteroidManager>();
        CameraGenerator cGenerator = GetComponent<CameraGenerator>();
        GameObject UIObject = Instantiate(UI);
        GameObject LLObject = Instantiate(levelLoader);
        player.UI = UIObject;
        player.UILoad();
        UIObject.GetComponentInChildren<UIController>().levelLoader = 
            LLObject.GetComponent<LevelLoader>();
        cGenerator.SetCameraToFollowPlayer(player);
        aManager.SetPlayer(player.gameObject);
        aManager.Start();
    }

}
