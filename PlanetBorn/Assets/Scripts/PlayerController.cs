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
    [Range(1, 10)]
    public float angularForce;

    public float mBoost;
    public MovementType mType;

    [Header("Mechanics")]
    public ShipRange forceField;
    public Hook hook;

    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float shield;
    public float maxShield;
    public float shieldRegenCooldown;
    public float shieldRegenAmount;

    [Header("Propulsion particles")]
    public ParticleSystem particleLeft;
    public ParticleSystem particleRight;

    // Private
    private Rigidbody2D body;
    private Vector2 mDir;
    private float mTorque;
    private float shieldRegenTime;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        particleLeft.Pause();
        particleRight.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)) // Empujar
        {
            forceField.Push();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            forceField.Pull();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            hook.Activate();
        }

        if (mType == MovementType.EVERYDIRECTION)
        {
            EveryDirectionMovementBehaviour();
        }
        else
        {
            ForwardAndRotationMovementBehaviour();
        }
        Boost();
        ShieldRegeneration();
    } // Update

    private void ShieldRegeneration()
    {
        if(shield < maxShield)
        {
            shieldRegenTime += Time.deltaTime;
            if(shieldRegenTime > shieldRegenCooldown)
            {
                shield = Mathf.Clamp(shield + shieldRegenAmount, 0, maxShield);
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        mDir = Vector2.zero;
        mTorque = 0;
    }

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
        if (Input.GetKey(KeyCode.W))
        {
            mDir.y = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            mDir.x = -1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            mDir.y = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            mDir.x = 1;
        }
    }
    private void ForwardAndRotationMovementBehaviour()
    {
        if (Input.GetKey(KeyCode.W))
        {
            mDir = transform.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            mTorque = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            mDir = -transform.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            mTorque = -1;
        }
    }

    void Move()
    {
        body.AddForce(mDir.normalized * forwardForce * mBoost);
    } // Move

    void Rotate()
    {
        body.AddTorque(mTorque * angularForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision " + collision.relativeVelocity);
        float collisionValue = Math.Abs(collision.relativeVelocity.x) + Math.Abs(collision.relativeVelocity.y);
        //collisionValue /= 10;
        collisionValue *= collision.rigidbody.mass;
        //Debug.Log("collision value " + collisionValue);
        TakeDamage(collisionValue);
        // Podemos usar esto para calcular los impactos?
    }

    private void TakeDamage(float collisionValue)
    {
        Debug.Log("damage: " + collisionValue + " shield: " + shield + " health: " + health);
        shieldRegenTime = 0;
        if(shield > 0)
        {
            shield = Mathf.Max(0, shield - collisionValue);
            if(shield == 0)
            {
                // Algo visual para mostrar que el escudo se ha agotado?
                Debug.Log("Escudo hace piu");
            }
        }
        else
        {
            health = Mathf.Max(0, health - collisionValue);
            if(health == 0)
            {
                // Muriose la nave
                Debug.Log("Nave hace bum");
            }
        }
    }
}
