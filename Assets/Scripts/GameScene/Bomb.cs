using UnityEngine;

public class Bomb : MonoBehaviour {

    public float explosionDelay = 2f;
    public float explosionRadius = 5f;
    public int damage = 1;
    public LayerMask destructibleLayer;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;

    private void Start() {
        Invoke("Explode", explosionDelay);
    }

    private void Explode() {
        LayerMask combinedLayers = destructibleLayer | playerLayer | enemyLayer;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, combinedLayers);
        foreach (Collider2D hit in hits) {

            if (hit.CompareTag("Destructible")) {
                hit.GetComponent<Destructible>().Damage(damage);
            } 
            
            if (hit.CompareTag("Player")) {
                hit.GetComponent<PlayerController>().TakeDamage(damage, transform.position);
            }
            
            if (hit.CompareTag("Enemy")) {
                var enemyController = hit.GetComponent<EnemyController>();
                var enemyGoomba = hit.GetComponent<EnemyGoomba>();

                if (enemyController != null) {
                    enemyController.Die();
                }

                if (enemyGoomba != null) {
                    enemyGoomba.Die();
                }
            }
        }
        Destroy(gameObject);
    }
}