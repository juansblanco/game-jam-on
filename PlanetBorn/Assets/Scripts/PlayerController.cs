using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement config")]
    public float mForce;


    // Private
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("Forward");
            dir.y = 1;
        } 
        
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Left");
            dir.x = -1;
        } 
        
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("Down");
            dir.y = -1;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Right");
            dir.x = 1;
        }

        Move(dir.normalized);
    } // Update

    void Move(Vector2 dir)
    {
        dir *= mForce;

        body.AddForce(dir);
    } // Move
}
