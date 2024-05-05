using System;
using UnityEngine;

[Serializable]
public class ClipAndScale
{
    public AudioClip clip;
    [Range(0, 1)] public float scale = 1.0f;
}
