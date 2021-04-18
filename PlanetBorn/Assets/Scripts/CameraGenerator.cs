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

    private bool playerCamera = true;
    private PlayerController player;
    
    
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
        this.player = player;
    }

    public void activateMinimap()
    {
        playerCamera = !playerCamera;
        virtualCamera.gameObject.SetActive(playerCamera);
        minimapCamera.gameObject.SetActive(!playerCamera);
        player.CanMove = playerCamera;
    }
}
