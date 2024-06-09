using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public float explosionDelay = 2f;
    public float explosionRadius = 5f;
    public int damage = 1;
    public LayerMask destructibleLayer;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public GameObject bombRadiusGraphics; 
    public float growthDuration = 1f; 
    public Color initialColor = Color.yellow; 
    public Color finalColor = Color.red; 

    private void Start() {
        StartCoroutine(GrowBombRadiusGraphics());
        Invoke("Explode", explosionDelay);
    }

    private IEnumerator GrowBombRadiusGraphics() {
        if (bombRadiusGraphics != null) {
            Vector3 initialScale = Vector3.zero;
            Vector3 finalScale = new Vector3(explosionRadius * 2f, explosionRadius * 2f, 1f);
            float elapsedTime = 0f;
            SpriteRenderer spriteRenderer = bombRadiusGraphics.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null) {
                spriteRenderer.color = initialColor;
            }

            while (elapsedTime < growthDuration) {
                float t = elapsedTime / growthDuration;
                bombRadiusGraphics.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
                if (spriteRenderer != null) {
                    spriteRenderer.color = Color.Lerp(initialColor, finalColor, t);
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            bombRadiusGraphics.transform.localScale = finalScale;
            if (spriteRenderer != null) {
                spriteRenderer.color = finalColor;
            }
        }
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