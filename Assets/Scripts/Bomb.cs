using UnityEngine;

public class Bomb : MonoBehaviour {

    public float explosionDelay = 2f;
    public float explosionRadius = 5f;
    public int damage = 1;
    public LayerMask destructibleLayer;
    public LayerMask playerLayer;

    private void Start() {
        Invoke("Explode", explosionDelay);
    }

    private void Explode() {
        LayerMask combinedLayers = destructibleLayer | playerLayer;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, combinedLayers);
        foreach (Collider2D hit in hits) {
            if (hit.CompareTag("Destructible")) {
                hit.GetComponent<Destructible>().Damage(damage);
            } else if (hit.CompareTag("Player")) {
                hit.GetComponent<PlayerController>().TakeDamage(damage, transform.position);
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}