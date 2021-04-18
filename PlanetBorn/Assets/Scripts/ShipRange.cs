using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRange : MonoBehaviour
{
    [Header("Forces")] public float pullForce;
    public float pushForce;

    private List<GameObject> inRange;

    private void Start()
    {
        inRange = new List<GameObject>(); // Init list
    }

    public void Push()
    {
        Vector2 pos = gameObject.transform.position;

        foreach (GameObject o in inRange)
        {
            Vector2 asPos = o.transform.position;

            Vector2 dir = asPos - pos;
            dir.Normalize();
            o.GetComponent<Asteroid>().AddForce(dir, pushForce);
        } // foreach
    } // Push

    public void Pull()
    {
        Vector2 pos = gameObject.transform.position;

        foreach (GameObject o in inRange)
        {
            Vector2 asPos = o.transform.position;
            Vector2 dir = asPos - pos;
            dir.Normalize();
            o.GetComponent<Asteroid>().AddForce(dir * -1, pullForce);
        } // foreach
    } // Pull

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Asteroid>())
        {
            //Debug.Log("Ahhhhh entró un asteroide nomás");
            inRange.Add(collision.gameObject);
        } // if
    } // OnTriggerEnter2D

    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            //Debug.Log("Ahhhhh saliose un asteroide nomás");
            // Technically this should not give an error
            inRange.Remove(collision.gameObject);
        } // try
        catch (UnityException e)
        {
            Debug.Log(e);
        } // catch
    } // OnTriggerExit2D
} // ShipRange