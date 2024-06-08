using UnityEngine;

public class Destructible : MonoBehaviour {

    public int health = 1;

    public void Damage(int damageAmount) {
        health -= damageAmount;
        if (health <= 0) {
            Destroy(gameObject);
            StreamerCam.NotifyStreamer(StreamerEvent.ObjectDestroyed);
        }
    }
}