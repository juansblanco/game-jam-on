using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesBackgroundController : MonoBehaviour
{
    void Start()
    {
        GetComponent<ParticleSystem>().Pause();
    }
}
