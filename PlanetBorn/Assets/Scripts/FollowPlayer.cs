using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Add the player's gameobject")]
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // Check that the gameObject added is the player
        if (!player.GetComponent<PlayerController>())
        {
            Debug.LogError("The object added is not the player");
        } // if
    } // Start

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 pos = new Vector3(playerPos.x, playerPos.y, -1);

        gameObject.transform.position = pos;
    } // Update
} // FollowPlayer
