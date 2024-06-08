using UnityEngine;

public enum StreamerEvent
{
    Death,
    EnemyKill,
    LevelComplete
}

public class StreamerCam : MonoBehaviour
{
    private static StreamerCam _instance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _instance = this;
    }
    
    public static void NotifyStreamer(StreamerEvent thing)
    {
        _instance.React(thing);
    }

    private void React(StreamerEvent thing)
    {
        Debug.Log($"The streamer sees the {thing}");
    }
}
