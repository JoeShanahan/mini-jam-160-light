using UnityEngine;
using System.Collections;

public class EnemyGoomba : MonoBehaviour {

    public float speed = 2f;
    private bool movingLeft = true;
    private Rigidbody2D rb;
    public int health = 1;
    private float originalGravityScale;
    private bool isFrozen = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (!isFrozen) {
            Move();
        }        
    }

    void Move() {
        if (movingLeft) {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") == false) {
            movingLeft = !movingLeft;
        }

        if (collision.gameObject.CompareTag("Player")) {
            if (collision.transform.position.y > transform.position.y + 0.5f) {
                Die();
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