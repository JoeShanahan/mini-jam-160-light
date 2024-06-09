using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Transform[] waypoints;
    public float speed = 2f;

    private int currentWaypointIndex = 0;
    private bool movingForward = true;
    private Transform playerTransform;
    private bool isFrozen = false;

    void Update() {
        if (!isFrozen) {
            MoveTowardsWaypoint();
        }
    }

    void MoveTowardsWaypoint() {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        float distance = speed * Time.deltaTime;

        if (direction.magnitude <= distance) {
            transform.position = targetWaypoint.position;
            UpdateWaypointIndex();
        } else {
            transform.Translate(direction.normalized * distance, Space.World);
        }
    }

    void UpdateWaypointIndex() {
        if (movingForward) {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length) {
                currentWaypointIndex = waypoints.Length - 2;
                movingForward = false;
            }
        } else {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0) {
                currentWaypointIndex = 1;
                movingForward = true;
            }
        }
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
        StartCoroutine(FreezeRoutine(time));
    }

    private IEnumerator FreezeRoutine(float time) {
        isFrozen = true;
        yield return new WaitForSeconds(time);
        isFrozen = false;
    }
}