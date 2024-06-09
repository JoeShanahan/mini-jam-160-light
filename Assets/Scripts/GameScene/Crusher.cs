using UnityEngine;
using System.Collections;

public class Crusher : MonoBehaviour {

    public float moveDistance = 5f;
    public float moveDuration = 2f;
    public float pauseDuration = 0.5f;
    public float initialDelay = 1f;

    private Vector3 startingPosition;
    private Transform playerTransform;
    private bool isFrozen = false;
    private Coroutine moveCoroutine;

    void Start() {
        startingPosition = transform.position;
        moveCoroutine = StartCoroutine(MoveCrusher());
    }

    IEnumerator MoveCrusher() {
        yield return new WaitForSeconds(initialDelay);

        while (true) {
            yield return new WaitForSeconds(pauseDuration);
            if (isFrozen) yield return new WaitUntil(() => !isFrozen);
            yield return MoveToPosition(startingPosition + Vector3.up * moveDistance, moveDuration);
            yield return new WaitForSeconds(pauseDuration);
            if (isFrozen) yield return new WaitUntil(() => !isFrozen);
            yield return MoveToPosition(startingPosition, moveDuration);
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition, float duration) {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            if (isFrozen) {
                yield return new WaitUntil(() => !isFrozen);
                initialPosition = transform.position;
                elapsedTime = 0f; 
            }
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerTransform = other.transform;
            playerTransform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerTransform.SetParent(null);
            playerTransform = null;
        }
    }

    public void Freeze(float time) {
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
        StartCoroutine(FreezeRoutine(time));
    }

    private IEnumerator FreezeRoutine(float time) {
        isFrozen = true;
        yield return new WaitForSeconds(time);
        isFrozen = false;
        moveCoroutine = StartCoroutine(MoveCrusher());
    }
}