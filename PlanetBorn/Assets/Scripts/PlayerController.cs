using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MovementType
    {
        EVERYDIRECTION,
        FORWARDANDROTATION
    }

    [Header("Movement config")]
    public float forwardForce;

    // Adding some range to keep it a bit smooth
    [Range(1, 2)]
    public float angularForce;

    public float mBoost;
    public MovementType mType;

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
        if(mType == MovementType.EVERYDIRECTION)
        {
            EveryDirectionMovementBehaviour();
        }
        else
        {
            ForwardAndRotationMovementBehaviour();
        }
        Boost();
    } // Update

    private void Boost()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            mBoost = 2;
        }
        else
        {
            mBoost = 1;
        }
    }

    private void EveryDirectionMovementBehaviour()
    {
        Vector2 dir = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
        }

        Move(dir.normalized);
    }
    private void ForwardAndRotationMovementBehaviour()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move(transform.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(-transform.up);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Rotate(-1);
        }
    }

    void Move(Vector2 dir)
    {
        body.AddForce(dir * forwardForce * mBoost);
    } // Move

    void Rotate(float torque)
    {
        body.AddTorque(torque * angularForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision " + collision.relativeVelocity);
        float collisionValue = Math.Abs(collision.relativeVelocity.x) + Math.Abs(collision.relativeVelocity.y);
        Debug.Log("collision value " + collisionValue);
        // Podemos usar esto para calcular los impactos?
    }
}
