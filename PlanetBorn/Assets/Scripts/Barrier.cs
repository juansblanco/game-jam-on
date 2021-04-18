using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Barrier : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform shipPosition;

    public float duration = 5;
    private float durationTimer;
    void Start()
    {
        durationTimer = duration;
    }

    // Update is called once per frame
    void Update()
    {
        durationTimer = Mathf.Max(0, durationTimer - Time.deltaTime);
        if (durationTimer == 0)
        {
            durationTimer = duration;
            this.gameObject.SetActive(false);
        }
        this.transform.position = shipPosition.position;
        this.transform.rotation = shipPosition.rotation;
    }

}
