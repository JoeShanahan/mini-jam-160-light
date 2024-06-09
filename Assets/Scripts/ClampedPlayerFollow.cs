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
    
    // streamer overlay is 275 width
    private float HorizontalBlockCount
    {
        get
        {
            float widthMultiplier = Screen.width / (float) Screen.height;
            float initialWidth = VerticalBlockCount * widthMultiplier;
            float remaining = 1 - StreamerPercentOfScreenCovered;
            
            return initialWidth * remaining;
        }
    }

    private float StreamerPercentOfScreenCovered
    {
        get
        {
            float widthMultiplier = Screen.width / (float) Screen.height;
            float streamerOverlayWidth = 275;
            float canvasPixels = 720 * widthMultiplier;
            return streamerOverlayWidth / canvasPixels;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float xPos = _player.transform.position.x;
        float yPos = _player.transform.position.y;

        float lowerBound = _botRight.y + (VerticalBlockCount / 2f);
        float upperBound = _topLeft.y - (VerticalBlockCount / 2f);

        float horzCount = HorizontalBlockCount;
        float rightBound = _botRight.x - (horzCount / 2f);
        float leftBound = _topLeft.x + (horzCount / 2f);

        float blocksPerPixel = 720 / VerticalBlockCount;
        float blocksCovered = 275 / blocksPerPixel;
        leftBound -= blocksCovered / 2f;
        
        if (lowerBound > upperBound)
        {
            upperBound = lowerBound = (upperBound + lowerBound) / 2;
        }
        
        if (leftBound > rightBound)
        {
            leftBound = rightBound = (leftBound + rightBound) / 2;
        }

        xPos = Mathf.Clamp(xPos, leftBound, rightBound);
        yPos = Mathf.Clamp(yPos, lowerBound, upperBound);

        transform.position = new Vector3(xPos, yPos, 0);
    }

    public void SetBounds(LevelContainer level)
    {
        _topLeft = level.TopLeft;
        _botRight = level.BottomRight;
    }
}
