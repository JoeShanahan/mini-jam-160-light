using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunState
{
    public int CurrentWorld;
    public float[] CurrentTimes;
    public float[] BestTimes;
    public float[] ResetTimes;
    public float RunTime;
}
