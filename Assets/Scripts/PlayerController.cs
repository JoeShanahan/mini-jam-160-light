using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AbilityType {
    None,
    Boost,
    Rocket,
    Freeze,
    Bomb,
    Invincible
}

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public int maxJumps = 2;
    public bool canDoubleJump = true;
    public int health = 1;

    [SerializeField] private AbilityType _equippedAbility;
    [SerializeField] private AbilityData _abilityData;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    [SerializeField] private bool isGrounded;
    private int remainingJumps;
    private InputSystem_Actions controls;
    private Vector3 _respawnPoint;

    private Dictionary<AbilityType, float> abilityTimers = new Dictionary<AbilityType, float>();

    [SerializeField] private float currentXVelocity;
    [SerializeField] private float currentYVelocity;

    private bool isHorizontalBoosting;
    private bool isVerticalBoosting;

    private float lastHorizontalDirection;

    [SerializeField] private float speedThreshold = 15f;
    [SerializeField] private float decelerationStrength = 0.5f;

    private bool isInvincible;
    private bool isCollidingWithDanger;

    private EnemyController[] enemies;
    private MovingPlatform[] platforms;
    private Crusher[] crushers;

    public GameObject bombPrefab;
    public Transform bombSpawnPoint;
    public float explosionForce = 10f;

    public Collider2D CrushCheckTop;
    public Collider2D CrushCheckBottom;

    private bool isCrushed = false;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controls = new InputSystem_Actions();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => OnJump();
        controls.Player.Interact.performed += ctx => OnAbilityPressed();

        enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        platforms = FindObjectsByType<MovingPlatform>(FindObjectsSortMode.None);
        crushers = FindObjectsByType<Crusher>(FindObjectsSortMode.None);
    }

    void OnEnable() {
        controls.Player.Enable();
    }

    void OnDisable() {
        controls.Player.Disable();
    }

    void Update() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, LayerMask.GetMask("Destructible", "SpikePlatform", "MovingPlatform", "Crusher", "Ground"));
        if (isGrounded) {
            remainingJumps = maxJumps;
        }

        if (transform.position.y < -10f) {
            Die();
        }

        if (!isHorizontalBoosting) {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        }

        ApplyDeceleration();

        if (!isVerticalBoosting) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        currentXVelocity = rb.velocity.x;
        currentYVelocity = rb.velocity.y;

        if (moveInput.x != 0) {
            lastHorizontalDirection = Mathf.Sign(moveInput.x);
        }

        List<AbilityType> keys = new List<AbilityType>(abilityTimers.Keys);
        foreach (AbilityType ability in keys) {
            abilityTimers[ability] -= Time.deltaTime;
            if (abilityTimers[ability] <= 0) {
                abilityTimers.Remove(ability);
            }
        }

        CrushCheck();
    }

    void OnJump() {
        if (isGrounded || (canDoubleJump && remainingJumps > 0)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            remainingJumps--;
        }
    }

    public void TakeDamage(int damage, Vector2 explosionPosition) {
        if (!isInvincible) {
            health -= damage;

            if (health <= 0) {
                Die();
            }
        } else {
            Vector2 explosionDirection = (transform.position - (Vector3)explosionPosition).normalized;
            rb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);
            Debug.Log("Bomb-jump shinanegans");
        }
    }

    void Die() {
        if (isInvincible) {
            Debug.Log("Player can't die, go wild!");
            return;
        }

        Debug.Log("You died :(");
        Respawn();
    }

    void Respawn() {
        transform.position = _respawnPoint;
        rb.velocity = Vector2.zero;
        isCrushed = false;
        isInvincible = false;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Enemy")) {
            isCollidingWithDanger = true;
            if (!isInvincible) {
                Die();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Enemy")) {
            isCollidingWithDanger = false;
        }
    }

    public void DebugUseAbility(int ability) {
        _equippedAbility = (AbilityType) ability;
        OnAbilityPressed();
    }

    private void OnAbilityPressed() {
        if (abilityTimers.ContainsKey(_equippedAbility)) {
            Debug.Log($"{_equippedAbility} is on cooldown!");
            return;
        }

        float power = _abilityData.GetPower(_equippedAbility);

        if (_equippedAbility == AbilityType.Boost)
            DoHorizontalBoost(power);
        else if (_equippedAbility == AbilityType.Rocket)
            DoVerticalBoost(power);
        else if (_equippedAbility == AbilityType.Freeze)
            DoFreeze(power);
        else if (_equippedAbility == AbilityType.Bomb)
            DoBomb(power);
        else if (_equippedAbility == AbilityType.Invincible)
            DoInvincibility(power);
    }

    private void DoFreeze(float time) {
        Debug.Log($"Freeze for {time} seconds!");

        foreach (var enemy in enemies) {
            enemy.Freeze(time);
        }

        foreach (var platform in platforms) {
            platform.Freeze(time);
        }

        foreach (var crusher in crushers) {
            crusher.Freeze(time);
        }

        StartCooldown(AbilityType.Freeze);
    }

    private void DoBomb(float power) {
        Debug.Log("Bomb!");

        GameObject bombInstance = Instantiate(bombPrefab, bombSpawnPoint.position, bombSpawnPoint.rotation);

        Rigidbody2D bombRb = bombInstance.GetComponent<Rigidbody2D>();

        Vector2 playerVelocity = rb.velocity;
        Vector2 throwForce = new Vector2(power * transform.localScale.x, power);
        bombRb.velocity = playerVelocity + throwForce;

        StartCooldown(AbilityType.Bomb);
    }

    private void StartCooldown(AbilityType ability) {
        float cooldown = _abilityData.GetCooldown(ability);
        abilityTimers[ability] = cooldown;
    }

    private void ApplyDeceleration() {
        if (Mathf.Abs(rb.velocity.x) > speedThreshold) {
            float deceleration = decelerationStrength * Time.deltaTime * Mathf.Sign(rb.velocity.x);
            float newVelocityX = rb.velocity.x - deceleration;

            if (Mathf.Abs(newVelocityX) < speedThreshold) {
                newVelocityX = speedThreshold * Mathf.Sign(rb.velocity.x);
            }

            rb.velocity = new Vector2(newVelocityX, rb.velocity.y);
        }
    }

    private void DoHorizontalBoost(float power) {
        Debug.Log("Boost!");

        float dashDirection = moveInput.x == 0 ? lastHorizontalDirection : Mathf.Sign(moveInput.x);

        rb.velocity = new Vector2(dashDirection * power, rb.velocity.y);

        StartCooldown(AbilityType.Boost);

        isHorizontalBoosting = true;
        StartCoroutine(EndHorizontalBoostAfterTime(0.5f));
    }

    private IEnumerator EndHorizontalBoostAfterTime(float duration) {
        yield return new WaitForSeconds(duration - 0.1f);
        isHorizontalBoosting = false;
    }

    private void DoVerticalBoost(float power) {
        Debug.Log("Rocket!");

        rb.velocity = new Vector2(rb.velocity.x, power);

        StartCooldown(AbilityType.Rocket);

        isVerticalBoosting = true;
        StartCoroutine(EndVerticalBoostAfterTime(0.5f));
    }

    private IEnumerator EndVerticalBoostAfterTime(float duration) {
        yield return new WaitForSeconds(duration);
        isVerticalBoosting = false;
    }

    public void SetSpawnPosition(Vector3 position) {
        _respawnPoint = position;
        transform.position = position;
    }

    private void DoInvincibility(float time) {
        Debug.Log($"Invincible for {time} seconds!");

        isInvincible = true;

        StartCoroutine(InvincibilityDuration(time));

        StartCooldown(AbilityType.Invincible);
    }

    private IEnumerator InvincibilityDuration(float time) {
        yield return new WaitForSeconds(time);
        isInvincible = false;
        if (isCollidingWithDanger) {
            Die();
        }
    }

    private void CrushCheck() {
        if (CrushCheckTop.IsTouchingLayers(LayerMask.GetMask("Crusher")) && CrushCheckBottom.IsTouchingLayers(LayerMask.GetMask("Crusher"))) {
            if (!isCrushed) {
                Die();
                isCrushed = true;
            }
        }
    }
}