using UnityEngine;

namespace MiniJam160.PostJam
{
    public class MovingPlatform : MonoBehaviour
    {
        public enum Direction { Horizontal, Vertical };

        [SerializeField] private Direction _direction;
        [SerializeField] private float _cycleLength;
        [SerializeField] private int _range;
        [SerializeField] private BoxCollider2D _collider;

        private Vector3 _startPosition;
        private float _currentPhase;
        private Rigidbody2D _rb;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _startPosition = transform.position;
        }

        private void FixedUpdate()
        {
            _currentPhase += Time.deltaTime;

            if (_currentPhase > _cycleLength)
            {
                _currentPhase -= _cycleLength;
            }

            bool isHorz = _direction == Direction.Horizontal;
            Vector3 endPosition = _startPosition;
            endPosition += isHorz ? Vector3.right * _range : Vector3.up * _range;

            float phasePercent = _currentPhase / _cycleLength;
            float adjusted = EaseValue(Mathf.PingPong(phasePercent * 2, 1));
            _rb.MovePosition(Vector3.Lerp(_startPosition, endPosition, adjusted));
        }
        
        void OnDrawGizmos()
        {
            if (_collider == null)
                return;
            
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;

            Vector3 startPos = transform.position;
            Vector3 endPos = transform.position;

            if (Application.isPlaying)
            {
                startPos = _startPosition;
            }
            
            if (_direction == Direction.Vertical)
            {
                endPos = startPos + new Vector3(0, _range);
            }
            else
            {
                endPos = startPos + new Vector3(_range, 0);
            }
            
            DrawPlatform(startPos);
            DrawPlatform(endPos);
                
            Gizmos.DrawLine(startPos, endPos);
        }

        private void DrawPlatform(Vector3 midPoint)
        {
            float platformWidth = _collider.size.x;
            float platformHeight = _collider.size.y;
            
            Vector3 topLeft = midPoint + new Vector3(-platformWidth / 2, platformHeight / 2, 0);
            Vector3 topRight = midPoint + new Vector3(platformWidth / 2, platformHeight / 2, 0);
            Vector3 botLeft = midPoint + new Vector3(-platformWidth / 2, - platformHeight / 2, 0);
            Vector3 botRight = midPoint + new Vector3(platformWidth / 2, - platformHeight / 2, 0);
                
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(botLeft, botRight);
            Gizmos.DrawLine(topLeft, botLeft);
            Gizmos.DrawLine(topRight, botRight);
        }
        
        private float EaseValue(float inVal)
        {
            return -(Mathf.Cos(Mathf.PI * inVal) - 1) / 2;
        }
    }
}