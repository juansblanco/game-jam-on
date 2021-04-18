using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Barrier : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform shipPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = shipPosition.position;
        this.transform.rotation = shipPosition.rotation;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
    
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
