using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum StreamerEvent {
    Death,
    EnemyKill,
    LevelComplete,
    Invincibility,
    HighSpeed,
    ObjectDestroyed,
    BombJumpExecuted
}

public class StreamerCam : MonoBehaviour {

    private static StreamerCam _instance;
    private Image _image;
    public Sprite defaultSprite;
    public Sprite deathSprite;
    public Sprite enemyKillSprite;
    public Sprite levelCompleteSprite;
    public Sprite invincibilitySprite;
    public Sprite highSpeedSprite;
    public Sprite objectDestroyedSprite;
    public Sprite bombJumpExecutedSprite;

    void Awake() {

        _instance = this;
        _image = GetComponent<Image>();
    }

    public static void NotifyStreamer(StreamerEvent thing) {
        _instance.React(thing);
    }

    private void React(StreamerEvent thing) {
        Debug.Log($"The streamer sees the {thing}");
        StartCoroutine(ChangeImageTemporarily(thing));
    }

    private IEnumerator ChangeImageTemporarily(StreamerEvent thing) {
        Sprite newSprite = defaultSprite;

        switch (thing) {
            case StreamerEvent.Death:
                newSprite = deathSprite;
                break;
            case StreamerEvent.EnemyKill:
                newSprite = enemyKillSprite;
                break;
            case StreamerEvent.LevelComplete:
                newSprite = levelCompleteSprite;
                break;
            case StreamerEvent.Invincibility:
                newSprite = invincibilitySprite;
                break;
            case StreamerEvent.HighSpeed:
                newSprite = highSpeedSprite;
                break;
            case StreamerEvent.ObjectDestroyed:
                newSprite = objectDestroyedSprite;
                break;
            case StreamerEvent.BombJumpExecuted:
                newSprite = bombJumpExecutedSprite;
                break;
        }

        _image.sprite = newSprite;
        yield return new WaitForSeconds(1f);
        _image.sprite = defaultSprite;
    }
}