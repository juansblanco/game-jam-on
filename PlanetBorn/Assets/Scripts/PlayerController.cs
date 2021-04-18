using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MovementType
    {
        EVERYDIRECTION,
        FORWARDANDROTATION
    }

    public GameObject UI;

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
    public Barrier barrier;

    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float shield;
    public float maxShield;
    public float shieldRegenCooldown;
    public float shieldRegenAmount;

    [Header("Timers")]
    public float hookCooldown = 10f;

    [Header("Propulsion particles")]
    public ParticleSystem particleLeft;
    public ParticleSystem particleRight;

    // Private
    private Rigidbody2D body;
    private Vector2 mDir;
    private float mTorque;
    private float shieldRegenTimer;
    private float hookTimer = 0;
    private HealthBar healthBar;
    private ShieldBar shieldBar;

    // Particles 
    private Vector2 minMaxEmitter = new Vector2(0, 5);
    private Vector2 minMaxSpeedDesire = new Vector2(0, 7);

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
        hookTimer = Mathf.Max(0, hookTimer - Time.deltaTime);
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
            if(hookTimer == 0 || hook.gameObject.activeSelf)
            {
                Debug.Log("hook " + hook);
                if (!hook.gameObject.activeSelf)
                {
                    hook.gameObject.SetActive(true);
                }
                hook.Activate();
                hookTimer = hookCooldown;
            }
            Debug.Log("hookTimer " + hookTimer);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Barrera");
            
            barrier.Activate();
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
            shieldRegenTimer += Time.deltaTime;
            if(shieldRegenTimer > shieldRegenCooldown)
            {
                shield = Mathf.Clamp(shield + shieldRegenAmount, 0, maxShield);
                shieldBar.SetShield(shield);
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
        int particleToEmit = 0;
        float speed = body.velocity.magnitude;
        Debug.Log("Velochita: " + speed);
        float t = Math.Abs((speed - minMaxSpeedDesire.x) / minMaxSpeedDesire.y);
        Debug.Log("Valor de t: " + t);
        particleToEmit = (int)Mathf.Lerp(minMaxEmitter.x, minMaxEmitter.y, t);

        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log(particleToEmit);
            particleLeft.Emit(particleToEmit);
            particleRight.Emit(particleToEmit);
            mDir.y = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            particleRight.Emit(particleToEmit + 2);
            particleLeft.Emit(particleToEmit - 1);
            mDir.x = -1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            mDir.y = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            particleLeft.Emit(particleToEmit + 2);
            particleRight.Emit(particleToEmit - 1);
            mDir.x = 1;
        }
    }
    private void ForwardAndRotationMovementBehaviour()
    {
        int particleToEmit = 0;
        float speed = body.velocity.magnitude;
        //Debug.Log("Velochita: " + speed);
        float t = (speed - minMaxSpeedDesire.x) / (minMaxSpeedDesire.y);
        //Debug.Log("Valor de t: " + t);
        particleToEmit = (int)Mathf.Lerp(minMaxEmitter.x, minMaxEmitter.y, t);

        if (Input.GetKey(KeyCode.W))
        {
            //Debug.Log("Emitter value uwu: " + particleToEmit);
            particleLeft.Emit(particleToEmit);
            particleRight.Emit(particleToEmit);
            mDir = transform.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //Debug.Log("Emitter value uwu: " + particleToEmit);
            particleRight.Emit(particleToEmit + 2);
            particleLeft.Emit(particleToEmit - 1);
            mTorque = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            mDir = -transform.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Debug.Log("Emitter value uwu: " + particleToEmit);
            particleLeft.Emit(particleToEmit + 2);
            particleRight.Emit(particleToEmit - 1);
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
        if (collision.gameObject.GetComponent<Asteroid>())
        {
            Debug.Log("Parate");
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        // Podemos usar esto para calcular los impactos?
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Asteroid>())
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void TakeDamage(float collisionValue)
    {
        Debug.Log("damage: " + collisionValue + " shield: " + shield + " health: " + health);
        shieldRegenTimer = 0;
        if(shield > 0)
        {
            shield = Mathf.Max(0, shield - collisionValue);
            shieldBar.SetShield(shield);
            if(shield == 0)
            {
                // Algo visual para mostrar que el escudo se ha agotado?
                Debug.Log("Escudo hace piu");
            }
        }
        else
        {
            health = Mathf.Max(0, health - collisionValue);
            healthBar.SetHealth(health);
            if(health == 0)
            {
                // Muriose la nave
                Debug.Log("Nave hace bum");
            }
        }
    }

    public void UILoad()
    {
        healthBar = UI.GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth((int)maxHealth);
        healthBar.SetHealth(maxHealth);
        shieldBar = UI.GetComponentInChildren<ShieldBar>();
        shieldBar.SetMaxShield((int)maxShield);
        shieldBar.SetShield(maxShield);
    }

    public void SetHookTimer()
    {
        hookTimer = hookCooldown;
    }

    public void ResetHookTimer()
    {
        hookTimer = 0;
    }
}
