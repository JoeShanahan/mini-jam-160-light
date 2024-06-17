using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJam160.PostJam
{
    [System.Serializable]
    public class JumpInfo
    {
        [Range(0f, 10f)] public float height = 2f;
        [Range(0f, 100f)] public float extraJumpSpeed = 20f;
        [Range(0f, 100f)] public float extraJumpLift = 20f;

        [HideInInspector] public bool hasExtraJump;
        [HideInInspector] public int stepsSinceLastJump;
        [HideInInspector] public bool isRequested;

        public float Speed => Mathf.Sqrt(-2f * Physics2D.gravity.y * height);
    }
}