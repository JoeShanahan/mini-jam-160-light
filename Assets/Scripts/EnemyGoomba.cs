using UnityEngine;
using System.Collections;

public class EnemyGoomba : MonoBehaviour {

    public float speed = 2f;
    private bool movingLeft = true;
    private Rigidbody2D rb;
    public int health = 1;
    private float originalGravityScale;
    private bool isFrozen = false;
    public Transform groundCheck;
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    void Update() {
        if (!isFrozen) {
            Move();
            CheckGround();
        }
    }

    void Move() {
        if (movingLeft) {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    void CheckGround() {
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, direction, groundCheckDistance, groundLayer);
        RaycastHit2D groundBelowInfo = Physics2D.Raycast(groundCheck.position + new Vector3(direction.x * groundCheckDistance, 0, 0), Vector2.down, groundCheckDistance, groundLayer);

        if (groundInfo.collider == null || groundBelowInfo.collider == null) {
            movingLeft = !movingLeft;
            Flip();
        }
    }

    void Flip() {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (collision.transform.position.y > transform.position.y + 0.5f) {
                Die();
            }
        } else {
            movingLeft = !movingLeft;
            Flip();
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    public void Die() {
        Debug.Log("Enemy unalived :O");
        Destroy(gameObject);
    }

    public void Freeze(float time) {
        StartCoroutine(FreezeRoutine(time));
    }

    private IEnumerator FreezeRoutine(float time) {
        isFrozen = true;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(time);
        rb.gravityScale = originalGravityScale;
        isFrozen = false;
    }
}