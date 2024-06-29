using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJam160.PostJam
{
    [System.Serializable]
    public class GroundInfo
    {
        [Range(0f, 90f)] public float maxSlopeAngle = 25f;
        [Range(0f, 100f)] public float maxSnapSpeed = 100f;
	    [Min(0f)] public float probeDistance = 1f;

        [HideInInspector] public int stepsSinceLastGrounded;
        [HideInInspector] public float minGroundDotProduct;

        [HideInInspector] public Vector2 contactNormal;
        [HideInInspector] public int groundContactCount;

        [HideInInspector] public Vector2 wallNormal;
        [HideInInspector] public int wallContactCount;

        public bool IsGrounded()
        {
            return groundContactCount > 0;
        }

        public void OnValidate()
        {
            minGroundDotProduct = Mathf.Cos(maxSlopeAngle * Mathf.Deg2Rad);
        }
    }
}