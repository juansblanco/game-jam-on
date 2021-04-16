using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraGenerator : MonoBehaviour
{
    public GameObject camerasPrefab;

    private GameObject cameras;

    public void SetCameraToFollowPlayer(PlayerController player)
    {
        cameras = Instantiate(camerasPrefab);
        CinemachineVirtualCamera virtualCamera = cameras.GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCamera.Follow = player.transform;
    }
}
