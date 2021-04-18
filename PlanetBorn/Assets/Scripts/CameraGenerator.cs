using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraGenerator : MonoBehaviour
{
    public GameObject camerasPrefab;

    private GameObject cameras;
    
    private CinemachineVirtualCamera virtualCamera;

    private CinemachineVirtualCamera minimapCamera;

    private bool camera = true;
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            activateMinimap();
        }
    }
    public void SetCameraToFollowPlayer(PlayerController player)
    {
        cameras = Instantiate(camerasPrefab);
        virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
        minimapCamera = GameObject.FindWithTag("Minimap").GetComponent<CinemachineVirtualCamera>();
        minimapCamera.gameObject.SetActive(false);
        virtualCamera.Follow = player.transform;
    }

    public void activateMinimap()
    {
        virtualCamera.gameObject.SetActive(camera);
        minimapCamera.gameObject.SetActive(!camera);
        camera = !camera;
    }
}
