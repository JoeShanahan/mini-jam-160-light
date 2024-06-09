using UnityEngine;

public class ClampedPlayerFollow : MonoBehaviour
{
    private PlayerController _player;
    private Vector3 _topLeft = new Vector3(-999, 999);
    private Vector3 _botRight = new Vector3(999, -999);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindFirstObjectByType<PlayerController>();
    }

    private float VerticalBlockCount => 30;

    // Update is called once per frame
    void Update()
    {
        float xPos = _player.transform.position.x;
        float yPos = _player.transform.position.y;


        float lowerBound = _botRight.y + (VerticalBlockCount / 2f);
        float upperBound = _topLeft.y - (VerticalBlockCount / 2f);
        
        if (lowerBound > upperBound)
        {
            upperBound = lowerBound = (upperBound + lowerBound) / 2;
        }

        yPos = Mathf.Clamp(yPos, lowerBound, upperBound);

        transform.position = new Vector3(xPos, yPos, 0);
    }

    public void SetBounds(LevelContainer level)
    {
        _topLeft = level.TopLeft;
        _botRight = level.BottomRight;
    }
}
