using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _bottomRight;
    [SerializeField] private Transform _topLeft;

    public Vector3 SpawnPosition => _spawnPoint.position;

    public Vector3 TopLeft => _topLeft.position;
    public Vector3 BottomRight => _bottomRight.position;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
