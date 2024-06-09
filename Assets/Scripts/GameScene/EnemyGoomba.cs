using UnityEngine;
using System.Collections;

public class EnemyGoomba : MonoBehaviour {

    public float speed = 2f;
    private bool movingLeft = true;
    private Rigidbody2D rb;
    public int health = 1;
    private float originalGravityScale;
    private bool isFrozen = false;
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public Transform leftCheck;
    public Transform rightCheck;
    public float groundCheckDistance = 1f;
    public float sideCheckDistance = 0.5f;
    public LayerMask groundLayer;
    public bool enableGroundDetection = true;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    void Update() {
        if (!isFrozen) {
            Move();
            if (enableGroundDetection) {
                CheckGroundLeft();
                CheckGroundRight();
            }
            CheckLeft();
            CheckRight();
        }
    }

    void Move() {
        if (movingLeft) {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    void CheckGroundLeft() {
        RaycastHit2D groundInfoLeft = Physics2D.Raycast(groundCheckLeft.position, Vector2.down, groundCheckDistance, groundLayer);

        if (groundInfoLeft.collider == null && movingLeft) {
            Debug.Log("No ground detected on the left. Turning around.");
            movingLeft = !movingLeft;
            Flip();
        }
    }

    void CheckGroundRight() {
        RaycastHit2D groundInfoRight = Physics2D.Raycast(groundCheckRight.position, Vector2.down, groundCheckDistance, groundLayer);

        if (groundInfoRight.collider == null && !movingLeft) {
            Debug.Log("No ground detected on the right. Turning around.");
            movingLeft = !movingLeft;
            Flip();
        }
    }

    void CheckLeft() {
        RaycastHit2D leftInfo = Physics2D.Raycast(leftCheck.position, Vector2.left, sideCheckDistance, groundLayer);

        if (leftInfo.collider != null) {
            Debug.Log("Obstacle detected on the left. Turning around.");
            movingLeft = false;
            Flip();
        }
    }

    void CheckRight() {
        RaycastHit2D rightInfo = Physics2D.Raycast(rightCheck.position, Vector2.right, sideCheckDistance, groundLayer);

        if (rightInfo.collider != null) {
            Debug.Log("Obstacle detected on the right. Turning around.");
            movingLeft = true;
            Flip();
        }
    }

    void Flip() {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        groundCheckLeft.localPosition = new Vector3(-groundCheckLeft.localPosition.x, groundCheckLeft.localPosition.y, groundCheckLeft.localPosition.z);
        groundCheckRight.localPosition = new Vector3(-groundCheckRight.localPosition.x, groundCheckRight.localPosition.y, groundCheckRight.localPosition.z);
        leftCheck.localPosition = new Vector3(-leftCheck.localPosition.x, leftCheck.localPosition.y, leftCheck.localPosition.z);
        rightCheck.localPosition = new Vector3(-rightCheck.localPosition.x, rightCheck.localPosition.y, rightCheck.localPosition.z);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (collision.transform.position.y > transform.position.y + 0.5f) {
                Die();
            }
        } else {
            if (enableGroundDetection) {
                movingLeft = !movingLeft;
                Flip();
            }
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