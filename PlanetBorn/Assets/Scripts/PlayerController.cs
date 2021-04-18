using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MovementType
    {
        EVERYDIRECTION,
        FORWARDANDROTATION
    }

    public GameObject UI;

    [Header("Death")]
    public float slowMotionScale;
    public float deathTimeToWait;

    [Header("Win")]
    public float winTimeToWait;

    [Header("Movement config")]
    public float forwardForce;

    // Adding some range to keep it a bit smooth
    [Range(1, 50)]
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
    public float pushChargeMax;
    public float pullChargeMax;
    public float abilityModFactor;

    [Header("Timers")]
    public float hookCooldown = 10f;
    public float pushCooldown = 2f;
    public float pullCooldown = 2f;
    public float barrierCD = 10f;

    [Header("Particles")]
    public ParticleSystem leftPropulsor;
    public ParticleSystem rightPropulsor;
    public GameObject explosion;

    // Private
    private Rigidbody2D body;
    private Vector2 mDir;
    private float mTorque;
    private float shieldRegenTimer;
    private float hookTimer = 0;
    private float pushTimer = 0;
    private float pullTimer = 0;
    private float barrierCDTimer = 0;
    private float pushCharge;
    private float pullCharge;
    private BarController healthBar;
    private BarController shieldBar;
    private BarController pushBar;
    private BarController pullBar;
    private BarController hookBar;
    private BarController barrierBar;
    private bool canMove;
    [Header("Initial Score")] public float score = 10000;

    // Particles 
    private Vector2 minMaxEmitter = new Vector2(0, 5);
    private Vector2 minMaxSpeedDesire = new Vector2(0, 7);

    public bool CanMove { get => canMove; set => canMove = value; }

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        leftPropulsor.Pause();
        rightPropulsor.Pause();
        pushCharge = pushChargeMax;
        pullCharge = pullChargeMax;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            ManageAbilities();
            if (mType == MovementType.EVERYDIRECTION)
            {
                EveryDirectionMovementBehaviour();
            }
            else
            {
                ForwardAndRotationMovementBehaviour();
            }
            Boost();
        }
        ShieldRegeneration();
        AbilitiesRegeneration();
        ManageTimers();
    } // Update

    private void ManageAbilities()
    {
        if (Input.GetKey(KeyCode.E)) // Empujar
        {
            Debug.Log("push charge " + pushCharge);
            if(pushCharge > 0)
            {
                forceField.Push();
                pushCharge = Mathf.Max(0, pushCharge - (abilityModFactor * Time.deltaTime));
                pushBar.SetValue(pushCharge);
                pushTimer = pushCooldown;
            }
        }

        if (Input.GetKey(KeyCode.Q)) // Atraer
        {
            Debug.Log("pull charge " + pullCharge);
            if (pullCharge > 0)
            {
                forceField.Pull();
                pullCharge = Mathf.Max(0, pullCharge - (abilityModFactor * Time.deltaTime));
                pullBar.SetValue(pullCharge);
                pullTimer = pullCooldown;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (hookTimer == 0 || hook.gameObject.activeSelf)
            {
                Debug.Log("hook " + hook);
                if (!hook.gameObject.activeSelf)
                {
                    hook.gameObject.SetActive(true);
                }
                hook.Activate();
                hookTimer = hookCooldown;
                hookBar.SetValue(hookCooldown-hookTimer);
            }
            Debug.Log("hookTimer " + hookTimer);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (barrierCDTimer == 0 || barrier.gameObject.activeSelf)
            {
                if (!barrier.gameObject.activeSelf)
                {
                    barrier.gameObject.SetActive(true);
                }
                else
                {
                    barrier.gameObject.SetActive(false);
                }
                barrierCDTimer = barrierCD;
                barrierBar.SetValue(barrierCD-barrierCDTimer);
            }
            //Debug.Log("BarreraCD " + barrierCDTimer);
        }
    }

    private void AbilitiesRegeneration()
    {
        //Debug.Log("push charge: " + pushCharge + " pull charge: " + pullCharge);
        if(pullCharge < pullChargeMax && pullTimer == 0)
        {
            pullCharge = Mathf.Min(pullChargeMax, pullCharge + (abilityModFactor * Time.deltaTime));
            pullBar.SetValue(pullCharge);
        }       
        if (pushCharge < pushChargeMax && pushTimer == 0)
        {
            pushCharge = Mathf.Min(pushChargeMax, pushCharge + (abilityModFactor * Time.deltaTime));
            pushBar.SetValue(pushCharge);
        }
    }

    private void ManageTimers()
    {
        hookTimer = Mathf.Max(0, hookTimer - Time.deltaTime);
        hookBar.SetValue(hookCooldown-hookTimer);
        pushTimer = Mathf.Max(0, pushTimer - Time.deltaTime);
        pullTimer = Mathf.Max(0, pullTimer - Time.deltaTime);
        barrierCDTimer = Mathf.Max(0, barrierCDTimer - Time.deltaTime);
        barrierBar.SetValue(barrierCD-barrierCDTimer);
        score -= 1;
    }

    private void ShieldRegeneration()
    {
        if(shield < maxShield)
        {
            shieldRegenTimer += Time.deltaTime;
            if(shieldRegenTimer > shieldRegenCooldown)
            {
                shield = Mathf.Clamp(shield + shieldRegenAmount, 0, maxShield);
                shieldBar.SetValue(shield);
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
            leftPropulsor.Emit(particleToEmit);
            rightPropulsor.Emit(particleToEmit);
            mDir.y = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rightPropulsor.Emit(particleToEmit + 2);
            leftPropulsor.Emit(particleToEmit - 1);
            mDir.x = -1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            mDir.y = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            leftPropulsor.Emit(particleToEmit + 2);
            rightPropulsor.Emit(particleToEmit - 1);
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
            leftPropulsor.Emit(particleToEmit);
            rightPropulsor.Emit(particleToEmit);
            mDir = transform.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //Debug.Log("Emitter value uwu: " + particleToEmit);
            rightPropulsor.Emit(particleToEmit + 2);
            leftPropulsor.Emit(particleToEmit - 1);
            mTorque = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            mDir = -transform.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Debug.Log("Emitter value uwu: " + particleToEmit);
            leftPropulsor.Emit(particleToEmit + 2);
            rightPropulsor.Emit(particleToEmit - 1);
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
        if (collision.gameObject.GetComponent<Asteroid>().aColor == Asteroid.AsteroidColor.RED)
        {
            collisionValue *= 2;
        }
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

    public void TakeDamage(float collisionValue)
    {
        score -= collisionValue;
        Debug.Log("damage: " + collisionValue + " shield: " + shield + " health: " + health);
        shieldRegenTimer = 0;
        if(shield > 0)
        {
            shield = Mathf.Max(0, shield - collisionValue);
            shieldBar.SetValue(shield);
            if(shield == 0)
            {
                // Algo visual para mostrar que el escudo se ha agotado?
                Debug.Log("Escudo hace piu");
            }
        }
        else
        {
            health = Mathf.Max(0, health - collisionValue);
            healthBar.SetValue(health);
            if(health == 0)
            {
                // Muriose la nave
                Debug.Log("Nave hace bum");
                Death();
            }
        }
    }

    private void Death()
    {
        canMove = false;
        Time.timeScale = slowMotionScale;
        GetComponent<SpriteRenderer>().sprite = null;
        Instantiate(explosion, transform.position, Quaternion.identity);
        StartCoroutine(LoseGame());
    }

    IEnumerator LoseGame()
    {
        yield return new WaitForSeconds(deathTimeToWait);
        Time.timeScale = 1;
        UI.GetComponentInChildren<UIController>().ShowLoseWindow();
        UI.GetComponentInChildren<UIController>().HideHUDWindow();
        Destroy(gameObject);
    }

    public IEnumerator WinGame()
    {
        yield return new WaitForSeconds(winTimeToWait);
        UI.GetComponentInChildren<UIController>().ShowWinWindow(score);
    }

    public void UILoad()
    {

        Transform container = null;

        foreach(Transform child in UI.transform.GetChild(0))
        {
            if(child.name == "HUD Window")
            {
                container = child.GetChild(0);
            }
        }

        int x = container.childCount;
        for (int i = 0; i < x; i++)
        {
            BarController bar = container.GetChild(i).gameObject.GetComponent<BarController>();

            if (bar.getType() == BarController.BarType.HEALTH)
            {
                healthBar = bar;
                healthBar.SetMaxValue((int)maxHealth);
                healthBar.SetValue(maxHealth);
                Debug.Log("isHealth");
            }
            if (bar.getType() == BarController.BarType.SHIELD)
            {
                shieldBar = bar;
                shieldBar.SetMaxValue((int)maxShield);
                shieldBar.SetValue(maxShield);
                Debug.Log("isShield");
            }
            if (bar.getType() == BarController.BarType.PUSH)
            {
                pushBar = bar;
                pushBar.SetMaxValue((int)pushChargeMax);
                pushBar.SetValue(pushChargeMax);
                Debug.Log("isPush");
            }
            if (bar.getType() == BarController.BarType.PULL)
            {
                pullBar = bar;
                pullBar.SetMaxValue((int)pullChargeMax);
                pullBar.SetValue(pullChargeMax);
            }
            if (bar.getType() == BarController.BarType.HOOK)
            {
                hookBar = bar;
                hookBar.SetMaxValue((int)hookCooldown);
                hookBar.SetValue(hookCooldown);
            }
            if (bar.getType() == BarController.BarType.BARRIER)
            {
                barrierBar = bar;
                barrierBar.SetMaxValue((int)barrierCD);
                barrierBar.SetValue(barrierCD);
            }
        }
        
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
